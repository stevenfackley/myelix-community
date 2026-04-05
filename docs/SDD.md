# Myelix Community — Software Design Document

## system goal

Myelix Community is the open-source, on-device half of the Myelix system. Its responsibilities are: passive sensor collection, local baseline modeling, ONNX inference, encrypted storage, plugin execution, and privacy-preserving aggregate sync with myelix-core. All biometric data processing occurs on-device. The system must function fully offline. The network boundary is narrow and explicitly controlled.

## architecture overview

```
┌─────────────────────────────────────────────────────────────────┐
│                         Myelix.App                              │
│              (.NET 10 MAUI — Android / iOS / Windows)           │
│                                                                 │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────────┐  │
│  │   UI Layer   │  │  AppShell    │  │   ConsentManager     │  │
│  │  (XAML/MVVM) │  │  Navigation  │  │   (permission gate)  │  │
│  └──────┬───────┘  └──────┬───────┘  └──────────┬───────────┘  │
│         └─────────────────┴──────────────────────┘             │
│                            │                                    │
└────────────────────────────┼────────────────────────────────────┘
                             │  DI / service locator
         ┌───────────────────┼───────────────────────┐
         │                   │                       │
         ▼                   ▼                       ▼
┌─────────────────┐ ┌─────────────────┐ ┌─────────────────────┐
│ Myelix.Sensors  │ │ Myelix.Baseline │ │  Myelix.Inference   │
│                 │ │                 │ │                     │
│ ISensorSource   │ │ BaselineManager │ │  InferenceEngine    │
│ TypingCadence   │ │ FeatureExtractor│ │  OnnxSession        │
│ GaitSensor      │ │ BaselineStore   │ │  ModelRegistry      │
│ TouchDynamics   │ │ DriftCalculator │ │  ScoreComputer      │
└────────┬────────┘ └────────┬────────┘ └──────────┬──────────┘
         │                   │                      │
         └───────────────────┼──────────────────────┘
                             │
                             ▼
                  ┌─────────────────────┐
                  │   Myelix.Storage    │
                  │                     │
                  │  MyelixDbContext    │
                  │  (EF Core + SQLite) │
                  │  EncryptedDatabase  │
                  │  RetentionPolicy    │
                  └──────────┬──────────┘
                             │
              ┌──────────────┼──────────────┐
              │                             │
              ▼                             ▼
   ┌─────────────────────┐     ┌─────────────────────┐
   │  Myelix.PluginSdk   │     │    Myelix.Sync       │
   │                     │     │                     │
   │  IPluginModule      │     │  SyncPipeline       │
   │  IPluginContext     │     │  AggregateBuilder   │
   │  PluginBase         │     │  FlowerClient       │
   │  PluginSandbox      │     │  ConsentFilter      │
   └─────────────────────┘     └─────────────────────┘
                                          │
                                          ▼
                               ┌─────────────────────┐
                               │    myelix-core       │
                               │  (proprietary —      │
                               │   aggregate sync     │
                               │   FL coordination)   │
                               └─────────────────────┘
```

## deployment assumptions

- The app runs on Android 12+, iOS 16+, and Windows 11 with .NET 10 MAUI.
- Background execution is constrained by OS policy on all three platforms. The sensor collection layer is designed to operate within background execution windows without assuming persistent background processes.
- SQLite database files are stored in the app's private data directory, which is inaccessible to other apps without root/jailbreak on Android and iOS.
- ONNX models are bundled with the app at release and updated via the model registry pull mechanism. The app does not require network access to run inference.
- On Windows, background execution is handled via a background task registered with the OS scheduler. On Android, a foreground service is used with a minimal persistent notification as required by Android 12+ policy. On iOS, background processing uses the BackgroundTasks framework with appropriate task identifiers.

## boundary rules

These rules are enforced architecturally. They are not configuration options.

1. Raw sensor samples never leave the device. The sync pipeline operates exclusively on derived aggregate features constructed by `AggregateBuilder`. The pipeline has no reference to raw sample types.
2. Plugins receive only the outputs defined in their plugin contract (a typed `IPluginContext` surface). `PluginSandbox` enforces this by providing a context object that has no path to raw sensor repositories or the storage layer.
3. Any data leaving the device must pass through `ConsentFilter`, which validates that the user has given explicit opt-in consent for aggregate sync and that the payload conforms to the approved aggregate schema.
4. The `ConsentManager` is the single authority on permission state. Sensor sources check `ConsentManager` before beginning a collection cycle. If consent is revoked mid-cycle, the in-progress cycle is discarded.
5. Encryption is always on. There is no plaintext mode. The `EncryptedDatabase` wrapper initializes SQLCipher with a device-bound key before the EF Core context is opened.

