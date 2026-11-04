# Federated Learning on Myelix

## Decentralized Intelligence
Myelix uses a privacy-preserving federated learning approach to improve resilience models without collecting raw biometric data.

## Process
1. Server broadcasts model update round.
2. Device pulls versioned ONNX model via `ModelRegistry`.
3. Device performs local fine-tuning on derived features.
4. Device submits anonymized weight updates via `FlowerClient`.

## Privacy Guarantees
- Raw biometric data is *never* transmitted.
- Only policy-approved aggregates leave the device.
