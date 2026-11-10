# November 10 — M2/M3 test evidence

The following test runs were executed by GitHub Actions on a Windows runner for the integrated player branch.

| Test suite | Passed | Failed |
|---|---:|---:|
| Library persistence and insertion sorting | 6 | 0 |
| Shuffle/repeat playback policy | 88 | 0 |
| Favorites persistence and service | 69 | 0 |
| **Total** | **163** | **0** |

Reference: [Windows CI run 42829509709](https://github.com/NeatKnight18586/amazing-oop10-media-player/actions/runs/42829509709), and more recent builds on PR #11 after art and project-file updates.

**What these tests establish:** compiling and unit tests for file I/O, algorithm behavior, playback index decisions and Favorites. They do **not** establish correctness of live VLC playback, accessibility, Windows Designer loading, original graphic fidelity, or the photo cards.

## Manual tests still to perform

| ID | Expected result | Evidence to record |
|---|---|---|
| M3-01 | Both forms open under View Designer and controls can be moved | Screenshot of Visual Studio Designer |
| M3-02 | Splash displays title/group/roles; auto closes after ~3 seconds or skips | Screenshot and timer observation |
| M3-03 | MP3 plays with sound and correct title/clock | MP3 filename, observed result, screenshot |
| M3-04 | MP4 plays in video panel; playlist remains visible | MP4 filename, observed result, screenshot |
| M3-05 | Library.txt persists and reloads across restart | Before/after list and file lines |
| M3-06 | Sort by Title and Artist works without changing current song | Three-track sample + result |
| M3-07 | Repeat Off/All/One and shuffle work in VLC | Actual last-track/one-track behavior |
| M3-08 | Favorites survive restart and filter correctly | Before/after screenshot |
| M3-09 | UI remains readable at 100% and 125% scaling | Windows scaling screenshot |
| M3-10 | Release ZIP opens on another Windows computer | Machine information + test result |

Do not mark these as PASS until run and observed. For any failure, add a bug entry with reproduction steps and retest result.
