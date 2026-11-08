# Team handoff — Amazing Player, Group 1 / BSCS2A

**Lead:** Mark Guevarra. **Common repo:** https://github.com/NeatKnight18586/amazing-oop10-media-player  
**Status:** The reconstructed C# WinForms/VLC starter is merged into `main`, CI builds and publishes on Windows, and Mark reports that the basic player works on a Windows PC. **The mandatory course features are not all finished.**

**Before anyone starts:** Mark must switch the repository to **Private** (instructor requirement), invite members, and provide the issue/branch link. Antigravity users should always open the actual code already present, not ask AI to build a new media player from scratch.

## Darrel — Media library / OOP / insertion sorting

- Branch: `feat/darrel-library`; [Issue #3](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/3)
- Primary ownership: existing MediaItem.cs, AudioItem.cs, VideoItem.cs; new LibraryStore.cs, LibrarySorter.cs, focused tests.
- **Deliver:** load/save the exact required `Type|Title|Artist|HH:MM:SS|FilePath|AlbumOrResolution` format, missing/corrupt rows handled with skipped count, hand-written insertion sort by title and artist, brief trace and complexity.
- **Do not edit:** Form1.cs, BasePlayerForm.cs, Program.cs, MediaPlayerPrj.csproj.
- **Ask Mark/Shanen to wire:** startup library load, saving after import, column sort and preserving now-playing item.

Prompt addition for Antigravity:
```text
My task is GitHub Issue #3 on feat/darrel-library.
The existing AudioItem/VideoItem/MediaItem classes already compile.
Extend them minimally; add LibraryStore and LibrarySorter using the
final PDF's six-field format and handwritten insertion sort for Title
and Artist. Write focused tests and a small sort trace. Do not touch
Form1, BasePlayerForm, Program or the main project file.
Show the first small proposed change and test before proceeding.
```

## Chris — Shuffle and Repeat

- Branch: `feat/chris-playback-modes`; [Issue #4](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/4)
- Ownership: RepeatMode.cs and optionally a pure PlaybackOrder.cs + focused tests.
- **Deliver:** Repeat Off/All/One, shuffle on/off and correct natural end-of-media transitions. Test 0/1/many items, last item, shuffle/repeat interaction. Make logic independently testable.
- **Do not edit:** BasePlayerForm.cs (Mark owns `PlayNextOrStop()`) or Form1.cs (Shanen owns mode UI).
- **Ask Mark/Shanen to wire:** behavior into VLC end event and menu items.

Prompt addition:
```text
My task is Issue #4 on feat/chris-playback-modes.
Read BasePlayerForm.PlayNextOrStop but DO NOT edit BasePlayerForm.
Implement a small testable RepeatMode and playback-order decision
helper; include off/all/one, shuffle and one-item/last-item edge cases.
Use deterministic tests. Provide the exact hook request Mark must
make and the UI states Shanen must expose. No unrelated refactors.
```

## Shanen — Original minimal skin and UI

- Branch: `feat/shanen-ui`; [Issue #5](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/5)
- Ownership: current **programmatic** Form1.cs (not a Designer file), Theme.cs, SplashForm.cs, TeamInfo.cs, project Assets/ and design handoff.
- **Deliver:** minimal original PNG skin, readable layout, video panel unobstructed, visible File/Playback/Library/Help menus and shortcuts, playlist + Now Playing, clock, favorites heart/filter, mode controls, 5-person ~3s skippable splash with missing-photo placeholders.
- **Needs from design team:** original PNG, logo PNG and .ico, 5 consented portraits, Canva link/PSD and credits. The UI can begin as layout work while assets are being produced.
- **Do not edit:** BasePlayerForm.cs / Program.cs; ask Mark to wire splash startup and shared playback hooks.
- **Coordinate:** connect Darrel's sorting/storage, Chris's modes and Mitchel's Favorites only when their APIs are ready.

Prompt addition:
```text
My task is Issue #5 on feat/shanen-ui.
Form1 currently builds a temporary interface in code; there is NO
Form1.Designer.cs. Preserve its working VLC VideoView and playback.
Create the original, minimal layout in a small safe step. Read
docs/DESIGN_HANDOFF.md. Do not invent the skin PNG or photos:
use appropriately labeled placeholders until our team supplies them.
Keep the VideoView unobscured and all required controls visible.
Do not edit BasePlayerForm, Program or project package settings.
```

## Mitchel — Favorites and QA

- Branch: `feat/mitchel-favorites`; [Issue #6](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/6)
- Ownership: FavoritesStore.cs, FavoritesService.cs, service-focused tests and QA/portfolio documents.
- **Deliver:** favorite/unfavorite by normalized media path, unique favorites, persistent text file **separate** from required Library.txt, safe missing/corrupt file behavior; QA plan/results recorded honestly.
- **Do not edit:** Form1.cs, BasePlayerForm.cs, Program.cs.
- **Ask Shanen to wire:** heart button, selected/favorite state, favorites-only filter.
- **Testing:** plan at least 15 cases; record each real executed case, bug and retest. Do not mark unexecuted scenarios as passes.

Prompt addition:
```text
My task is Issue #6 on feat/mitchel-favorites.
Implement FavoritesStore and FavoritesService for an independent
Favorites.txt persistence file; NEVER change the six-field
Library.txt format. Include duplicate/missing/corrupt entries tests.
Do not modify Form1 or BasePlayerForm. Specify the service methods
that Shanen will call for a heart button and favorites filter.
Keep the test plan truthful: not-run is not passed.
```

## Mark — Integration and release

- Own BasePlayerForm.cs, Program.cs, .csproj, CI, shared playback hooks, final Windows tests and PR reviews.
- Integrate APIs **after** each teammate's PR is reviewed. Keep passing current features intact. Implement/verify actual MP4 resolution instead of the bootstrap's `Unknown`.
- Request a Windows check after each integration. Finish real test log, original skin and asset credits, UML diagrams, manual, different-PC ZIP verification and ten-minute team demo.
- Ensure **at least 10 meaningful commits per person**, with authentic contributor identities; do not create forced sequences simply to fill a quota.

## Instructor dates
M1–M3 Nov 10, M4 Nov 12, M5 Nov 14, 2026. Plan for early integration because deadlines are close.