## high-level components

### Myelix.App

The MAUI application shell. Hosts the navigation structure, dependency injection container, platform service registrations, and AppShell. Responsible for platform-specific lifecycle handling (foreground/background transitions) and surfacing platform permission dialogs. Contains no business logic — it wires services and navigates.

Key types: `MauiProgram`, `AppShell`, `ConsentManager`.

`ConsentManager` is a cross-cutting service that tracks per-signal consent state, persists consent records to encrypted storage, and exposes a reactive API so that UI and sensor sources can observe consent changes in real time.

### Myelix.Sensors

Passive sensor abstraction layer. Each sensor type implements `ISensorSource`, which exposes a streaming observable of typed sample events. Sensor sources are platform-aware; each has platform-specific implementations behind the abstraction (e.g., Android `InputMethodService` integration for typing cadence vs. iOS `UITextInput` observation).

Active sensors:
- `TypingCadenceSensor`: captures keystroke interval and dwell time distributions without capturing key content. Keystrokes are recorded as timing events only.
- `GaitSensor`: processes accelerometer and gyroscope data during locomotion windows detected by a step-detection filter. Only locomotion-windowed data is retained; ambient accelerometer noise outside locomotion windows is discarded immediately.
- `TouchDynamicsSensor`: captures touch event metadata (contact area, pressure, velocity, gesture duration) without capturing what was touched or any spatial coordinates relative to app content.

All sensors implement a sample rate limiter and a local ring buffer. Samples older than the configured retention window are dropped before storage.

### Myelix.Baseline

The 30-day Ghost Mode baseline manager. Consumes sample streams from `Myelix.Sensors`, extracts feature vectors via `FeatureExtractor`, accumulates them during the baseline window, and manages the `BaselineStore` — the serialized representation of the user's personal norm.

`FeatureExtractor` produces derived statistical features (mean, variance, autocorrelation, spectral features) from raw samples. These features are what get stored and what the TCN model trains on. Raw samples are subject to the retention policy and are deleted after feature extraction.

`DriftCalculator` computes the residual between today's feature vector and the stored baseline. This residual is the primary input to the ONNX inference engine.

`BaselineManager` tracks baseline window progress, enforces the 30-day minimum, and emits a `BaselineEstablished` event when the threshold is met.

### Myelix.Inference

ONNX Runtime wrapper and resilience score computation. The `InferenceEngine` loads an ONNX model session from `ModelRegistry`, feeds the drift vector produced by `DriftCalculator`, and returns a scored output.

`ModelRegistry` manages versioned ONNX model files. On startup it checks whether a newer model version is available from myelix-core (requires network and user opt-in). It maintains a local model cache and falls back to the bundled baseline model if no update is available or the device is offline.

`ScoreComputer` translates raw model output into the 0–100 resilience score, applies per-signal weighting based on which sensors are active, and attaches a confidence indicator reflecting signal completeness.

The TCN (Temporal Convolutional Network) architecture is chosen for its efficiency on sequential behavioral data. It is compact enough to run inference on-device in under 2 seconds on mid-range 2022 hardware.

### Myelix.Storage

Encrypted persistence layer. Built on EF Core 9 with the SQLite provider and SQLCipher for at-rest encryption. The `MyelixDbContext` defines entity models for: raw sample ring buffers (short-lived), extracted feature vectors, baseline snapshots, resilience score history, sync audit log, and consent records.

`RetentionPolicy` runs on a scheduled background task. It enforces configurable retention windows per data class: raw samples are deleted after feature extraction (configurable, default 72 hours); feature vectors are retained for 90 days; score history is retained indefinitely (user-deletable); sync log entries are retained for 30 days.

The encryption key is derived per-platform: on Android from the Android Keystore system using a device-bound AES key; on iOS from Secure Enclave via the Keychain; on Windows from DPAPI with the user account as the protection scope.

### Myelix.PluginSdk

