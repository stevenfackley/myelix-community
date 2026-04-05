# Myelix Community — Product Requirements Document

## product goal

Myelix Community is a passive cognitive health monitoring application for Android, iOS, and Windows. It detects subtle changes in a user's cognitive and behavioral patterns by continuously observing device interaction signals — typing cadence, gait, touch dynamics — and modeling those signals locally into a personalized resilience baseline. When patterns drift from that baseline, the app surfaces a resilience score that reflects the change without diagnosing, alarming, or transmitting raw data.

The product goal is to give individuals a private, longitudinal window into their own cognitive health that requires no active effort, no clinical visit, and no data surrender. The app is invisible when collecting and clear when reporting.

## users

**primary: health-aware individuals**
Adults who want to monitor their own cognitive health as part of a broader self-care or preventive health practice. They are not necessarily experiencing symptoms; they want early signal. They are comfortable with mobile apps, value privacy, and are skeptical of data-harvesting health products.

**secondary: researchers and plugin developers**
Academic and independent researchers building behavioral study plugins. They need SDK access, study management tooling, and participant consent workflows. They do not need access to raw sensor streams — they work with derived features and plugin output.

**tertiary: healthcare providers (Phase 3, out of scope v1)**
Clinicians who want longitudinal behavioral data exported in a clinically useful format. This audience is acknowledged but explicitly deferred.

## core jobs to be done

- Passively monitor behavioral signals with zero daily effort from the user.
- Build a personalized baseline over a 30-day Ghost Mode period before surfacing any scores.
- Surface a daily resilience score that reflects drift from the user's own personal norm — not a population average.
- Let the user understand what is being collected and revoke consent for any signal at any time.
- Enable researchers to build and distribute study plugins without accessing raw biometric data.

## product scope

**in scope for v1**

- Ghost Mode: 30-day passive collection period that builds the local baseline before the app surfaces any scores. No scores are shown, no nudges are sent, no comparisons are made during this window.
- Passive sensor collection: typing cadence (keystroke intervals, dwell times), gait (accelerometer/gyroscope patterns during locomotion), touch dynamics (swipe pressure, contact area, velocity). All collection is background and permission-gated.
- Local baseline modeling: on-device TCN (Temporal Convolutional Network) model trained on the user's own 30-day signal history. The model runs entirely on-device via ONNX Runtime. No data is required to leave the device to produce a score.
- Resilience score: a 0–100 daily score expressing the degree to which today's behavioral signals match the user's personal baseline. Lower scores indicate greater drift. The score is not a diagnosis. It is not compared to any population benchmark.
- Privacy vault: a user-facing screen that shows exactly what signals are being collected, what is stored locally, what (if anything) is synced in aggregate form, and granular consent toggles per signal type. Consent is revocable at any time.
- Plugin marketplace (read): users can view available research plugins and install them. Plugin execution is sandboxed. Plugins cannot access raw sensor data.
- Local encrypted storage: all sensor data and baseline models are stored in an encrypted SQLite database on-device. The encryption key is derived from device credentials and never transmitted.

**out of scope for v1**

- Clinical integration: EHR export, HL7 FHIR, provider-facing dashboards.
- Cloud sync of raw or granular biometric data: only anonymized aggregates may sync, and only with explicit opt-in.
- Real-time alerts or notifications based on score drift: v1 surfaces the score on demand; push alerts are a v2 consideration.
- Social or comparative features: no benchmarking against other users, no sharing.
- Plugin authoring tools within the app: researchers use the SDK and CLI tooling outside the app.
- Wearable integrations: v1 is phone-sensor only.

## execution modes

**ghost mode**
The default state for all new users. Collection is active. No scores are shown. No inferences are presented. The user knows collection is happening (onboarding makes this explicit) but the app asks for nothing and reports nothing for 30 days. After 30 days the baseline is considered established and the app transitions to active mode.

**active mode**
The state after a valid baseline exists. The resilience score is surfaced daily. Users can drill into signal breakdowns, view historical trends, and manage plugins.

**paused mode**
The user has explicitly suspended collection. No data is gathered. The baseline is preserved. Resuming collection restarts the drift detection window but does not invalidate the existing baseline.

## feature details

**ghost mode onboarding**
The onboarding flow explains what Ghost Mode is, what signals will be collected, and why 30 days of baseline is necessary before scores can be meaningful. Users grant permissions per signal type. The app requests only the permissions needed for the signals the user has consented to. Progress toward the 30-day baseline is shown as a simple fill indicator — no partial scores, no interim readings.

**resilience score**
Computed locally by the ONNX inference engine against the TCN model fine-tuned on the user's own 30-day baseline. The score reflects multi-signal drift across whichever sensor types the user has enabled. It is presented as a single number with a directional indicator (improving, stable, drifting). The breakdown view shows per-signal contribution. Scores are stored locally in encrypted SQLite and are never transmitted in identifiable form.

**privacy vault**
A dedicated screen accessible from the main navigation. It shows: the list of active sensors and their collection status; a data retention timeline (how long raw samples are stored locally before aggregation and deletion); the aggregated sync log (what was sent, when, in what anonymized form); and per-signal consent toggles. Toggling a sensor off immediately suspends collection for that signal and marks any cached data for deletion on the next retention sweep.

**plugin system**
Users can browse available research plugins. Each plugin shows its author, study description, what derived features it requires access to, and whether it participates in any sync protocol. Users install, enable, or disable plugins individually. Plugins run in a sandboxed execution context; they receive only the derived feature outputs the plugin contract specifies, not raw sensor streams. The app enforces this at the SDK boundary.

**federated learning client**
After the baseline is established, and with explicit opt-in, the app participates in federated model improvement rounds coordinated by myelix-core. The device pulls a versioned ONNX model update, performs local fine-tuning on its own derived features, and submits only anonymized weight gradients. Raw data and identifiable signals are never included in sync payloads.

## non-functional requirements

- All sensor collection must be background-capable on all three target platforms within OS-permitted background execution windows.
- The ONNX inference engine must produce a resilience score in under 2 seconds on mid-range 2022 hardware.
- Encrypted SQLite storage must survive app reinstallation on Android (using device-bound key) and iOS (using Secure Enclave-backed key).
- The app must function fully offline. Network connectivity is optional and only affects federated sync and plugin updates.
- The app must meet WCAG 2.1 AA accessibility requirements on all target platforms.
- On first launch, the app must not request any permissions before the user has completed the consent and onboarding screens.
