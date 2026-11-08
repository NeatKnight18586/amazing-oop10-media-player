# Independent AI task packs — Group 1 Amazing

**Purpose:** Four agents work simultaneously on separate branches; each contributor remains responsible for understanding and reviewing their work. These are engineering responsibilities, not a script for manufacturing commits.

**Verified starting point:** as of Nov 8, 2026 all four feature branches matched `main` at commit `9a544fa636fe9b42f9cef994e67669ca9da49fe7`. The reconstructed .NET 8 WinForms/LibVLC bootstrap is merged and builds in GitHub Actions; Mark reported a successful Windows smoke test. **Source changes may occur later: always fetch and inspect before editing.**

## Common hard rules — apply to EVERY agent

1. Clone/open `https://github.com/NeatKnight18586/amazing-oop10-media-player`. Work **only** in the exact pre-created feature branch named below; not `main`. Verify `git branch --show-current` before writing/committing.
2. Read `AGENTS.md`, `docs/PRD.md`, `docs/INTEGRATION_CONTRACT.md`, `docs/PHASE_PLAN.md`, `docs/TEAM_HANDOFF.md` and the assigned issue. Treat the already-checked-in source as authoritative; do not regenerate the project.
3. Edit only allowlisted paths. If a file must be edited by another owner, STOP and request the minimal public API/hook in the GitHub issue; do not sneak in shared edits.
4. **Never** modify `BasePlayerForm.cs`, `Program.cs`, `MediaPlayerPrj.csproj`, `.github/workflows/*`, other teammates' files, or `main` without lead approval. **Shanen alone** may edit `Form1.cs`; everyone else must use integration requests for UI changes.
5. Each checkpoint below must produce genuine source, tests, asset integration or documented technical evidence. There is **no requirement to exactly match one checkpoint to one commit** if work does not warrant it; commit only when something meaningfully changed and review/test evidence exists. Instructor requires **at least 10 meaningful commits from EACH member**. Don't invent changes, tests, names, authorship, or superficially split a single change.
6. Use actual Git author credentials per person; review `git diff` and stage only relevant files. Suggested prefixes: `feat:`, `fix:`, `test:`, `docs:`, `refactor:`.
7. After each meaningful checkpoint, report modified paths, implementation summary, test command/result, and any blocked integration. Only commit/push after the person has inspected the diff and approves. Never auto-merge.
8. If GUI/Visual Studio is unavailable, run the available checks and say **NOT RUN** for unexecuted Windows steps. CI green is necessary but not proof the GUI, VLC and assets work. Raise blockers to Mark.

## Shared Git routine / Antigravity UI equivalent

```bash
git fetch origin
git switch <YOUR_ASSIGNED_BRANCH>
git status
# Edit and test ONLY your allowed paths.
git diff --stat
git diff
git add <EXACT_FILES_FOR_THIS_CHANGE>
git commit -m "feat: concise description of real completed work"
git push -u origin <YOUR_ASSIGNED_BRANCH>
```

Do not use `git add .` casually, because it can stage another contributor's or runtime files. The commit author should be the teammate who genuinely performed and reviewed the work. Open a PR **from your branch to main**; Mark reviews/merges. Your PR body must include issue, FR, files, executed tests/results, any NOT RUN checks, and cross-owner API requests.

## Darrel Cango — P2: models, LibraryStore and insertion sorting

**Branch:** `feat/darrel-library`  
**Issue:** https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/3  
**Allowlist:** `app/MediaPlayerPrj/MediaItem.cs`, `AudioItem.cs`, `VideoItem.cs`, `LibraryStore.cs`, `LibrarySorter.cs`; `tests/DarrelLibraryTests/**`; `docs/workstreams/darrel/**`. Do not change `Form1.cs`, base player or project configuration.

### Paste to your AI

```text
Act as my C# developer and teacher, working only on the Amazing Player
OOP10 issue #3. Repository:
https://github.com/NeatKnight18586/amazing-oop10-media-player
My existing branch is feat/darrel-library. Verify this is checked out.

Read AGENTS.md, docs/PRD.md, docs/INTEGRATION_CONTRACT.md,
docs/PHASE_PLAN.md, docs/TEAM_HANDOFF.md, and issue #3.
Inspect the checked-in C# models before editing; several already exist.
The six-field persistence format from the FINAL PDF is:
Type|Title|Artist|HH:MM:SS|FilePath|AlbumOrResolution.
AudioItem/VideoItem use polymorphic MediaItem. File I/O is separated.
Create LibraryStore.Save/Load, count skipped corrupt rows, safely handle
missing first-run Library.txt, and use HAND-WRITTEN INSERTION SORT by
both Title and Artist: no List.Sort, OrderBy or equivalent framework
sort in FR12. Sorting works on a copy; it does not lose the playing
item's identity. Keep C# API compatible with the integration contract.

Allowed paths ONLY: existing MediaItem.cs, AudioItem.cs, VideoItem.cs;
new LibraryStore.cs, LibrarySorter.cs; tests/DarrelLibraryTests/** and
docs/workstreams/darrel/**. Do not touch Form1.cs, BasePlayerForm.cs,
Program.cs, app project files, other team docs or branches.

Work on one small coherent checkpoint at a time. Propose exact files,
write code and real tests, run available checks and show me the diff.
Pause for my review before I approve each meaningful commit. Use
feat:/fix:/test:/docs:/refactor: commit prefixes and my Git identity.
Push only to feat/darrel-library. If Form1 needs changes, write a
precise hook request for Shanen/Mark rather than editing it.
I must understand and explain the implementation.
```

