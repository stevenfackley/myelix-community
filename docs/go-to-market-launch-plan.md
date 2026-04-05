# Myelix Community — Go-to-Market Launch Plan

## objective

Establish Myelix Community as the credible, privacy-first alternative to data-harvesting cognitive health apps. Build an active open-source community and research user base that creates the adoption flywheel for premium and institutional tiers. The launch is not a single event — it is a sequence of five phases designed to validate each audience segment before the next one is engaged.

## launch sequence

**phase 0: internal dogfooding (weeks 1–6)**
The founding team and close collaborators use the app daily. Goal: find the friction in Ghost Mode onboarding, validate that the 30-day baseline produces a meaningful score for real users, identify platform-specific collection gaps (battery optimization, background execution edge cases). No public presence. GitHub repository is private. No marketing activity.

Exit criteria: at least 10 people have completed the 30-day Ghost Mode period and received a resilience score. Score is considered meaningfully differentiated day-to-day by at least 8 of 10 participants.

**phase 1: researcher beta (weeks 7–16)**
GitHub repository goes public. v0.1 tagged. Outreach to a curated list of 30–50 academic researchers in cognitive neuroscience, behavioral medicine, and digital health. The message is technical: here is an open-source platform with a plugin SDK, participant consent built in, and a privacy-by-design architecture. We want researchers to build on it and stress-test the plugin sandbox.

Deliverables for researcher beta: complete plugin SDK documentation, reference plugin (Myelix.Plugin.Sample), PluginDevelopment.md guide, FederatedLearning.md guide, and a private Discord for beta participants.

Goal: 5 researchers build and publish working plugins. Collect structured feedback on SDK ergonomics, data access model, and study management needs.

Exit criteria: plugin SDK documented to a quality where a researcher unfamiliar with the codebase can build a working plugin in one day. At least one external researcher completes a plugin and provides written feedback.

**phase 2: public community launch (weeks 17–24)**
v1.0 released to Apple App Store, Google Play, and Microsoft Store. GitHub repository fully public with complete documentation suite. Launch strategy focuses on earned media in privacy, digital health, and open-source communities — not paid advertising.

Launch message: "The only cognitive health monitor that runs entirely on your device." Anchor on Ghost Mode as the flagship feature — a 30-day silent baseline period that requires no input and reveals nothing until the model is ready.

Channels:
- Hacker News: Show HN post focused on the architecture (ONNX on-device, Flower federated learning, plugin sandbox). Technical audience. Lead with the architecture.
- r/privacy, r/selfhosted: lead with the privacy architecture — no account, no cloud, open source.
- Digital health Twitter/LinkedIn: lead with the product concept — passive monitoring, personal baseline, not population benchmarks.
- Academic researcher networks: direct outreach building on Phase 1 researcher relationships.

Goal: 1,000 GitHub stars, 5,000 app installs, 3 press mentions in relevant outlets (Wired, The Markup, a digital health publication) within 8 weeks of public launch.

**phase 3: premium tier launch (weeks 25–40)**
Introduce Pro and Research tiers. The launch is informed by Phase 2 usage data: which features are most used, what do active users ask for in GitHub issues and Discord, what do researchers need beyond the base SDK.

Pro tier launch: in-app upgrade flow, no separate marketing push. Users who have completed Ghost Mode and are using the daily score are the conversion target. Offer: historical trend analysis, per-signal deep dives, export.

Research tier launch: direct outreach to institutions. Targeted at department-level purchasing decisions. Pitch: complete participant consent infrastructure, privacy-compliant data collection, no IRB complications from raw biometric transmission because there is none.

Goal: 500 Pro subscribers and 3 signed research partnership agreements within 6 months of premium launch.

**phase 4: clinical track (months 12–24)**
Initiate FDA pre-submission meeting for 510(k) pathway for the clinical export feature. Launch Clinical tier in pilot with 2–3 healthcare provider partners identified through Phase 3 research partnerships.

