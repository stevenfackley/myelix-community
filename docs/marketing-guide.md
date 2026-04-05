# Myelix Community — Marketing Guide

## positioning statement

Myelix is the only cognitive health monitor that runs entirely on your device. It watches how you interact with your phone — typing, walking, touching — and builds a model of your normal. When that normal shifts, Myelix notices. Nothing leaves your phone. No account required. No cloud required. Just a quiet, accurate picture of how your cognitive patterns are holding up.

## messaging pillars

**privacy is the product, not a feature**
Most health apps say they care about privacy. Myelix is architecturally incapable of leaking raw biometric data — it never transmits it in the first place. The baseline model lives on your device. Inference runs on your device. If you delete the app, all data is gone. Privacy is not a policy Myelix might change. It is the design.

**passive collection, active insight**
You do not have to do anything for Myelix to work. No daily check-ins, no questionnaires, no wearables. It observes ordinary phone use. After 30 days of building your personal baseline, it tells you how your cognitive patterns compare to your own norm — not a population average, not a benchmark, just you relative to yourself.

**built for the long run**
Cognitive drift is gradual. A single test or a single day of data tells you almost nothing. Myelix works on a 30-day baseline and tracks changes over months. The signal it surfaces is longitudinal by design. It is not an alerting system. It is a monitoring system.

**open, auditable, extensible**
Myelix Community is open source. The sensor collection logic, the inference pipeline, the plugin SDK — all of it is publicly auditable on GitHub. Researchers can build study plugins that extend the scoring pipeline without ever seeing raw data. The code is the proof of the privacy claims.

## audience segments and relevant messages

**health-aware individuals**
These are adults who think proactively about their health, are skeptical of data-harvesting apps, and want signal — not gamification. They do not have a diagnosis. They want early warning. They have probably looked at other cognitive health apps and been put off by account requirements, data sharing, or clinical framing.

Relevant messages: privacy architecture, passive collection, no account required, long-run monitoring. Avoid: any clinical framing, score anxiety, comparison to other users.

**researchers and plugin developers**
Academic and independent researchers building behavioral study tools. They care about the SDK quality, the data access model, the study consent workflow, and institutional credibility. They want to know that participant privacy is handled correctly so they do not have to solve it themselves.

Relevant messages: open source SDK, sandboxed plugin architecture, participant consent built in, raw data never accessible to plugins, institutional support pathway. Avoid: consumer-facing wellness language.

**open source contributors**
Developers interested in privacy-preserving health tech, MAUI development, ONNX inference, or federated learning. They want to understand the architecture, find good first issues, and know the project is actively maintained.

Relevant messages: clean architecture, open governance, good documentation, active community. Avoid: product marketing language entirely in technical community spaces.

## tone guidelines

**calm and factual.** Myelix does not raise the alarm. It does not tell you that you might be cognitively declining. It reports patterns. Copy should match this register: declarative, precise, unhurried.

**specific over vague.** "Myelix uses a Temporal Convolutional Network trained on your 30-day behavioral baseline" is better than "advanced AI monitors your cognitive health." Specificity builds trust. Vagueness erodes it.

**confident without overclaiming.** The product does what it says it does. Copy should state that clearly and stop. No superlatives, no "revolutionary," no comparisons to competitors.

**respectful of user intelligence.** The target user has already thought about privacy. They have already dismissed simpler options. Write to someone who is capable of understanding how the product works and will appreciate being told.

**not alarmist.** Health monitoring can easily tip into anxiety-inducing territory. Every piece of copy should ask: does this make the reader worried about their health in a way that is not supported by what the product actually does? If yes, rewrite it.

## recommended language

| instead of | use |
|---|---|
| detects cognitive decline | monitors changes in your cognitive patterns |
| AI-powered brain health | on-device behavioral pattern modeling |
| real-time alerts | daily resilience score |
| your brain health score | your resilience score |
| clinical-grade monitoring | long-term behavioral baseline |
| tracks your mental health | monitors cognitive and behavioral patterns |
| we take your privacy seriously | raw biometric data never leaves your device |
| advanced machine learning | ONNX inference running locally on your phone |

## claims to avoid

**medical claims.** Myelix is not a medical device. It does not diagnose, treat, or predict any medical condition. Any copy that implies otherwise creates regulatory risk and erodes user trust when the product cannot deliver clinical validation. Do not say: "detects early signs of Alzheimer's," "predicts cognitive decline," "clinically validated," "medically accurate."

**absolute privacy claims.** Do not say "completely private" or "100% private." The correct framing is architecturally specific: raw biometric data never leaves the device; only anonymized aggregates may sync with explicit opt-in. Overstating privacy creates legal exposure and trust problems when technical nuances are scrutinized.

**comparison to competitors by name.** Do not name competitors in marketing copy.

**efficacy claims without qualification.** Do not say "Myelix will detect cognitive drift." The correct framing is that it monitors patterns and surfaces changes — not that it will definitively identify any specific condition or event.

**urgency and fear.** Do not use copy that creates anxiety about cognitive health to drive installs. "What if your brain is changing and you don't know?" is not appropriate for this product.

## acquisition language by channel

**app store listing**
Lead with the privacy architecture. The user browsing app stores for a cognitive health app has seen a lot of products that want their data. The headline differentiator is that Myelix does not. Second: passive collection — no effort required. Third: the 30-day baseline approach as evidence of seriousness.

**github readme / developer community**
Lead with the architecture: .NET 10 MAUI, ONNX Runtime, Flower federated learning, open source plugin SDK. Technical specificity is the credibility signal here. Privacy architecture is a secondary message — still important, but framed in terms of design rather than marketing.

**research community outreach**
Lead with the plugin SDK and consent workflow. Researchers want to know: can I build on this without solving participant privacy from scratch? The answer should be yes, and the explanation should be architectural and specific about what the SDK exposes and what it blocks.

**word of mouth and community**
Encourage users to share the app on the basis of the privacy story — not the health story. "An app that builds a cognitive baseline entirely on your phone, no account, no cloud" is a shareable claim. Asking people to share their health scores is inappropriate.
