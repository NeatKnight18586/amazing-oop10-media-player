# Work on Amazing Player

## Current implementation
The latest M1–M3 changes are in [PR #11](https://github.com/NeatKnight18586/amazing-oop10-media-player/pull/11), branch `feat/nov10-milestones`. They include the library persistence/sorting, editable Windows Forms Designer files, cassette artwork, splash, and connections to the previously merged playback and Favorites classes.

## To open the project
1. Use `AmazingPlayer.sln` in Visual Studio 2022/2026.
2. Select `Form1.cs` → **View Designer** and verify controls are individually editable.
3. Do the same for `SplashForm.cs`. The five portrait images are currently placeholders.
4. Press F5 to verify actual MP3/MP4 playback and full M3 functionality.
5. Keep screenshots of the real designer and running app, then report defects before merging.

## Verification
GitHub Windows CI compiles and publishes the app and runs the three unit test suites (163 tests). These do not replace live GUI testing; the tests and project artifacts should be checked before submission.

## Submission responsibilities
- M1: review `docs/milestones/M1/` and the separate design packet; verify GitHub invitations and original art source.
- M2: verify Library.txt round trip and error handling on a Windows computer.
- M3: verify original skin, splash, menus, elapsed time, insertion sort, shuffle/repeat and Favorites.
- Make repository **Private** as required; do not add real member photographs while it is public.

## Group
BSCS2A / Group 1 / Amazing — Darrel Cango, Chris Garcia, Mark Guevarra, Shanen Mirador, Mitchel Susi.
