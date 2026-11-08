# Source reconciliation: instructor's direct sample vs final guide

## Verified sources
1. **Original instructor-provided 21-page guide** (OOP10): pages 8–20 use Appendix A–D starter fragments; Appendix A says listings are abridged and the full project files are separate.
2. **Professor's posted “Sample Codes for MediaPlayerPrj”** supplied by group in conversation, dated no later than Nov 8. This contains Form1 : Form and key VLC controls.

The posted sample uses `Form1 : Form` with a designer-generated `InitializeComponent()`, `panelVideo`, `listView1`, and buttons. The entire AddFilesToLibrary/ReadInfo/Folders/Notepad implementation and deferred disposal are in block comments. The actual Designer and .csproj were not supplied.

## Important conflicts and resolution
| Issue | Direct sample | Final project PDF | Decision |
|---|---|---|---|
| OOP form | Form1 : Form | BasePlayerForm : Form, IMediaControls and group form inherits | Preserve sample's VLC ideas but refactor into BasePlayerForm + derived Form1 |
| File format | Header + Title,Artist,Album,Duration,Type,FilePath | Type,Title,Artist,HH:MM:SS,FilePath,AlbumOrResolution | Follow PDF as final deliverable; Darrel may add *read-only legacy import* with explicit tests |
| Media model | string[] rows | List<MediaItem>, abstract class and Audio/Video inheritance | Implement OO types, not string[] as persisted internal model |
| UI | Designer references without Designer file | Original PNG skin inside project Assets | Temporary programmatic UI to permit build; Shanen replaces and tests actual design |
| Auto next/shuffle | Absent | FR13 mandatory | Chris supplies policy, Mark wires into base |
| Favorites | Absent | FR8 new separate enhancement | Mitchel implements persisted favorites distinct from Library.txt |
| Video metadata | Sample Album column only | MP4 resolution required | Track metadata resolution pending P2 |

## Bootstrap limitation
`bootstrap/instructor-sample` contains a **reconstructed starter**, not a byte-for-byte copy of an instructor Visual Studio ZIP. Native playback and WinForms UI cannot be manually tested here. GitHub Windows CI compile is the first gate; a real Windows PC still needs to run media and designer tests.

The initial code only targets add-file, basic playback, metadata, basic menu and elapsed display. Library persistence, sorting, shuffle/repeat modes, Favorites, splash, original assets and portfolio are intentionally unimplemented team workstreams.

## Package choices
- Target `net8.0-windows`, following instructor Appendix E.
- LibVLCSharp + LibVLCSharp.WinForms `3.10.2`, VideoLAN.LibVLC.Windows `3.0.24`, chosen by current NuGet package listings on Nov 8 2026. Versions may be adjusted if Windows restore/build demonstrates incompatibility.
