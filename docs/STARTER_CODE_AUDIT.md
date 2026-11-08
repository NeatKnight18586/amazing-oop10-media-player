# Starter source audit — pending instructor project

The 21-page requirement guide's Appendix A explicitly says source snippets are **abridged** and the instructor provides complete starter files separately. Do not treat omitted /* ... */ segments as working LibVLCSharp code.

## Visible source reference
| Guide page | Source/feature | Note |
|---|---|---|
| 8 | IMediaControls, abstract MediaItem, AudioItem, VideoItem | Shared hierarchy; each class its own file |
| 9 | LibraryStore | ParseLine is a student TODO; skip/count invalid lines |
| 9–10 | BasePlayerForm | VLC engine, playback, metadata parsing are abbreviated; real source required |
| 10–11 | Theme, group-derived form | Theme helper methods omitted; real form must override 2+ meaningful members |
| 12 | Sample Library.txt/test/bug/role formats | Preserve six fields |
| 13–15 | TeamInfo, SplashForm, Program | Startup splash and five member cards |
| 15–17 | MenuStrip, shortcuts, timer | DurationText and other references need confirmation |
| 18–19 | LibrarySorter and SetPlaylist | Use only hand-written insertion sort for this group |
| 19–20 | Shuffle/Repeat | End-of-media callback must run safely on WinForms UI thread |
| 21 | App icon + publishing | x64, native VLC plugins, assets, test on a different PC |

## Known integration hazards
- Example namespace varies between `MediaPlayerTemplate` and `mediaPlayerPrj`; preserve actual starter namespaces.
- Appendix C mentions `MediaItem.DurationText` not shown in Appendix A.
- Appendix D calls `SetPlaylist`, `RefreshListView` and `SaveLibraryToFile`, which must be checked in actual code.
- Metadata can contain `|`, the six-field separator; choose a safe documented compatibility policy.
- A VLC media-ended event may occur on a background thread; marshal any UI call to WinForms main thread.
- The screenshot/designer layout cannot be validated from PDF text.

## Baseline audit
- [ ] Actual starter ZIP received
- [ ] Original project copied into app/ without restructuring
- [ ] .sln/.csproj examined
- [ ] VLC packages and target framework recorded
- [ ] Real WinForms Designer opens
- [ ] MP3 and MP4 playback tested
- [ ] Baseline warnings/errors recorded

Every unchecked item is NOT VERIFIED.
