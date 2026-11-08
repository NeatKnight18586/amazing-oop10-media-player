# Product Requirements Document — Amazing Player

**Group:** Amazing / Group 1 / BSCS2A  
**Team:** Darrel Cango, Chris Garcia, Mark Guevarra, Shanen Mirador, Mitchel Susi  
**Source:** instructor's 21-page OOP10 Media Player Application Project Guide, §§1–8 and Appendices A–E.  
**Status:** Approved scope, implementation blocked on complete instructor starter source. This is a plan, not evidence of completed work.

## Goal and exclusions
Create a Winamp-like desktop player for locally selected MP3/MP4 files using C# Windows Forms, LibVLCSharp and the instructor's BasePlayerForm. It shows media metadata, persists the local library, plays files, demonstrates OOP, handles errors and uses an original group-designed skin. No streaming, downloading, cloud library, conversion or database.

## Functional acceptance criteria
| ID | Requirement | Priority |
|---|---|---|
| FR1 | Open file dialog; add valid MP3 and MP4 files | Must |
| FR2 | Display title, artist, duration, file path, media type and audio album / video resolution | Must |
| FR3 | Save/load six-field Library.txt; reload when app starts | Must |
| FR4 | Play, Pause, Stop, Next, Previous plus volume or seek | Must |
| FR5 | Show playlist and current Now Playing details | Must |
| FR6 | Search or sort by title/artist; sorting becomes Must via FR12, searching remains Should | Should |
| FR7 | Recover without crashing from missing file, corrupted library lines, unsupported media and empty playlist | Must |
| FR8 | Group enhancement: Favorite/Unfavorite a track, visibly identify favorites, filter/show favorites and retain likes on restart | Should |
| FR9 | Splash screen ~3 seconds (or skip click/key): logo, title, group no., section, every member's photo/name/role; missing-photo fallback | Must |
| FR10 | Visible MenuStrip: File, Playback, Library, Help, keyboard shortcuts, reused button logic | Must |
| FR11 | Elapsed time / total duration, updated at least every second | Must |
| FR12 | Hand-written insertion sort by title and artist; no List.Sort/OrderBy; refresh list and retain current item | Must |
| FR13 | Shuffle on/off; Repeat Off/All/One; automatic next on media-end, including 1-item and last-item cases | Must |

FR8 is **in addition** to FR13: shuffle and repeat are not the optional enhancement.

## OOP and file architecture
- `IMediaControls` has Play, Pause, Stop, Next, Previous.
- `MediaItem` abstract base with validated Title, Artist, Duration, FilePath, polymorphic MediaType and Extra; `AudioItem` adds Album, `VideoItem` adds Resolution.
- Playlist is `List<MediaItem>`; use base type and overrides instead of large media-type branching.
- The instructor's **concrete, parameterless** `BasePlayerForm : Form, IMediaControls` supplies VLC playback; derived group form must override at least **two meaningful virtual members** and add at least **one unique member**.
- `LibraryStore` manages plain-text file I/O; `LibrarySorter` only uses hand-written insertion sort; `Theme.cs` contains skin settings.
- Favorites stored separately (proposed `Favorites.txt` keyed by normalized paths), so `Library.txt` remains compatible.
- Never substitute WPF/web framework for WinForms or COM controls for LibVLCSharp.

## Storage contract
Required record layout: `Type|Title|Artist|HH:MM:SS|FilePath|AlbumOrResolution`. Load corrupt rows defensively, count skipped rows, treat missing Library.txt as empty. Warn on unreadable or missing audio files. Settle a safe handling rule for pipe characters in metadata once real starter parsing is reviewed.

## Skin + usability
Original group-designed minimalist background PNG in real project Assets/; PNG and edit source (Canva-view link or PSD), logo.png and multi-resolution logo.ico, team photos in Assets/Team, credits/licenses, screenshots of running UI. Normal border by default. Keep MenuStrip visible and video area unobscured. Test 100% and 125% Windows display scaling. Set each image to Copy to Output Directory.

## Reliability and deployment
Wrap media/file operations in try/catch with visible error feedback. No empty catches. 500ms WinForms timer permitted for elapsed time; stop on disposal. Final Release x64 self-contained publish targeting instructor-recommended net8.0-windows **if compatible with actual starter**, with LibVLC native runtime/plug-ins, no trimming or single-file. Test ZIP on second Windows PC.

## Completion gate
All Must features work in a real Windows build; at least 15 **executed** test cases with results including negatives; UML use case/class/sequence diagrams; trace, best/worst complexity of insertion sort; source/design credits; portfolio PDF; release ZIP verified on another PC; recorded demo and each member ready to explain code. Track real per-person contributions, no invented test or commit evidence.

## Priority order
P0 instructor starter → models/file I/O → VLC playback → mandatory UI/playback/sort/modes → Favorites enhancement → end-to-end tests/publishing → portfolio/presentation. Do not sacrifice required functionality for decorative polish.
