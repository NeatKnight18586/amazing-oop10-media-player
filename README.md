# Amazing Player
**OOP10 Final Project — BSCS2A, Group 1: Amazing**

A Windows Forms application that imports local MP3 and MP4 files, maintains a media library, and plays them using LibVLCSharp. The interface uses a cassette-inspired design with an editable Windows Forms layout.

**Members:** Darrel Cango, Chris Garcia, Mark Guevarra, Shanen Mirador, Mitchel Susi.

## Project status

The November 10 M1–M3 implementation is under review in [PR #11](https://github.com/NeatKnight18586/amazing-oop10-media-player/pull/11). It builds and publishes on GitHub's Windows runner. The automated suites contain **163 passing tests** (library: 6, playback policy: 88, Favorites: 69). A Windows desktop visual check of the new Designer layout and integrated media controls is still required before final approval.

| Milestone | Work |
|---|---|
| M1 — Analysis and design | Requirements, use-case/class/sequence diagrams, cassette-inspired design concept |
| M2 — Core logic | MediaItem hierarchy, LibraryStore text persistence, exception handling, file import and unit tests |
| M3 — Player and skin | Form1 inherited from BasePlayerForm, designer-backed controls, original cassette assets, splash screen, menus, elapsed clock, insertion sort, shuffle/repeat and Favorites |

## Open in Visual Studio

Open `AmazingPlayer.sln` or `app/MediaPlayerPrj/MediaPlayerPrj.csproj` using Visual Studio 2022/2026 with **.NET desktop development** installed. Restore packages if prompted, then press F5. The target is .NET 8 Windows x64.

To adjust the interface graphically, right-click `Form1.cs` or `SplashForm.cs` in Solution Explorer and choose **View Designer**. Their controls are defined in matching `.Designer.cs` and `.resx` files; playback logic stays in separate class files. Skin graphics are loaded from `app/MediaPlayerPrj/Assets/`.

The repository follows the professor's posted MediaPlayerPrj example and the final OOP10 requirement guide. The file format for new library records follows the final guide:
`Type|Title|Artist|HH:MM:SS|FilePath|AlbumOrResolution`.

## Main features

- Add MP3 and MP4 files using a file dialog; view metadata and the playlist.
- Play, Pause, Stop, Next, Previous, volume and seek.
- Automatically save and reload `Library.txt`; skip damaged records.
- Search the playlist; hand-written insertion sort by title and artist.
- Shuffle and Repeat Off/All/One with automatic media-end behavior.
- Mark tracks as Favorites and filter the library by liked songs.
- Splash screen with group information and member-photo placeholders.
- Retro cassette skin with separate playlist, menus, clock and video surface.

## Source organization

| Directory / file | Purpose |
|---|---|
| `app/MediaPlayerPrj/Form1.Designer.cs` | Editable main-window controls and positions |
| `app/MediaPlayerPrj/SplashForm.Designer.cs` | Editable splash layout |
| `app/MediaPlayerPrj/BasePlayerForm.cs` | LibVLC playback functionality |
| `app/MediaPlayerPrj/LibraryStore.cs` | Read/write local library |
| `app/MediaPlayerPrj/LibrarySorter.cs` | Insertion sort |
| `app/MediaPlayerPrj/PlaybackOrder.cs` | Shuffle/repeat decisions |
| `app/MediaPlayerPrj/FavoritesService.cs` | Favorites logic |
| `app/MediaPlayerPrj/Assets/` | Application artwork and team photos |
| `tests/` | Automated unit tests |
| `docs/milestones/M1/` | Requirements and UML/design material |

## Build and tests

```powershell
dotnet build AmazingPlayer.sln -c Release
dotnet test tests/LibraryTests/LibraryTests.csproj -c Release
dotnet test tests/PlaybackModes.Tests/PlaybackModes.Tests.csproj -c Release
dotnet test tests/MitchelFavoritesTests/MitchelFavoritesTests.csproj -c Release
```

CI runs these tests and publishes a self-contained win-x64 ZIP. The executable and native VLC libraries must be distributed together. See [deployment checklist](docs/DEPLOYMENT_CHECKLIST.md).

## Required final checks

Open both forms in Visual Studio Designer, test MP3 and MP4 playback, verify persistent Library/Favorites across restart, inspect sizing at 100% and 125%, and capture screenshots. Replace missing member portraits with the actual photos (with permission), retain the editable original artwork source and credits, and test the final ZIP on a second Windows PC.

The course requires a **private** GitHub repository, instructor/member access, genuine individual commits, milestone logs and the final PDF portfolio.
