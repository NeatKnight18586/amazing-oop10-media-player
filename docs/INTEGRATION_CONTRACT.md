# API and integration contract — must verify against instructor project

**Status: provisional / unverified.** Full starter source is missing. Do not generate assumptions as fixed method signatures.

## Contracts visible in instructor guide
- `IMediaControls`: Play(), Pause(), Stop(), Next(), Previous().
- `MediaItem`: Title, Artist, Duration, FilePath; polymorphic `MediaType`, `Extra`, `ToFileLine()`.
- `AudioItem : MediaItem`: Album; `VideoItem : MediaItem`: Resolution.
- `BasePlayerForm : Form, IMediaControls`: concrete, parameterless constructor and VLC `VideoView`; protected Playlist and CurrentIndex shown. Virtual hooks `OnNowPlayingChanged(MediaItem)`, `ApplyTheme()`, `SetStatus(string)`, plus playback overrides.
- `LibraryStore.Save(path, List<MediaItem>)` and `Load(path, out skippedLines)`.
- `LibrarySorter.InsertionSort(List<MediaItem>, SortKey)` for Title/Artist.
- `RepeatMode`: Off, All, One.
- Derived group form: >=2 meaningful overrides + >=1 new member. MenuStrip delegates to existing button/logic methods.

## Validation needed before individual implementation
- [ ] Exact actual solution path and target framework:
- [ ] Full starter ZIP imported into app/ without replacement:
- [ ] Real namespaces and type names:
- [ ] LibVLCSharp, LibVLCSharp.WinForms, VideoLAN.LibVLC.Windows package versions:
- [ ] Public/protected API of BasePlayerForm and existing designer hooks:
- [ ] `SetPlaylist`, `ReadMediaItemAsync`, `RefreshListView`, `DurationText` exist or not:
- [ ] Library path and write permissions:
- [ ] `dotnet restore`: NOT RUN
- [ ] `dotnet build`: NOT RUN
- [ ] Designer opens: NOT RUN
- [ ] MP3/MP4 VLC playback: NOT RUN

## Planned component handshake
Darrel returns `List<MediaItem>` with safe parsing and sorting; Chris provides next-on-ended decision/policy without touching BasePlayerForm; Mitchel provides a favorite check/toggle/store API without changing Library.txt; Shanen's derived form invokes approved APIs and updates playlist, status, clock, controls and splash. Mark is responsible for adding shared hooks in the base/Program/project and integrating components, then verifying the real Windows build.

Record any API change in this file before asking another contributor to depend on it.
