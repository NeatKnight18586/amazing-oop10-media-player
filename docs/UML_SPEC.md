# UML design spec — to render and verify against actual source

These are **proposed** diagrams; do not pass off as finalized diagrams until checked against the actual instructor starter. Use draw.io, StarUML or another UML tool; export images for the PDF portfolio.

## Use case diagram
**Actor:** User. **Core use cases:** Add MP3/MP4, View Library/metadata, Play/Pause/Stop, Next/Previous, Change Volume, Save Library, Load Library on Startup, Sort by Title/Artist, Search (optional), Toggle Shuffle, Cycle Repeat, Favorite/Unfavorite, Filter Favorites. Include a short plain-English description, precondition and success/failure outcome for each main use case.

## Class diagram relationships
- `IMediaControls` declares five playback operations; `BasePlayerForm` implements it.
- Abstract `MediaItem` provides shared properties; `AudioItem` and `VideoItem` inherit and provide `Extra`/media-type behavior.
- `BasePlayerForm` has a `List<MediaItem>`, LibVLC player and a hosted `VideoView`.
- `AmazingPlayerForm` (or actual derived form) inherits `BasePlayerForm`; overrides at least two meaningful virtual members.
- `LibraryStore` saves/loads `List<MediaItem>`; `LibrarySorter` orders a copy without replacing playing-item identity.
- `FavoritesService` associates favorites with media paths, persisting via `FavoritesStore`.
- `Theme` loads/chooses skin visuals; `TeamInfo` supplies `SplashForm`.

## Sequence diagram (minimum)
**User presses Play:** User → Derived Form button handler → inherited BasePlayerForm.Play → validate current playlist/path → LibVLC MediaPlayer → media started → OnNowPlayingChanged override → update Now Playing/clock/status. Add alternate branch for empty library or missing file. Record exact method names after baseline review.

## Additional technical explanation
Insertion sort comparison uses Title or Artist case-insensitively. On sorted list best case `O(n)`, worst case `O(n²)`; stable when code shifts only on strictly greater-than comparison. Trace at least three inputs in portfolio. Preserve selected item by identity/path when applying sorted copy.
