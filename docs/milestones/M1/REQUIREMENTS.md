# Amazing Player — Requirements and scope (M1)

**BSCS2A · Group 1 · Amazing**

**Problem:** Playing and organizing locally stored MP3/MP4 files requires opening files individually. Amazing Player combines a persistent local library, player controls, sorting and favorite songs.

**Users:** Windows users with local MP3/MP4 files. No Internet, streaming, downloads, conversion, or account system is planned.

| ID | Priority | Acceptance requirement |
|---|---|---|
| FR1 | Must | Import valid MP3/MP4 using an OpenFileDialog |
| FR2 | Must | Display title, artist, duration, path, type, album for audio or resolution for video |
| FR3 | Must | Save/reload six-field Library.txt, including startup load |
| FR4 | Must | Play, Pause, Stop, Next, Previous and Volume or Seek |
| FR5 | Must | Show playlist and Now Playing details |
| FR6 | Should | Search/filter plus mandatory sorting via FR12 |
| FR7 | Must | Recover from empty playlist, deleted file, corrupt row and unsupported media |
| FR8 | Should | Favorites: mark/unmark, persist, and filter liked songs |
| FR9 | Must | Three-second or skippable splash with logo, names, roles, section and group |
| FR10 | Must | Visible File, Playback, Library and Help MenuStrip with shortcuts |
| FR11 | Must | Elapsed/total clock updated at least once a second |
| FR12 | Must | Handwritten insertion sort by title/artist; preserve selected track |
| FR13 | Must | Shuffle on/off, Repeat Off/All/One, automatic media-ended continuation |

**OOP:** IMediaControls implemented by BasePlayerForm; abstract MediaItem with AudioItem/VideoItem; Form1 derived from BasePlayerForm with meaningful overrides; List<MediaItem> and property validation; separate LibraryStore, LibrarySorter, Theme, playback and Favorites logic.

**Non-functional:** recover gracefully from errors; original readable skin with video unobstructed, normal Windows border and 100%/125% scaling; assets copied into build; private GitHub with genuine feature commits and review; Windows x64 distributable ZIP tested on another PC.

**Chosen enhancement:** Favorites/Liked Songs. **Chosen algorithm:** insertion sort.

**Storage:** Type|Title|Artist|HH:MM:SS|FilePath|AlbumOrResolution (final guide). The professor's earlier sample had a different header; the application loader can import that older format for compatibility.