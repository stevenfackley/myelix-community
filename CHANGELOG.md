# Changelog

All notable changes to myelix-community will be documented here.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

## [Unreleased]

### Added
- Repository restructured: code under `src/`, tests under `tests/`
- Full documentation suite: PRD, PDD, SDD, marketing, business plan, go-to-market, brand brief, pricing, risk memo

---

## [1.0.0] — 2026-04-04

### Added
- Initial repository scaffold
- `Myelix.App` — .NET 10 MAUI app shell (Android, iOS, Windows)
- `Myelix.Baseline` — 30-day Ghost Mode baseline manager
- `Myelix.Sensors` — passive sensor abstraction (typing cadence, gait, touch)
- `Myelix.Storage` — SQLite + EF Core encrypted-at-rest storage
- `Myelix.Inference` — ONNX Runtime inference with TCN model support
- `Myelix.PluginSdk` — extensible plugin interface for researchers
- `Myelix.Plugin.Sample` — reference plugin implementation
- `Myelix.Sync` — aggregate sync pipeline (privacy-validated, no raw biometrics)
- `Myelix.Community.Tests` — xUnit test suite
- GitHub Actions CI pipeline (build + test)
- Federated learning and plugin development documentation