### Suggested 10 independently meaningful checkpoints / example commit messages

1. Validate and document existing abstract `MediaItem` invariants (blank artist/title, nonnegative duration, path handling) — `fix: validate media item metadata consistently`.
2. Tighten `AudioItem` and `VideoItem` constructors and polymorphic `Extra` behavior when needed — `fix: normalize audio and video extra metadata`.
3. Implement six-field `LibraryStore.ParseLine` with safe duration and media type handling — `feat: parse library records into media subclasses`.
4. Implement `LibraryStore.Load`, first-run empty result and skipped corrupt-line count — `feat: recover valid tracks from library text file`.
5. Implement `LibraryStore.Save`, UTF-8 serialization and meaningful IO exception behavior — `feat: persist media library in required format`.
6. Add meaningful load/save round-trip tests across MP3 and MP4 entries — `test: cover media library persistence round trips`.
7. Add sort-key/comparison rules for Title and Artist, including blanks/case-insensitivity — `feat: compare playlist items by title and artist`.
8. Implement manually coded stable insertion sort over a caller-provided copy — `feat: implement required insertion sort for playlist`.
9. Add robust sorting tests (ties, already sorted, reverse order, mixed media) — `test: verify insertion sort boundary cases`.
10. Add remaining malformed-row/serialization tests and a brief worked sort trace with best/worst complexity — `docs: explain sorting trace and storage edge cases`.

**Important:** Existing source may already satisfy some checkpoints. Do not create a no-op commit; improve missing tests, error handling or real behavior instead, while still meeting the course's substantive contribution requirement.

**Handoff to Mark/Shanen:** `LibraryStore.Save/Load` APIs; sort method, sort-key enum, and exact example to load at startup, save after import, sort a copy and update UI/CurrentIndex.

## Chris Garcia — P3: shuffle/repeat policy

**Branch:** `feat/chris-playback-modes`  
**Issue:** https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/4  
**Allowlist:** `app/MediaPlayerPrj/RepeatMode.cs`, `PlaybackOrder.cs` (if needed), `tests/ChrisPlaybackTests/**`, `docs/workstreams/chris/**`. **No** `BasePlayerForm.cs` or `Form1.cs` changes.

### Paste to your AI

```text
Act as my C# developer and teacher for Amazing Player issue #4.
Repository: https://github.com/NeatKnight18586/amazing-oop10-media-player
Existing branch: feat/chris-playback-modes. Verify it is checked out.

Read AGENTS.md, PRD, INTEGRATION_CONTRACT, PHASE_PLAN, TEAM_HANDOFF,
issue #4 and actual BasePlayerForm.cs (READ ONLY). Build a small
self-contained playback policy service, not another media player.
Required FR13: Shuffle on/off, Repeat Off/All/One, automatic transition
at item-end. Handle empty playlist, single-item playlist, last item,
shuffle with repeat, and direct Next separately from natural end.
Follow the instructor's Appendix D semantics, and document any
ambiguity rather than silently choosing a conflicting behavior.

Allowed paths ONLY: RepeatMode.cs, optional PlaybackOrder.cs,
tests/ChrisPlaybackTests/**, docs/workstreams/chris/**.
DO NOT edit BasePlayerForm.cs, Form1.cs, Program.cs or csproj.
The existing protected PlayNextOrStop() belongs to Mark; create an
isolated, easily called policy, returning a next index or a stop
decision, plus exact instructions for Mark to hook it into that method.
Shanen owns the shuffle/repeat menus.

Use dependency-injected seeded/deterministic random selection for tests.
Work incrementally, show me each genuine code/test diff and test
result, then ask for my approval before meaningful commits. Use
feat:/fix:/test:/docs:/refactor: messages; commit as me on only my
branch, never main. Explain the decisions so I can defend them.
```

### Suggested 10 substantive checkpoints