This phase is not detailed in this document — it is contingent on Phase 3 revenue, regulatory timeline, and clinical partnership development.

## initial audience

The launch-day audience is defined precisely. It is not "everyone interested in cognitive health." It is:

- Privacy-conscious adults who use iOS or Android and have read at least one article about health app data practices. They are motivated by data ownership, not by fear of cognitive decline.
- Academic researchers who have had difficulty getting IRB approval for mobile biometric studies because of data transmission concerns. Myelix's architecture removes the transmission problem.
- Open-source contributors interested in .NET MAUI, on-device ML, or federated learning. The technical architecture is the hook.

These three groups are small and specific. Serving them well in Phase 1 and 2 creates the credibility that reaches a broader audience in Phase 3.

## core launch offer

The core launch offer is the free community edition with Ghost Mode. It is complete, functional, and requires no premium upsell to be useful. This is intentional. The launch offer must be credible as a standalone product — not a free trial of something else.

The secondary launch offer is the open-source repository: clean code, complete documentation, a working reference plugin, and an invitation to contribute. For the researcher and contributor audiences, the GitHub repository is the product.

## acquisition channels

**organic / earned media**
Primary channel in Phase 1 and 2. Focus on communities where the privacy and open-source angles generate genuine word of mouth: Hacker News, privacy-focused subreddits, digital health Twitter, academic conferences and preprints.

**app store optimization**
The app store listing is a significant acquisition surface for the consumer segment. Optimize for search terms around cognitive health, brain health, privacy, and passive monitoring. The Ghost Mode concept is a differentiating keyword with low competition.

**research community outreach**
Direct, relationship-driven. Identify 100 researchers in relevant fields through Google Scholar, institutional faculty pages, and conference program committees. Personalized outreach. Small volume, high conversion.

**content**
Technical blog posts explaining the architecture (on-device ONNX inference, Flower federated learning, the plugin sandbox model). These serve double duty: SEO for developer and researcher audiences, and credibility signals that support press outreach.

**github as acquisition**
An excellent GitHub presence is acquisition. Comprehensive README, good first issues, responsive maintainers, visible contributor activity. The repository is the product for developer and researcher audiences.

## retention strategy

**for individual users**
The retention mechanism is the baseline itself. After 30 days, the user has a personalized model that only gets more accurate over time. Switching apps means starting over. There is no gamification, no streak mechanics — the lock-in is the quality of the data. Support retention with: meaningful score history that accumulates value, plugin discovery that expands utility, and responsive handling of platform-specific collection issues (battery optimization, background execution).

**for researchers**
Researchers are retained by the quality of the SDK, the responsiveness of the core team to plugin development questions, and the growing participant pool that makes Myelix studies statistically viable. The Research tier includes study management tooling that creates operational dependency.

**for contributors**
Open-source contributor retention is driven by: responsive maintainers, meaningful issues, clear contribution guidelines, recognition in changelogs and release notes, and a product that is used by real people (which validates the contribution).

## metrics

**phase 1 and 2 (community health)**
- GitHub stars and forks
- App installs by platform
- 30-day Ghost Mode completion rate (proxy for product viability — if users are not completing the baseline, the product is failing)
- Daily active users post-baseline
- Plugin SDK: number of external plugins published
- Community: Discord member count, GitHub issue response time

**phase 3 (revenue)**
- Pro tier subscriber count and monthly recurring revenue
- Research tier subscriber count
- Research partnership agreements signed and total contract value
- Federated learning opt-in rate (trust proxy)
- App store rating and review sentiment

**leading indicators to watch**
The 30-day Ghost Mode completion rate is the most important early signal. If fewer than 30% of users who start the app complete the baseline period, the product is not delivering on its core promise. Investigate: onboarding clarity, permission friction, background collection reliability, and whether the 30-day wait is communicated in a way that sets correct expectations.
