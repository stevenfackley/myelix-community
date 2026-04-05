# Myelix Community — Product Design Document

## design objective

Myelix should feel like a health monitor, not a surveillance tool. The design objective is to build an experience that earns trust through restraint: restraint in what it asks for, restraint in what it shows, and restraint in how often it interrupts. During collection the app is invisible. During review it is clear and calm. It never alarms. It never moralizes. It reports.

The single guiding principle: **the app should feel invisible during collection and clear during review.**

## product tone

Scientific without being clinical. Calm without being cold. Empowering without being motivational. Myelix does not celebrate streaks or punish gaps. It does not use gamification language. It does not push notifications asking "how are you feeling today?" It observes, models, and reports.

The visual language should reinforce this tone: low contrast, generous whitespace, typographic hierarchy over color hierarchy, no status indicators that read as warnings or achievements.

## core ux principles

**passive first.** The app should require zero daily engagement to function. Every design decision should ask: does this require the user to do something? If yes, is it absolutely necessary? Most of the time the answer should be no.

**consent is visible, not buried.** Privacy controls are a primary navigation destination, not a settings sub-sub-menu. The user should be able to find, review, and change what is being collected in three taps from any screen.

**no false precision.** The resilience score is a relative signal, not a medical reading. Design must not dress it up as clinical output. No medical iconography, no red/amber/green status lights, no urgency framing. The number is presented with its uncertainty context.

**progressive disclosure.** The default view shows the minimum useful information. Drill-down is available but not required. A user who opens the app, sees their score, and closes it should have had a complete and useful interaction.

**platform conventions respected.** On Android the experience follows Material You guidelines. On iOS it follows Human Interface Guidelines. On Windows it follows WinUI 3 patterns. The design system is adaptive; it is not a lowest-common-denominator wrapper.

## primary user journeys

**journey 1: first-time setup (ghost mode onboarding)**

The user installs the app and opens it for the first time. The onboarding flow has five screens:

1. Welcome screen — one sentence about what Myelix does. No feature list. A single call to action: "get started."
2. What Ghost Mode is — explains the 30-day baseline period in plain language. No technical detail. Emphasis: you will not see scores yet, and that is by design.
3. What is collected — a list of signal types with a one-line plain-language description of each. Each signal type has a toggle. All are on by default. The user can turn any off before granting permissions.
4. Permissions — the app requests only the OS-level permissions needed for the signals the user left enabled in step 3. Each permission prompt is preceded by a one-sentence explanation of why the permission is needed.
5. Confirmation — tells the user Ghost Mode is now active, shows the 30-day progress indicator at zero, and surfaces a single action: "go to privacy vault" for users who want to review settings immediately, or a dismiss action to close onboarding.

At no point does the onboarding flow ask for an account, email address, or any identifying information.

**journey 2: daily check-in (active mode)**

The user opens the app during active mode to check their resilience score. The journey is:

1. Home screen shows the resilience score, a directional trend indicator, and the date of last computation.
2. User taps the score to open the breakdown view, which shows per-signal contributions as a simple proportional list.
3. User swipes left on the home screen to see a 30-day history view as a minimal line chart.
4. User closes the app. Total interaction: under 30 seconds.

**journey 3: privacy review**

The user wants to understand what the app is collecting or change what is enabled.

1. User navigates to Privacy Vault (primary nav item).
2. Screen shows: active sensors with live status indicators, data retained locally with a retention timeline, sync log showing what (if anything) was sent to myelix-core in aggregate form, and per-signal consent toggles.
3. User disables a signal. The app confirms immediately and shows a projected effect on score confidence (e.g., "disabling gait reduces score input by approximately 30%"). No alarm, no dark pattern pressure to re-enable.
4. User can trigger local data deletion from this screen: per signal type or all data.

**journey 4: plugin installation**

The user installs a research plugin.

1. User navigates to Plugins (primary nav item).
2. Directory lists available plugins with: name, research institution, study description, required feature access, participant count.
3. User taps a plugin to read the full study description and data access manifest.
4. User taps Install. The app shows the plugin's consent screen, which is authored by the plugin developer and reviewed by Myelix before listing. User confirms.
5. Plugin appears in the Installed list with an enable/disable toggle.

## core screens

**home screen**
The score dominates the screen. Large numeric display, typographic treatment only — no dial, no gauge, no color-coded ring. Below the score: a brief plain-language summary (e.g., "patterns consistent with your baseline" or "some drift detected over the past 3 days"). Below that: a small 7-day sparkline. Navigation bar at the bottom: Home, History, Privacy Vault, Plugins.

**history screen**
30-day line chart of the resilience score. Clean, minimal, no annotations by default. Tap a data point to see the date and score. Below the chart: a signal breakdown heatmap showing which signals were most active per day. No comparison to population data.

**privacy vault screen**
Three sections: Active Signals (list with toggles and status), Data on This Device (storage size, retention policy, deletion action), Sync Log (chronological list of aggregate sync events with payload summaries). Section headers use a subdued typographic treatment. No icons used as status indicators.

**plugin screen**
Two tabs: Installed and Discover. Installed shows enabled plugins with their last execution time and a disable option. Discover shows the plugin directory. Each plugin card: name, institution, one-line description, a "data access" tag showing the lowest access level the plugin requests.

**ghost mode progress screen**
Accessible from the home screen during the 30-day baseline period. Shows: days elapsed, days remaining, a fill indicator (no partial scores shown), and a brief reminder of what happens when the baseline is complete. This screen is informational only. No engagement mechanics.

## interaction requirements

**permissions**
The app must request permissions contextually, not upfront as a list. Each OS permission dialog must be preceded by an in-app explanation screen that explains why the permission is needed in plain language. If a user denies a permission, the app must gracefully degrade — disable the relevant sensor, adjust the score confidence disclosure, and not ask again unless the user re-enables the signal from Privacy Vault.

**consent revocation**
Disabling a sensor in Privacy Vault must take effect immediately. The app must not retain a grace period or buffer of recently-collected data after revocation. Data associated with the revoked signal must be queued for deletion and deleted on the next retention sweep (within 24 hours).

**score presentation**
The score must always be accompanied by a confidence indicator reflecting how many signals are active and how complete the baseline is. A score computed on fewer than three active signals must show a reduced-confidence label. The app must never present a score as medical output.

**notifications**
In v1, the app sends no push notifications. The only system notification is a weekly passive confirmation that background collection is active (platform-required for background sensor access on some Android versions). This notification is minimal: "Myelix is running quietly in the background."

**error states**
If collection is interrupted (permission revoked at OS level, battery saver mode, background process killed), the app surfaces the interruption on next open — not as an alarm but as a status note: "collection paused due to battery optimization settings." It provides a direct link to the relevant OS setting. It does not re-request permissions automatically.

**accessibility**
All interactive elements must meet WCAG 2.1 AA contrast requirements. The score display must work with system large text settings without truncation. All charts must have accessible text alternatives. VoiceOver and TalkBack support is required for all primary navigation flows.

**onboarding exit**
A user must be able to exit the onboarding flow at any time and return to it later. If the user exits before granting any permissions, the app enters a dormant state and resurfaces the onboarding prompt on next launch. The app does not collect any data before permissions are granted.