1. `RepeatMode` Off/All/One enum and behavior contract — `feat: define repeat modes and playback policy`.
2. Implement normal next-item behavior for shuffle-off — `feat: advance through ordered playlist`.
3. Handle natural end on last item for Repeat Off — `feat: stop playback when repeat is disabled`.
4. Implement Repeat All wrap-around — `feat: loop playlist when repeat all is enabled`.
5. Implement Repeat One replay — `feat: replay current item for repeat one`.
6. Implement shuffle selection for multiple tracks — `feat: select randomized next playlist item`.
7. Handle single-item/empty-list boundaries and avoid invalid indices — `fix: handle empty and one-track playback modes`.
8. Define/test interaction of shuffle and repeat, and direct Next versus natural end — `test: verify shuffle and repeat precedence`.
9. Build deterministic policy tests, including previously failed edge cases — `test: cover playback transitions and edge cases`.
10. Document precise `PlayNextOrStop` integration hook, state transitions and mode acceptance cases — `docs: specify playback policy integration contract`.

**Handoff:** Give Mark exact policy call + expected stop/next result; give Shanen mode labels/commands. Neither needs access to your files to write UI or base hooks.

## Shanen Mirador — P4: original minimal UI/skin/splash

**Branch:** `feat/shanen-ui`  
**Issue:** https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/5  
**Allowlist:** `app/MediaPlayerPrj/Form1.cs`, `Theme.cs`, `SplashForm.cs`, `TeamInfo.cs`, `app/MediaPlayerPrj/Assets/**`, `docs/workstreams/shanen/**`. No edits to base, Program or .csproj. Current `Form1.cs` builds its UI in C# and has **no Designer file**.

### Paste to your AI

```text
Act as my C# WinForms UI engineer and teacher for Amazing Player
issue #5. Repo:
https://github.com/NeatKnight18586/amazing-oop10-media-player
Branch: feat/shanen-ui. Verify it is checked out.

Read AGENTS.md, PRD, INTEGRATION_CONTRACT, PHASE_PLAN, DESIGN_HANDOFF,
TEAM_HANDOFF and issue #5. Inspect the current working Form1.cs:
it builds its controls IN CODE, not Form1.Designer.cs. Preserve the
working VLC VideoView and every playback action. Our look is an
ORIGINAL, MINIMAL PNG media-player skin, standard window border.
Actual art arrives from our design team in app/MediaPlayerPrj/Assets.
Do not invent or claim final skin/portraits; use safe temporary
placeholders until supplied. Keep menu shortcuts visible, status,
controls and text readable at 100%/125% scale. Never cover the
VideoView panel.

Allowed paths ONLY: Form1.cs, Theme.cs, SplashForm.cs, TeamInfo.cs,
app/MediaPlayerPrj/Assets/** and docs/workstreams/shanen/**.
Do not touch BasePlayerForm.cs, Program.cs, csproj, models, sorter,
playback policy or Favorites service.

Implement the layout and splash in safe, individually reviewable
steps, preserving build and playback. A 3-second splash shows app
title, Group 1, BSCS2A, and all five members' names, photos and roles;
it skips on click/key, and a missing picture shows a placeholder.
You may DEFINE buttons/menus for sorting, shuffle/repeat and Favorites,
but DO NOT invent service APIs or claim working connections until
Darrel/Chris/Mitchel provide their contracts. Ask Mark for the
Program.cs splash start and any project asset-copy changes.

Show diff and actual test result before I approve any commit.
Use feat:/fix:/test:/docs:/refactor: messages with my Git identity,
push only to feat/shanen-ui, never main. Explain UI changes clearly.
```

### Suggested 10 substantive checkpoints

1. Create `Theme.cs` for colors, asset file mapping and safe missing-image fallbacks — `feat: centralize minimal player theme settings`.
2. Refine main layout/resizing without changing playback logic — `refactor: improve responsive player panel layout`.
3. Protect native VLC video panel against overlay and redraw issues — `fix: isolate video surface from skin controls`.
4. Improve playlist display, selection focus and metadata readability — `feat: polish playlist and selected-track details`.
5. Improve Now Playing/status/timing visibility — `feat: enhance now playing and elapsed time display`.
6. Style playback buttons and volume/seek visuals using real approved assets — `feat: apply original playback control skin`.
7. Consolidate visible menus/shortcuts and preserve existing handlers — `feat: refine file playback library and help menus`.
8. Implement 3-second skippable `SplashForm`, no app startup edit — `feat: add skippable splash screen component`.
9. Add `TeamInfo` data, five member cards and safe photo/logo fallbacks — `feat: display group details and team on splash`.
10. Integrate original group PNG/logo assets as available, verify 100%/125% scaling, record screenshots/credits/layout handoff — `docs: verify original skin layout and assets`.

