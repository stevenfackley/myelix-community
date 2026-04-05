# Security Policy

## Reporting a Vulnerability

Do not open a public issue for security vulnerabilities.

Instead, please report them by emailing the maintainer directly. Include:

- A description of the vulnerability and its potential impact
- Steps to reproduce
- Any suggested mitigations

You can expect an acknowledgement within 48 hours and a resolution timeline within 14 days depending on severity.

## Scope

| Area | In Scope |
|---|---|
| Local data storage encryption | Yes |
| Plugin sandbox isolation | Yes |
| Raw biometric data leakage to network | Yes |
| Consent bypass or manipulation | Yes |
| Dependency vulnerabilities | Yes |
| Sensor data exfiltration | Yes |

## Privacy Boundary

The Myelix privacy contract is a security boundary. Any mechanism — including a plugin, sync service, or SDK consumer — that causes raw biometric data to leave the device is treated as a critical vulnerability.

## Supported Versions

Only the latest commit on `main` is actively maintained.
