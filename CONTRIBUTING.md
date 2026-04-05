# Contributing to myelix-community

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- An editor with C# support (Visual Studio, Rider, or VS Code with C# Dev Kit)
- Android/iOS SDK for mobile targets (optional — library projects build without them)

### Local Setup

```bash
git clone https://github.com/stevenfackley/myelix-community.git
cd myelix-community
dotnet restore src/Myelix.Community.slnx
dotnet build src/Myelix.Community.slnx
```

### Running Tests

```bash
dotnet test src/Myelix.Community.slnx
```

## Project Structure

```
src/
  Myelix.App/              # .NET 10 MAUI app (Android, iOS, Windows)
  Myelix.Baseline/         # Ghost Mode baseline manager
  Myelix.Sensors/          # Passive sensor abstraction layer
  Myelix.Storage/          # SQLite + EF Core encrypted storage
  Myelix.Inference/        # ONNX Runtime inference engine
  Myelix.PluginSdk/        # Public plugin interfaces
  Myelix.Plugin.Sample/    # Reference plugin implementation
  Myelix.Sync/             # Aggregate sync pipeline
tests/
  Myelix.Community.Tests/  # xUnit unit tests
docs/
  FederatedLearning.md     # FL client integration guide
  PluginDevelopment.md     # Plugin SDK development guide
```

## Branch & PR Workflow

- Branch from `main` using `feat/`, `fix/`, or `chore/` prefixes
- Keep PRs focused on a single concern
- All CI checks must pass before merging

## Code Style

- Standard C# conventions: PascalCase types/methods, camelCase locals
- Async methods must be suffixed with `Async`
- Do not commit secrets or build artifacts

## Privacy Boundary

This is the most important rule in this repo:

- Raw biometric data (keystroke timings, gait samples, touch coordinates) **must never leave the device**
- `Myelix.Sync` may only transmit policy-approved, anonymized aggregates
- Any PR that touches sensor collection, storage, or sync is subject to privacy review
- Plugins loaded via `Myelix.PluginSdk` are sandboxed and must not access raw sensor streams directly

## Plugin Development

See [docs/PluginDevelopment.md](./docs/PluginDevelopment.md) for the full plugin authoring guide.

## Reporting Issues

Open an issue describing the problem, steps to reproduce, and expected vs. actual behaviour. For security issues, see [SECURITY.md](./SECURITY.md).
