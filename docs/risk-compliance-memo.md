# Myelix Community — Risk and Compliance Memo

## purpose

This memo identifies the principal legal, regulatory, privacy, and operational risks facing the Myelix Community product, states the intended posture on each, and specifies the design and operational constraints that implement that posture. It is intended for the founding team, legal counsel, and any compliance review process. It is not a legal opinion.

## intended operating posture

Myelix Community occupies a specific and intentional regulatory position: it is a personal wellness monitoring tool, not a medical device. It collects behavioral signals, builds a personal baseline, and surfaces a relative pattern indicator. It does not diagnose, treat, or predict any medical condition. It does not provide clinical recommendations. This posture is not a liability hedge — it is an accurate description of what the product does. The design, the copy, and the operational practices are all required to maintain this boundary consistently.

The privacy architecture is the primary risk mitigation mechanism. By ensuring that raw biometric data never leaves the device, Myelix eliminates the largest class of privacy liability (data breach, unauthorized access, regulatory action for unlawful data transfer) before any policy or legal protection is needed.

## product boundary

These are the hard limits of what the product claims to do and what it is permitted to say. All product copy, documentation, and marketing materials must be reviewed against this boundary.

**the product does:**
- Passively collect behavioral interaction signals from the device.
- Build a personal behavioral baseline on-device using a local ML model.
- Surface a resilience score that reflects the degree to which current patterns match the personal baseline.
- Store all data locally in encrypted form.
- Submit only anonymized aggregate weight gradients for federated model improvement, with explicit opt-in.

**the product does not:**
- Diagnose any medical condition, including any neurodegenerative, psychiatric, or cognitive disorder.
- Predict future cognitive health status.
- Provide clinical recommendations of any kind.
- Claim accuracy for any medical purpose.
- Transmit raw biometric data.
- Store or process any health data in a form that would trigger HIPAA covered entity obligations (the product is not a HIPAA covered entity and is designed to remain outside that scope).

## privacy risks

**risk: GDPR / UK GDPR compliance for EU and UK users**

Behavioral biometric data — typing cadence, gait patterns, touch dynamics — may constitute biometric data under GDPR Article 9, which is a special category requiring explicit consent and a higher standard of protection. Even data that does not qualify as biometric data under the strict GDPR definition may qualify as personal data subject to standard GDPR requirements.

Posture: the on-device architecture is the primary mitigation. If raw biometric data never leaves the device, cross-border data transfer rules (Chapter V GDPR) do not apply to that data. The sync layer transmits only anonymized aggregates; anonymization must meet the GDPR Article 4(1) standard (the data cannot be re-identified without disproportionate effort). Legal basis for any sync is explicit consent (Article 6(1)(a) and, if biometric classification applies, Article 9(2)(a)).

Required actions: privacy notice must comply with GDPR Articles 13/14 transparency requirements. Data subject rights (access, erasure, portability, restriction) must be exercisable from within the app for all locally stored data. The app must support erasure without requiring a support ticket. For EU/UK users who opt in to sync, a data processing record must be maintained.

**risk: CCPA / CPRA compliance for California users**

California treats precise geolocation and sensitive personal information categories, which may overlap with behavioral biometric data, as requiring opt-out rights and disclosure. The CPRA added sensitive personal information with specific use limitation rights.

Posture: on-device-only processing of raw data eliminates most CCPA "sale" or "sharing" exposure. The privacy notice must disclose what categories of information are collected, the purpose, and any third-party sharing (limited to the anonymized aggregate sync with myelix-core, which is disclosed explicitly). California users must have a clear "do not sell or share my personal information" mechanism — in Myelix's case this is the sync opt-out, which must be accessible in one tap from Privacy Vault.

**risk: HIPAA-adjacent exposure for clinical use drift**

The product is explicitly not a HIPAA covered entity or business associate. However, if healthcare providers recommend the app to patients, or if the Clinical tier (Phase 3) is introduced without adequate structural separation, HIPAA obligations could be triggered for the business associate relationship with a covered entity.

Posture: the community edition terms of service must explicitly disclaim clinical use. The product must not be marketed to healthcare providers as a clinical tool until the Clinical tier's regulatory and compliance structure is established. Phase 3 requires a separate compliance analysis before launch.

**risk: biometric privacy laws (Illinois BIPA, Texas, Washington, and others)**

Several US states have biometric privacy laws, the most stringent being Illinois BIPA, which requires written consent before collection of biometric identifiers and imposes a private right of action with statutory damages.

Posture: the consent-first onboarding flow, which requires explicit per-signal permission before any collection begins, is designed to satisfy BIPA-style consent requirements. The privacy notice must identify the specific signals collected, describe how they are used, and state the retention and deletion policy. Given the on-device-only model, the "possession" of biometric identifiers is entirely on-device — which does not eliminate BIPA obligations but substantially reduces the breach risk that motivates BIPA's statutory damages.

Required actions: legal review of the consent flow against BIPA requirements before any US public launch. Consider Illinois-specific consent language in the onboarding flow if the BIPA analysis recommends it.

## plugin ecosystem risks

**risk: a plugin author misrepresents data access or study purpose**

A malicious or negligent plugin author publishes a plugin that misrepresents what it collects, how data is used, or the nature of the study. Users install it based on the listing description.

