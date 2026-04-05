# Myelix — Pricing and Packaging Brief

## packaging strategy

Myelix uses a four-tier packaging model: Free, Pro, Research, and Clinical. The Free tier is the open-source community edition — a complete, functional product, not a demo. It is the foundation for everything else and must be genuinely useful on its own. Premium tiers add depth, not access to core functionality that should be free.

The packaging strategy is built around a principle: the privacy architecture is never paywalled. Every user on every tier gets full on-device processing, encrypted local storage, and the guarantee that raw biometric data never leaves the device. Privacy is not a premium feature. It is the product.

Tiers are differentiated on: analytics depth, data portability, research tooling, and institutional integration — not on core collection, baseline modeling, or resilience scoring.

## free tier

**name:** Myelix Community

**price:** free, no account required

**what it includes:**

- Full passive sensor collection (typing cadence, gait, touch dynamics), permission-gated.
- 30-day Ghost Mode baseline period with on-device TCN model training via ONNX Runtime.
- Daily resilience score with a 7-day sparkline.
- Per-signal consent controls and privacy vault.
- Plugin installation and execution (sandboxed, community-published plugins).
- Opt-in participation in federated learning model improvement rounds.
- Encrypted local SQLite storage with default retention policy.
- Full offline functionality.

**what it does not include:**

- Historical trend analysis beyond 7 days of score history displayed in-app.
- Data export (PDF, CSV, or any machine-readable format).
- Advanced per-signal breakdown analytics.
- Plugin authoring tools.
- Priority plugin review.

**rationale:** The free tier must be complete enough that a health-aware individual can get genuine long-run value from it indefinitely without upgrading. The constraint on history depth is a mild and honest one — 7 days of score context is useful for day-to-day awareness; upgrading to Pro unlocks the full longitudinal value. The constraint on export is appropriate because there is no clinical use case for export at the free tier.

## pro tier

**name:** Myelix Pro

**price:** $9.99/month or $79/year

**what it includes, in addition to Free:**

- Full 90-day score history in-app, with a detailed timeline chart.
- Per-signal contribution breakdown for any historical date — understand which signals drove that day's score.
- Trend analysis: week-over-week and month-over-month pattern summaries.
- Score export to PDF (personal health summary format) and CSV (raw score history with metadata).
- Configurable data retention (extend raw sample retention window beyond default 72 hours for personal reference, or reduce it for privacy preference).
- Priority access to new app features during beta periods.
- Early access to new ONNX model versions before general rollout.

**target user:** Health-aware individuals who have completed Ghost Mode and want to use the resilience score as a long-run health indicator, not just a daily check. People who share data with a personal physician, integrate it with other health records, or are doing structured self-tracking.

**pricing rationale:** $9.99/month is positioned below most subscription health apps while above commodity utility apps. The annual option at $79 (equivalent to $6.58/month) is the preferred offering — annual subscribers represent genuine engagement and reduce churn. No free trial for Pro; the Free tier is the trial.

## research tier

**name:** Myelix Research

**price:** $29/month per researcher, with volume pricing for institutional accounts

**what it includes, in addition to Pro:**

- Full plugin SDK tooling: study management dashboard, participant enrollment and consent workflow management, plugin performance analytics.
- Access to the opt-in Myelix research participant pool (anonymized, aggregate data only, governed under Myelix's data governance framework and the researcher's own IRB protocol).
- Study design tools: configure which derived features a study plugin accesses, set study duration and data collection windows, manage participant withdrawal.
- Plugin submission and review portal: submit plugins for Myelix security and privacy review, required before listing in the community plugin directory.
- Direct support channel with the Myelix core team for plugin development questions.
- Co-branding of approved study plugins in the community plugin directory ("Powered by [Institution]").

**institutional volume pricing:** Organizations purchasing Research seats for a team of researchers are billed at $19/month per seat (minimum 5 seats). Research partnership agreements (annual contracts, custom data governance scope) are priced separately — see the business plan.

**target user:** Academic researchers, independent researchers, and research teams at digital health companies building behavioral study plugins. The primary value proposition is the consent and privacy infrastructure that removes the IRB complication of raw biometric transmission. Myelix's architecture means the researcher never has access to raw data — the system enforces this, which simplifies ethics review for the researcher's institution.

**pricing rationale:** $29/month is aggressive for an individual researcher but appropriate for the value: the alternative is building a custom mobile data collection platform with consent management, participant recruitment, and privacy infrastructure from scratch. For an institution, $19/month per researcher on an annual contract is a rounding error against the cost of that alternative.

## clinical tier

**name:** Myelix Clinical

**status:** Phase 3 roadmap — not yet available

**price:** annual institutional license (projected $10,000–$50,000/year depending on facility size and integration scope)

**planned inclusions:**

- All Pro and Research tier features for enrolled clinical staff.
- EHR integration: HL7 FHIR-compatible export for longitudinal behavioral data.
- Provider dashboard: aggregate, de-identified population-level reporting for patient cohorts (patients opt in to provider data sharing explicitly).
- Clinical export: structured PDF report suitable for inclusion in clinical notes, showing longitudinal resilience score history and trend annotations.
- Compliance documentation package: supporting materials for clinical validation and regulatory documentation.
- Dedicated onboarding and support for clinical deployment.

**availability:** Clinical tier requires completion of the FDA regulatory track for the clinical export feature (510(k) pathway) before general availability. Enterprise pilot programs with selected healthcare partners will precede general availability. Timeline: no earlier than 24 months from community launch.

## future tiers

**family plan (under consideration)**
A single subscription covering up to 5 family members on their own devices, each with independent baselines and privacy vaults. No data sharing between family members without explicit mutual consent. Targeted at adults monitoring cognitive health in aging parents while also monitoring themselves. Pricing would be approximately 2.5× the Pro individual rate.

**lifetime license (under consideration)**
A one-time payment option for Pro, targeted at users who prefer not to manage subscriptions for personal health tools. Pricing in the $199–$249 range. Risk: revenue recognition complexity and the difficulty of lifetime support obligations. Would only be offered if the user base is large enough to make the actuarial math comfortable.

**academic institution bulk license (under consideration)**
Annual license covering all researchers at a single institution, replacing per-seat Research pricing. Targeted at universities and research hospitals that want to manage Myelix access centrally and integrate with institutional procurement. This is an extension of the Research tier, not a distinct product.

## packaging guardrails

The following features must never be paywalled at any tier:

- Privacy vault and consent management. Every user, on every tier, has full visibility and control over what is collected and stored.
- Core sensor collection, baseline modeling, and daily resilience score. The product's fundamental value proposition is not a premium feature.
- Local encrypted storage and full offline functionality.
- The ability to delete all local data. This is a privacy right, not a feature.

These guardrails exist for ethical and regulatory reasons as well as brand reasons. A product that paywalls privacy controls would undermine the trust on which all commercial tiers depend.
