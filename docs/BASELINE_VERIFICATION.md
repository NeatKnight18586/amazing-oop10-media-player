# Bootstrap validation record — 2026-11-08

This is a provenance-aware engineering record; it does **not** replace the final formal test plan or second-PC deployment test.

## Source and merge
- Instructor provided a 21-page OOP10 project PDF and a separate posted `Form1 : Form` LibVLCSharp example.
- The posted example contained commented-out functions and lacked `.csproj` and `Form1.Designer.cs`. We reconstructed a temporary programmatic WinForms UI and a concrete `BasePlayerForm` conforming to the PDF's OOP requirements.
- [PR #8](https://github.com/NeatKnight18586/amazing-oop10-media-player/pull/8) merged into main at commit `a7366e8c64b182c22090964fc4359023bc1a82c3`.

## Automated GitHub checks
- Windows CI restore/build and self-contained win-x64 publish **completed successfully** for PR head `925e4b59c0bbee6486b3ad918a320d75454630d3`.
- [PR build run 42486131178](https://github.com/NeatKnight18586/amazing-oop10-media-player/actions/runs/42486131178).
- These prove source compilation/publish steps; they do **not** prove VLC playback, skin usability, nor correctness of unfinished features.

## Manual Windows smoke test (lead's report)
- On Nov 8, Mark stated that the application, MP3 and MP4 playback, controls, elapsed counter and close behavior were all working on a temporary Windows computer.
- **Evidence type:** self-reported verbal confirmation in group planning conversation. Screenshots, machine information, detailed steps and independent reproduction have **not** been attached yet.
- **Result:** basic playback smoke check accepted as a green light to merge bootstrap; full FR test suite remains pending.

## Explicitly unverified
- Required `Library.txt` save/load and parsing; video resolution; Favorites; sorting; shuffle/repeat; original player skin; splash screen and member portraits; 100%/125% scaling; all keyboard shortcuts; negative cases.
- 15+ formally executed test cases and retests.
- Installation on **another** Windows computer and final submission release ZIP.

## Owner's follow-up
Capture one actual Visual Studio running screenshot and Windows version; record at least one MP3 and MP4 filename (not copyrighted file contents) and the real observed result; fill docs/TEST_PLAN.md cases only as tested.