Posture: all plugins submitted for listing in the community directory must pass a Myelix review process before publication. The review checks: the plugin manifest's claimed access scope against the actual `IPluginContext` surface the plugin uses; the study description for accuracy and completeness; and the plugin's sync flag (whether it participates in any external data transmission). Plugins that pass review are listed with their access manifest displayed to users before install. The SDK enforces that plugins cannot exceed their declared access claims at runtime.

The architectural sandbox is the hard limit: plugins cannot access raw sensor data regardless of what their manifest claims or what the review process misses. The SDK boundary is the last line of defense, and it does not require correct plugin author behavior to function.

**risk: plugin author is a bad actor with a legitimate-seeming study**

A plugin is approved because it looks like a legitimate research study but is designed to exfiltrate derived feature data to an adversarial server.

Posture: the plugin sandbox limits exposure to derived feature summaries, not raw data. However, derived features could still be sensitive. The plugin review process must include: verification of researcher identity and institutional affiliation; review of the plugin's network access (plugins should not make outbound network calls — sync must go through the Myelix sync pipeline, not the plugin); and a terms of service for plugin authors that specifies permitted data use and imposes liability for violations. A mechanism for users to report suspicious plugins and for Myelix to remotely disable a listed plugin must exist at launch.

## model accuracy and claims risks

**risk: users or third parties rely on the resilience score for medical decisions**

A user interprets a low resilience score as evidence of a medical condition and seeks (or avoids) medical care based on it. Or a plaintiff argues that Myelix's score constituted medical advice in a legal dispute.

Posture: the product boundary (no medical claims) must be maintained with absolute consistency across all surfaces — in-app copy, marketing, documentation, and support responses. The score presentation must always include a contextual disclaimer that the score is a relative behavioral pattern indicator, not a medical finding. The terms of service must explicitly disclaim medical advice and require users to acknowledge this.

The in-app score presentation must not use language or visual design that evokes clinical output (no red/amber/green health status indicators, no language like "your cognitive health today").

**risk: model bias and differential accuracy**

The TCN model may perform with different accuracy across demographic groups, device types, or usage patterns. A model that is more accurate for one population and less accurate for another creates fairness and potential discrimination risks, particularly in a health context.

Posture: the federated learning approach, where models improve on distributed device populations, creates some organic diversity in training data. However, the initial model is trained on a limited development population. Required actions: model validation must include analysis of accuracy variance across device type, age group (to the extent that can be inferred from behavioral patterns), and usage intensity. Known accuracy limitations must be disclosed in the app's methodology documentation.

## consent management risks

**risk: consent records are lost or are not legally sufficient**

A user claims they never consented to a signal type being collected. The consent record is unavailable, corrupt, or insufficiently detailed to establish what was agreed to.

Posture: consent events are written to the encrypted local database as immutable records: timestamp, signal type, consent state, version of the consent prompt shown. If the user requests data deletion, consent records are retained for a legally appropriate period (minimum 3 years) after deletion of the underlying data, as evidence that consent was properly obtained and later revoked. The consent prompt text is versioned; the version shown at each consent event is recorded.

**risk: consent for minors**

A user under 18 installs the app and uses it. The GDPR and COPPA impose heightened requirements for data processing involving minors.

Posture: the app's terms of service must specify a minimum age of 18 (or 16 in jurisdictions where GDPR Article 8 applies). The onboarding flow must include an age acknowledgment. App store listings must specify the minimum age. This is a policy control, not a technical one — it does not fully prevent minor users but establishes the required legal posture.

## accessibility and health app requirements

**risk: accessibility failures create legal exposure and exclude users with the greatest need**

Cognitive health monitoring is disproportionately relevant to populations that may also have motor, visual, or cognitive accessibility needs. Failure to meet accessibility standards creates legal exposure under the ADA (US), the Equality Act (UK), and analogous laws.

Posture: WCAG 2.1 AA compliance is a hard requirement for all primary navigation flows. VoiceOver (iOS) and TalkBack (Android) support is required for onboarding, score display, history, and privacy vault. The score and history displays must have accessible text alternatives. Platform accessibility audits must be part of the release checklist before any public launch.

**risk: the app itself causes harm through anxiety or behavioral change**

A user becomes anxious about their resilience score and changes behavior in ways that are harmful — either over-monitoring, over-interpreting, or acting on score changes without clinical guidance.

Posture: the product design must not create anxiety loops. No push notifications, no alarming visual design, no urgency language around score changes. The onboarding must set the expectation that the score is a relative indicator, not a health alarm. The app must provide a clear pathway for users who are concerned about their cognitive health to seek professional guidance — a non-alarming, matter-of-fact statement in the app that the score is not a substitute for medical evaluation, with a link to appropriate resources.

## marketing guardrails

These restrictions apply to all marketing copy, press materials, social media, and partner communications.

- Do not use the words "diagnose," "detect [condition name]," "predict," or "clinical" in any consumer-facing context unless the Clinical tier is launched and the specific regulatory track for that tier is complete.
- Do not compare the resilience score to medical benchmarks or population health statistics.
- Do not name clinical conditions (Alzheimer's, dementia, ADHD, depression, etc.) in marketing copy. The product does not diagnose or monitor specific conditions.
- Do not imply that using Myelix reduces the risk of cognitive decline or improves cognitive health. The product monitors patterns. It does not intervene.
- Do not claim "100% private" or "completely private." The accurate claim is: raw biometric data never leaves the device; only anonymized aggregates may sync with explicit opt-in.
- Do not use testimonials that include medical claims or imply clinical benefit.

Any proposed marketing copy that is ambiguous on these guardrails must be reviewed before publication.
