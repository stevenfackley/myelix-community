# myelix-community

[![CI](https://github.com/stevenfackley/myelix-community/actions/workflows/main.yml/badge.svg)](https://github.com/stevenfackley/myelix-community/actions/workflows/main.yml)

**Privacy-first resilience monitoring — open client, sensor pipeline, and Plugin SDK.**

myelix-community is the public half of the Myelix system. It contains the .NET MAUI mobile app, passive sensor collection layer, local baseline engine, ONNX inference runtime, and the Plugin SDK for researchers. Raw biometric data never leaves the device.

---

## Architecture

| Component | Technology |
|---|---|
| **Mobile App** | .NET 10 MAUI (Android, iOS, Windows) |
| **Sensor Layer** | Myelix.Sensors (typing cadence, gait, touch) |
| **Baseline Engine** | Myelix.Baseline (30-day Ghost Mode) |
| **Local Storage** | SQLite + EF Core (encrypted at rest) |
| **Inference** | ONNX Runtime + TCN model |
| **Plugin SDK** | Myelix.PluginSdk (extensible researcher interface) |
| **Sync** | Myelix.Sync (anonymized aggregates only) |
| **Contracts** | Myelix.Core.Contracts (versioned NuGet from myelix-core) |
| **CI/CD** | GitHub Actions |

---

## Quick Start

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Android/iOS SDK for mobile targets (optional for library-only development)

### Build & Test

```bash
git clone https://github.com/stevenfackley/myelix-community.git
cd myelix-community
dotnet restore src/Myelix.Community.slnx
dotnet build src/Myelix.Community.slnx
dotnet test src/Myelix.Community.slnx
```

---

## Project Structure

```
myelix-community/
├── src/
│   ├── Myelix.App/              # .NET 10 MAUI app (Android, iOS, Windows)
│   ├── Myelix.Baseline/         # Ghost Mode baseline manager
│   ├── Myelix.Sensors/          # Passive sensor abstraction layer
│   ├── Myelix.Storage/          # SQLite + EF Core encrypted storage
│   ├── Myelix.Inference/        # ONNX Runtime inference engine
│   ├── Myelix.PluginSdk/        # Public plugin interfaces
│   ├── Myelix.Plugin.Sample/    # Reference plugin implementation
│   ├── Myelix.Sync/             # Aggregate sync pipeline
│   └── Myelix.Community.slnx   # Solution file
├── tests/
│   └── Myelix.Community.Tests/  # xUnit unit tests
├── docs/
│   ├── PRD.md                   # Product Requirements Document
│   ├── PDD.md                   # Product Design Document
│   ├── SDD.md                   # Software Design Document
│   ├── marketing-guide.md
│   ├── business-plan.md
│   ├── go-to-market-launch-plan.md
│   ├── brand-business-brief.md
│   ├── pricing-packaging-brief.md
│   ├── risk-compliance-memo.md
│   ├── FederatedLearning.md     # FL client integration guide
│   └── PluginDevelopment.md     # Plugin SDK authoring guide
├── .github/workflows/main.yml
├── CHANGELOG.md
├── CONTRIBUTING.md
├── LICENSE
└── SECURITY.md
```

---

## Privacy Model

Myelix is built on a strict on-device privacy boundary:

| Data Type | Location | Leaves Device? |
|---|---|---|
| Raw keystroke timings | Device only | Never |
| Raw gait samples | Device only | Never |
| Raw touch coordinates | Device only | Never |
| Anonymized resilience aggregates | Synced | Yes (with consent) |
| Resilience score | Device + optional sync | Opt-in |

The `Myelix.Sync` pipeline enforces this policy in code. Automated CI validation checks that no raw biometric types appear in sync payloads.

---

## Plugin SDK

Researchers and developers can extend Myelix by implementing `IPlugin` from `Myelix.PluginSdk`. Plugins run in an isolated context and cannot access raw sensor streams.

See [docs/PluginDevelopment.md](./docs/PluginDevelopment.md) for the full authoring guide.

---

## Tracks

| Track | Status |
|---|---|
| Track 1: Bootstrap (v1.0.0) | Complete |
| Track 2: Federated Learning & Plugins | In Progress |
| Track 3: Research API & Analytics | Planned |

---

## Documentation

- [Product Requirements Document](./docs/PRD.md)
- [Product Design Document](./docs/PDD.md)
- [Software Design Document](./docs/SDD.md)
- [Federated Learning Guide](./docs/FederatedLearning.md)
- [Plugin Development Guide](./docs/PluginDevelopment.md)

---

## License

Source-available — forks permitted for personal and non-commercial use. Commercial use requires written permission. See [LICENSE](./LICENSE) for full terms.
