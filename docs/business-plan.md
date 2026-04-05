# Myelix — Business Plan

## executive summary

Myelix is a passive cognitive health monitoring platform built on a privacy-first, on-device architecture. The Community edition — the subject of this document — is open source and free. It serves as the foundation for a commercial ecosystem built on premium features, research partnerships, and eventually institutional licensing.

The business model is a flywheel: the open-source community edition drives adoption at scale, which builds a research participant pool that attracts institutional research partners, which funds model improvement that makes the premium product more valuable, which drives conversion to paid tiers.

Revenue is generated through premium feature subscriptions, research data partnership agreements (anonymized aggregate only, explicit opt-in required), and institutional licenses for clinical and enterprise contexts. The open-source core is never monetized directly — it is the distribution mechanism.

## problem

Cognitive health monitoring currently sits in one of two places: expensive clinical assessments that require a visit and produce a point-in-time snapshot, or consumer wellness apps that produce shallow self-reported data and have dubious privacy practices.

Neither option serves the growing population of adults who want longitudinal, private, scientifically grounded visibility into their own cognitive patterns. Clinical assessment is too expensive, too infrequent, and too invasive to function as continuous monitoring. Consumer wellness apps trade user data for convenience, which is the wrong tradeoff for health data.

The gap is a passive, on-device, privacy-preserving system that builds a personalized baseline and tracks drift over time — without requiring clinical infrastructure, without requiring data surrender, and without requiring any daily effort from the user.

## solution

Myelix Community is a .NET 10 MAUI app for Android, iOS, and Windows that passively observes device interaction signals (typing cadence, gait, touch dynamics), builds a personal behavioral baseline on-device using a TCN model running via ONNX Runtime, and surfaces a daily resilience score. Raw biometric data never leaves the device. Only anonymized aggregates may sync, and only with explicit user opt-in.

The architecture is open source and auditable. The plugin SDK allows researchers to extend the scoring pipeline for study purposes. The federated learning client participates in model improvement rounds without transmitting identifiable data.

The free community edition does everything a health-aware individual needs for passive self-monitoring. Premium tiers add analytics depth, data export, and institutional integration.

## market focus

**primary: health-aware adults in the US and EU**
Adults over 35 who proactively manage their health, are early adopters of health technology, and are skeptical of data-harvesting apps. Market size: the US alone has approximately 100 million adults in this demographic profile. Penetration at 0.1% yields 100,000 active users — a meaningful research participant pool and an addressable paid tier.

**secondary: academic and independent researchers**
Researchers in cognitive neuroscience, behavioral medicine, and digital health who need a passive data collection platform with participant consent built in. The plugin SDK is the product here. Research institutions pay for partnership agreements that give them aggregate data access under strict governance.

**tertiary: healthcare providers (Phase 3)**
Clinical practices, hospital systems, and population health programs that want longitudinal behavioral data integrated with EHR systems. This segment requires significant regulatory navigation and is deferred to Phase 3.

## product strategy

**Phase 1: establish the open-source community**
Release myelix-community as an open-source project on GitHub with complete documentation, a working plugin SDK, and a reference plugin. Goal: 1,000 GitHub stars, 100 active contributors, 5,000 app installs within 12 months of launch. This phase is pre-revenue. Investment comes from founding team resources.

**Phase 2: launch premium tier and research partnerships**
Introduce the Pro tier (advanced analytics, trend history, clinical-grade export) and the Research tier (full plugin SDK, study management, participant controls). Begin outreach to research institutions for partnership agreements. Goal: 500 Pro subscribers and 3 active research partnerships within 6 months of premium launch.

**Phase 3: clinical track**
Seek FDA 510(k) clearance for the clinical export feature. Launch the Clinical tier targeting healthcare providers. This phase is contingent on Phase 2 revenue and regulatory timeline. No earlier than 24 months from community launch.

## revenue model

**Pro tier subscription**
$9.99/month or $79/year per user. Targets health-aware individuals who want more than the daily score: historical trend analysis, per-signal deep dives, export to PDF/CSV for personal records, and priority access to new features.

**Research tier subscription**
$29/month per researcher. Full plugin SDK with study management tools, participant consent workflow management, study analytics dashboard, and access to Myelix's opt-in research participant pool (anonymized, aggregate data only, per IRB protocol). Priced for individual researchers; institutional volume pricing available.

**Research partnership agreements**
Custom agreements with universities, medical schools, and research institutes. Structure: institution pays an annual partnership fee ($10,000–$100,000 depending on scale and data access scope) in exchange for priority plugin review, dedicated aggregate data access under a governance agreement, co-authorship opportunities on Myelix model papers, and early access to the clinical track APIs. These agreements are the primary revenue driver in Phase 2.

**Clinical tier licensing**
Per-seat or per-organization license for healthcare providers. Priced separately from consumer tiers. Requires completion of the regulatory track. Revenue from this tier is a Phase 3 projection only.

## infrastructure strategy

**myelix-community (open source)**
Hosted on GitHub. Build infrastructure via GitHub Actions. App distribution via Apple App Store, Google Play, and Microsoft Store. No server-side infrastructure required for core functionality — the app works fully offline. CDN-hosted ONNX model updates for users who opt in to model refresh.

**myelix-core (proprietary)**
Server-side infrastructure required for: federated learning coordination (Flower server), aggregate sync storage, research partnership data governance, premium feature API. Hosted on managed cloud infrastructure (targeting AWS or Azure). Designed to handle the narrow aggregate sync API surface, not raw biometric data — the server never processes raw sensor data.

**cost structure**
In Phase 1: primarily personnel (engineering, product, community management). Infrastructure costs are minimal — the open-source community bears its own compute. In Phase 2: myelix-core hosting costs scale with federated learning participation and sync volume, but the aggregate-only data model keeps storage costs low relative to raw data approaches.

## go-to-market summary

Launch sequence: internal dogfooding → researcher beta (plugin SDK focus, private GitHub releases) → public launch of community edition with Ghost Mode as the headline feature → premium tier → clinical track. Full detail in the go-to-market launch plan document.

## success metrics

- Phase 1: GitHub stars, app installs, plugin ecosystem activity (number of plugins published), contributor count, media coverage in privacy and digital health communities.
- Phase 2: Pro subscriber count, research partnership revenue, research partnership data governance agreements signed, federated learning opt-in rate (proxy for trust signal).
- Phase 3: clinical tier LOIs, regulatory filing timeline, healthcare provider pilots.