The public plugin interface surface. This is the only project in the solution intended for external consumption. Researchers reference `Myelix.PluginSdk` to build plugins; they do not reference any other project.

Core interfaces:
- `IPluginModule`: entry point for plugin registration and lifecycle (Initialize, Start, Stop, Dispose).
- `IPluginContext`: the typed surface exposed to plugins at runtime. Provides: current resilience score, aggregated feature summaries for whichever signal types the plugin contract specifies, app-level metadata (baseline established flag, active signal list), and a write channel for plugin-authored output values that users have consented to.
- `PluginBase`: abstract base class providing default lifecycle implementations and a structured logger. Recommended for all plugins.

The SDK enforces the access boundary by design: `IPluginContext` has no methods or properties that would expose raw sample data or direct access to the storage layer. The `PluginSandbox` in Myelix.App wraps the real context implementation and injects a constrained proxy to each plugin instance.

Plugin manifests declare: minimum SDK version, required `IPluginContext` access claims, optional sync participation flag, author identity, and study description. Claims are validated at install time.

### Myelix.Plugin.Sample

Reference plugin implementation. Demonstrates: plugin registration, context access, aggregated feature consumption, and output writing. Intended as the canonical starting point for plugin developers.

### Myelix.Sync

Aggregate sync pipeline. Responsible for constructing policy-approved aggregate payloads from local feature data and submitting them to myelix-core via the Flower federated learning protocol.

`AggregateBuilder` transforms feature vectors into the anonymized aggregate format approved by the sync policy. It has no access to raw sample repositories — only to the feature vector tables in `Myelix.Storage`.

`FlowerClient` implements the Flower gRPC client protocol for federated learning rounds: pull model update from server, perform local fine-tuning, submit weight gradients. It operates only when the device is on wifi (configurable), the user has opted in to federated participation, and a valid baseline exists.

`ConsentFilter` is the final gate before any payload leaves the device. It validates consent state from `ConsentManager`, validates payload schema against the approved aggregate schema, and rejects any payload that fails either check. This is a hard rejection — there is no fallback path that bypasses it.

## data flow summary

```
Sensor hardware
     │
     ▼
ISensorSource (per-platform implementation)
     │ typed sample events
     ▼
FeatureExtractor (Myelix.Baseline)
     │ feature vectors
     ▼
BaselineStore (Myelix.Storage — encrypted SQLite)
     │
     ├──► DriftCalculator → InferenceEngine → ScoreComputer → score history
     │
     └──► AggregateBuilder → ConsentFilter → FlowerClient → myelix-core
                                                (only anonymized weight gradients)
```

Raw samples never reach `AggregateBuilder`. Identifiable data never reaches `FlowerClient`. The split between raw sample retention and feature retention is enforced by the storage schema — the sync pipeline references only feature-level tables.

## dependency injection and composition

All inter-project dependencies are wired through the .NET Generic Host DI container, configured in `MauiProgram`. Projects depend on abstractions (interfaces in `Myelix.PluginSdk` and internal interface contracts), not on concrete implementations. This allows the test suite to substitute in-memory or mock implementations for all storage, sensor, and inference dependencies.

Platform-specific service registrations (e.g., the Android vs. iOS vs. Windows implementation of `ISensorSource`) are resolved via `#if ANDROID / IOS / WINDOWS` conditional compilation in the `Myelix.App` composition root.

## testing strategy

- Unit tests cover: `FeatureExtractor`, `DriftCalculator`, `ScoreComputer`, `AggregateBuilder`, `ConsentFilter`, `RetentionPolicy`. All use in-memory implementations of storage interfaces.
- Integration tests cover: full collection-to-score pipeline with a synthetic sensor source and an in-memory SQLite database.
- Platform tests (GitHub Actions matrix): build and basic smoke tests on Android emulator, iOS simulator, and Windows runner.
- Plugin SDK contract tests: a test suite in `Myelix.Plugin.Sample.Tests` validates that the sample plugin cannot access raw data through the injected `IPluginContext`.

## ci pipeline

GitHub Actions workflows:
- `build.yml`: restore, build, and unit test on every pull request. Runs on ubuntu-latest for the .NET projects; Android and Windows build steps run on their respective hosted runners.
- `release.yml`: triggered on version tags. Builds release artifacts for Android (AAB), iOS (IPA), and Windows (MSIX). Signs artifacts using secrets stored in the repository's encrypted secret store.
