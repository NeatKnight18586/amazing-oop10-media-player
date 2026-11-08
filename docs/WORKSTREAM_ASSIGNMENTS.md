# Team responsibilities — feature ownership, not a commit script

Each contributor owns actual outcomes and publishes reviewable changes under their own GitHub identity. Instructor requires at least 10 meaningful commits **per member**, but there is no prescribed series of ten superficial updates. Keep history organic and truthful.

| Member | Responsibility | Primary files to own after real starter import |
|---|---|---|
| Guevarra, Mark | Lead: baseline starter, BasePlayer/VLC integration, shared API decisions, merge review, release | BasePlayerForm.cs, Program.cs, .sln/.csproj, integration/deployment |
| Cango, Darrel | OOP hierarchy, validation, library file I/O and insertion sort | MediaItem.cs, AudioItem.cs, VideoItem.cs, LibraryStore.cs, LibrarySorter.cs, tests |
| Garcia, Chris | Shuffle/repeat behavioral policies and boundary-case verification | RepeatMode.cs, PlaybackOrder.cs (if needed), playback-policy tests |
| Mirador, Shanen | Minimal original WinForms skin, derived form, MenuStrip, splash and visuals | Group form/Designer/.resx, Theme.cs, SplashForm.cs, TeamInfo.cs, Assets/ |
| Susi, Mitchel | Favorites persistence/service, QA and portfolio evidence | FavoritesStore.cs, FavoritesService.cs, tests, test/bug/portfolio docs |

### Boundaries
Mark alone changes shared VLC base/Program/project files unless explicitly delegating; Shanen alone changes derived form and designer; Darrel owns models/store/sorter; Chris owns isolated playback policy; Mitchel owns Favorites and QA. If an API hook in another person's file is required, submit a short integration request rather than editing that file without permission.

### Genuine progress and review
Open one issue per bounded feature with required behavior, file allowlist and acceptance criteria. Push meaningful commits while building/test-fixing/reviewing, not to hit a predetermined count. PRs must list FR IDs, changed files, actual test results, known risks and integration points. Save real collaboration/meeting evidence for portfolio. Do not commit on behalf of another person.
