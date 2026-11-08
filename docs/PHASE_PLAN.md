# Build phases — Amazing Player

**Status Nov 8, 2026:** Reconstructed WinForms/VLC bootstrap is already merged and CI-compiled. Mark reports passing Windows smoke test. Continue **from main**; do not regenerate a new starter. Original school-compliant design, storage, modes, Favorites and complete test evidence remain pending.

## P0 — Mark: baseline and contract — COMPLETE FOR RECONSTRUCTED STARTER
The professor's posted Form1 sample + PDF guide were adapted into a concrete `BasePlayerForm` and derived temporary `Form1`. [PR #8](https://github.com/NeatKnight18586/amazing-oop10-media-player/pull/8) is merged. CI restore/build and publish passed; Mark confirms basic playback. Original instructor .sln/Designer files were **not** supplied. Preserve this distinction in the portfolio.

## P1 — Mark: shared VLC core — BOOTSTRAP WORKS, REMAINING INTEGRATION PENDING
Own `BasePlayerForm.cs`, `Program.cs`, .csproj. Maintain MP3/MP4 playback, API hooks, status, elapsed getter and thread-safe media-end handling. Implement actual MP4 resolution metadata (FR2), plus narrow hooks for Chris/Mitchel/Shanen. Avoid rewriting other owners' code.

## P2 — Darrel: OOP models, storage and sorting
Own `MediaItem.cs`, `AudioItem.cs`, `VideoItem.cs`, `LibraryStore.cs`, `LibrarySorter.cs` and tests. Models already exist; **extend carefully**, do not duplicate them. Implement compatible six-field `Library.txt` save/load with corrupt-line skip count, and only hand-written insertion sort by Title/Artist. Ship tests; Shanen/Mark integrate saving/reloading and ListView sorting. [Issue #3](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/3).

## P3 — Chris: shuffle / repeat policy
Own `RepeatMode.cs`, optionally `PlaybackOrder.cs`, focused tests and end-of-track behavior documentation. Mark connects the policy to `BasePlayerForm.PlayNextOrStop()`, Shanen adds UI. Cover off/all/one, shuffle, last and single item. [Issue #4](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/4).

## P4 — Shanen: original UI + splash
Own existing `Form1.cs` **code-built UI**, `Theme.cs`, `SplashForm.cs`, `TeamInfo.cs`, and `app/MediaPlayerPrj/Assets/`; avoid assuming a designer-generated file exists. Implement original minimal PNG skin, visible menus/shortcuts and status, elapsed clock, 5-member 3s skippable splash, and controls for P2/P3/P5 after APIs settle. Mark edits `Program.cs` for splash. VideoView must not be obscured; validate 100% and 125%. [Issue #5](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/5).

## P5 — Mitchel: Favorites / QA
Own `FavoritesStore.cs`, `FavoritesService.cs`, focused tests and portfolio QA materials. Persist favorites in separate `Favorites.txt`, not inside required six-field Library.txt; deduplicate and handle missing/corrupt rows. Shanen wires heart/filter UI. Plan >=15 test cases (see docs/TEST_PLAN.md) and record real executed results after integration. [Issue #6](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/6).

## P6 — Mark + team: integration, release, portfolio
PR merge order depends on readiness: P2 storage → P3 playback policy → P5 Favorites service → P4 final UI; Shanen may work on assets/mockups concurrently. Mark owns cross-file hooks, CI verification, real Windows regression, release x64 and second-PC verification, UML and PDF portfolio. Everyone supplies authentic commits, tests, reflections and explanation of their work.

## Review and commit guardrail
One owner per conflicting file; changes outside assigned allowlist require lead review. Do not split changes merely to reach a commit target. Merge only after CI and scoped tests pass; indicate tests not run. Recheck `main` before every new branch/PR.

## Instructor deadlines
M1–M3 Nov 10; M4 Nov 12; M5 Nov 14, 2026. **Privacy:** repository is currently public but guide requires private; owner must correct immediately.
