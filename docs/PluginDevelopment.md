# Myelix Plugin Development Guide (v1.3.0)

## Overview
Myelix plugins allow researchers to extend the resilience scoring pipeline without compromising user privacy.

## SDK Interfaces
- `IPluginModule`: Core registration and lifecycle.
- `IPluginContext`: Access to app-level metadata and scores.
- `PluginBase`: Recommended base class for all plugins.

## Restrictions
- Plugins cannot access raw sensor data.
- Plugins are sandboxed to local-only execution in the initial release.

## Sample Plugin
See `Myelix.Plugin.Sample` for a reference implementation.
