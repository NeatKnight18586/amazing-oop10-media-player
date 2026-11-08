# Integration contract — verified against checked-in bootstrap

**Baseline:** PR #8 merged Nov 8, 2026. Contract below is grounded in actual repository source, NOT a missing instructor ZIP. Source was reconstructed from the professor's pasted Form1 sample + the final course PDF. Do not replace it with fresh generated code.

## Project / environment
- Project: `app/MediaPlayerPrj/MediaPlayerPrj.csproj`
- Framework: `net8.0-windows`, WinForms, `PlatformTarget=x64`, namespace `mediaPlayerPrj`.
- NuGet package versions from `.csproj`: `LibVLCSharp 3.10.2`, `LibVLCSharp.WinForms 3.10.2`, `VideoLAN.LibVLC.Windows 3.0.24`.
- Entry: `Program.Main()` → `Application.Run(new Form1())`.
- `Form1` is a **code-built temporary UI**, not a VS Designer form. There is no `Form1.Designer.cs` in the checked-in bootstrap; Shanen should style or intentionally redesign this without assuming `InitializeComponent()`.
- WinForms CI restore/build and self-contained publish: **PASSED** on GitHub Actions. Mark reported successful MP3/MP4 playback and basic controls on Windows; formal test evidence still pending.

## Existing types and signatures (verified in source)
- `IMediaControls` declares `void Play(), Pause(), Stop(), Next(), Previous()`.
- `MediaItem` abstract: `Title` (validated), `Artist`, `Duration: TimeSpan`, `FilePath`; `MediaType` / `Extra`, `DurationText`, `ToFileLine()`.
- `AudioItem : MediaItem` has `Album`; `VideoItem : MediaItem` has `Resolution`.
- `BasePlayerForm : Form, IMediaControls` has a concrete parameterless constructor and VLC player/video surface.
- Protected: `VideoSurface`, `Playlist: List<MediaItem>`, `CurrentIndex`, `AttachVideo(Control)`, `SetPlaylist(List<MediaItem>)`, `ElapsedTime: TimeSpan`, `ReadMediaItemAsync(string): Task<MediaItem>`.
- Public virtual: `Play()`, `Pause()`, `Stop()`, `Next()`, `Previous()`, `SetVolume(int)`, `Seek(float)`.
- Protected virtual: `OnNowPlayingChanged(MediaItem)`, `ApplyTheme()`, `SetStatus(string)`, `PlayNextOrStop()`. Media-ended VLC event is marshaled to WinForms UI thread before `PlayNextOrStop()`.
- `Form1 : BasePlayerForm` overrides `OnNowPlayingChanged`, `SetStatus`, `ApplyTheme`. Current private UI helpers include `BuildMenu()`, `RebuildList()`, `PlaySelected()`, `AddFromDialogAsync()`, `UpdateElapsed()`.

## NOT YET PRESENT / important gaps
- `LibraryStore` and `LibrarySorter` do not exist yet; Darrel implements them, following PDF Appendix B six-field format `Type|Title|Artist|HH:MM:SS|FilePath|AlbumOrResolution` and the PDF's specified `Save`/`Load` signatures.
- `Form1` currently stores imported files **in memory only**; library persistence and sorting UI require a narrow interface integration by Shanen/Mark after Darrel's PR. This is **not** the professor's older header + `Title|Artist|Album|Duration|Type|FilePath` format.
- The current `ReadMediaItemAsync` returns `VideoItem.Resolution = "Unknown"`; actual MP4 resolution extraction remains required for FR2, coordinated with Mark.
- `RepeatMode`, shuffle/repeat engine, Favorites service/storage, skin assets, splash and about team are not implemented.
- `Playlist` is a mutable protected list; do not replace its structure without lead approval. `SetPlaylist` preserves selection by path; sorting a **copy** is required.
- `Form1` and `BasePlayerForm` are shared integration points. Darrel/Chris/Mitchel should not modify them directly; send exact methods/hooks needed to Mark/Shanen.

## Integration handshake / ownership
1. Darrel: `List<MediaItem>` storage/parsing, insertion sorter with `SortKey.Title/Artist`. Return pure service APIs and tests; Shanen/Mark wires UI.
2. Chris: `RepeatMode` and optional pure playback order strategy; Mark wires `PlayNextOrStop()` and mode state in base; Shanen owns mode UI.
3. Mitchel: `FavoritesStore`/`FavoritesService` API keyed by normalized local file path, separate `Favorites.txt`. Shanen adds heart/filter UI; Mark resolves any shared hook.
4. Shanen: `Form1` UI, `Theme`, `SplashForm`, `TeamInfo`, `Assets`. Request changes to `Program.cs` for startup splash from Mark.

## Test status
- CI restore/build: PASSED (GitHub run 42486131178). Windows win-x64 self-contained publish: PASSED.
- Windows manual smoke test: **user-reported PASS**, not independently observed; date Nov 8, 2026.
- 15+ detailed tests, release test on **second Windows computer**, full demo: **NOT RUN / NOT COMPLETE**.

Before changing any cross-owner API, update this contract first and request integration review.