**Handoff:** Ask Mark to display SplashForm before main Form1 and update .csproj Copy-to-Output if required. Ask Darrel/Chris/Mitchel for exact service calls before wiring menus/heart/filter, with a follow-up integration PR if needed.

## Mitchel Susi — P5: favorites service and QA

**Branch:** `feat/mitchel-favorites`  
**Issue:** https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/6  
**Allowlist:** `app/MediaPlayerPrj/FavoritesStore.cs`, `FavoritesService.cs`, `tests/MitchelFavoritesTests/**`, `docs/workstreams/mitchel/**`, and **only if explicitly assigned** the global `docs/TEST_PLAN.md`/`docs/BUG_LOG.md`. Do not change `Library.txt`, models, Form1, BasePlayerForm, Program, .csproj.

### Paste to your AI

```text
Act as my C# developer, unit-test writer and teacher for Amazing
Player issue #6. Repo:
https://github.com/NeatKnight18586/amazing-oop10-media-player
Branch: feat/mitchel-favorites. Verify it is checked out.

Read AGENTS.md, PRD, INTEGRATION_CONTRACT, PHASE_PLAN, TEAM_HANDOFF,
TEST_PLAN.md and issue #6. Implement our FR8 enhancement:
Favorites / Liked Songs. Store liked media paths in a SEPARATE
Favorites.txt, never change or corrupt required six-field Library.txt.
Use a small service/store API for add, remove, lookup and selecting
favorite MediaItem entries. Handle duplicate paths, missing file,
corrupt line, empty store and persistence after relaunch; use safe
path normalization and file I/O error messages.

Allowed paths ONLY: FavoritesStore.cs, FavoritesService.cs,
tests/MitchelFavoritesTests/**, docs/workstreams/mitchel/**.
Do not change Form1.cs, BasePlayerForm.cs, Program.cs, csproj,
LibraryStore or another teammate's files. Tell Shanen the exact UI
service calls for the heart and Favorites-only filter.

Also prepare feature tests and contribute honest QA documents. At
least 15 actually executed group tests are required eventually; do
not mark unexecuted cases as pass. Record real defects and retests.

Make one small testable improvement at a time. Show me the diff and
actual test results before I approve each meaningful feat:/fix:/
test:/docs:/refactor: commit using my Git identity. Push only to
feat/mitchel-favorites; never main. Explain so I can present the code.
```

### Suggested 10 substantive checkpoints

1. Define favorite storage and normalized path-key contract — `feat: define favorites persistence contract`.
2. Implement safe `FavoritesStore.Load` for missing/empty file — `feat: load existing favorite media paths`.
3. Parse/skip malformed and duplicate favorites records — `fix: recover from invalid favorite entries`.
4. Implement safe UTF-8 `FavoritesStore.Save` with appropriate errors — `feat: save favorite media paths to text file`.
5. Implement `FavoritesService.Add` and `Remove` operations — `feat: add and remove favorite tracks`.
6. Expose `IsFavorite` status and a stable favorites-only query — `feat: query liked tracks from the library`.
7. Cover path comparisons, deleted media and duplicate entries reliably — `fix: normalize and deduplicate favorite paths`.
8. Add service tests for toggle/add/remove/filter — `test: cover favorites service operations`.
9. Add storage tests for round trips, missing files, corrupt content and errors — `test: cover favorites persistence and recovery`.
10. Prepare/review precise QA test cases, issue integration handoff and record only genuine observations/bugs — `docs: document favorites verification and QA findings`.

**Handoff:** Give Shanen exact `IsFavorite`/`Toggle`/favorites-filter signatures and Mark the save-location/exception strategy; app UI wiring is outside Mitchel's scope.

## What Mark (lead) does while they work

- **Immediately:** change GitHub repository **PUBLIC → PRIVATE** (Settings → General → Danger Zone) before sharing photos or personal materials; this is mandatory per instructor.
- Invite four teammates and instructor; send each one their issue URL, branch name and this file.
- Keep ownership of `BasePlayerForm.cs`, `Program.cs`, `.csproj`, automation/CI and merge decisions. Do not let four agents edit these shared files at once.
- As APIs are delivered, wire them sequentially: library storage/sort (Darrel), playback modes (Chris), Favorites (Mitchel), UI/skin (Shanen). Shanen can independently develop early layout and splash while waiting on service calls.
- Verify CI on PRs; run combined app in Visual Studio on your Windows PC. Record real regression results, tests, bug log, and screenshots.
- Course requires **at least 10 real meaningful commits by Mark as well**. They should come from actual integration, metadata resolution, tests, corrections, docs and release work—never artificial content.

## Instructor timing

M1–M3 due **Nov 10, 2026**, M4 Nov 12, M5 Nov 14. Total 5 members. Each must be able to explain any part of project code and the team's use of libraries; record independent real work in Git history.
