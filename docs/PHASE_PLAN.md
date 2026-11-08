# Build phases — Amazing Player

Do not run five AI agents editing the same WinForms form. Build workstreams behind clear file boundaries, then integrate them in dependency order. Each phase has a validation gate.

## P0 — Mark: baseline and contract (BLOCKING)
Import complete instructor .sln/.csproj/WinForms files into app/ without refactoring. Restore and build unchanged starter on Windows; record actual APIs, package versions and framework in INTEGRATION_CONTRACT.md. Gate: valid, understood baseline or a transparent recorded blocker. Designers may create mockups meanwhile.

## P1 — Mark: VLC core and shared integration
Own BasePlayerForm, Program and project settings. Preserve designer-safe, concrete base; implement/verify media import, VLC playback, missing-file errors, metadata, UI-thread marshaling for end-of-media, elapsed-time getter and cleanup. Gate: MP3/MP4 genuinely play; status readable; existing starter behavior preserved.

## P2 — Darrel: models, storage and sorting
Own media models, LibraryStore, LibrarySorter and focused tests. Implement validated titles, polymorphic Audio/Video, six-field load/save, damaged-line skip count, and ONLY hand-written insertion sort by Title and Artist. Gate: deterministic model/storage/sorter test cases, API compatible with real starter. No changes to base or UI.

## P3 — Chris: playback modes
Own RepeatMode, isolated end-of-track decision helper and tests (after baseline API). Cover Repeat Off/All/One, shuffle on/off, last item and single item. Gate: deterministic cases, documented end behavior, integration request to Mark for single base-player hook. No direct edits to BasePlayerForm.

## P4 — Shanen: original UI / splash
Own Theme, derived group form and Designer, SplashForm, TeamInfo, Assets and layout spec. Form inherits BasePlayerForm, has >=2 real overrides and one unique member. MenuStrip headings and shortcuts call existing methods, elapsed time shows total, video unobstructed, controls remain visible and design is readable at 100%/125%. Gate: screenshots of actual WinForms app; startup splash shows all five correctly.

## P5 — Mitchel: Favorites and quality
Own FavoritesStore, FavoritesService, tests and QA/documentation. Favorite/unfavorite, clear on-screen indicator, favorites-only view/filter, persist across app restarts without changing six-field Library.txt. Interface wiring requested of Shanen (not edited by Mitchel). Gate: tests for empty, missing, duplicated/corrupt favorites data and demo.

## P6 — Mark + team: integration, release, portfolio
Review PRs in order: P2 data → P3 playback policy → P5 favorites → P4 UI; Mark integrates shared hooks as needed, runs build, regression + negative tests, publishes Windows x64 Release with VLC natives/assets and verifies on **another PC**. Capture bug log, real history/screenshots, 15+ actual test results, UML, manual, deployment guide and video demo; all five participate.

## Safe working rule
One owner per conflicting file at a time. Missing API? Request lead action. No branch merged without reviewer + actual known test status. Commit real milestones, not arbitrary quotas.

## Instructor schedule
M1–M3 due Nov 10; M4 Nov 12; M5 Nov 14, 2026 (cutoff times not in PDF).
