# Amazing Player — WinForms application

The **reconstructed starter is present** in `MediaPlayerPrj/`. It was adapted from the professor's posted Form1 example and the OOP10 final PDF guide (not from an uploaded complete Visual Studio starter ZIP).

Open `MediaPlayerPrj/MediaPlayerPrj.csproj` in Visual Studio 2022/2026 (install .NET desktop development workload). Target: **.NET 8 / Windows x64**. The UI is created in code in `Form1.cs` (no `Form1.Designer.cs`), and `BasePlayerForm.cs` owns LibVLC and playback.

**Verified status:** GitHub Actions Windows restore/build and publish PASSED; Mark reports basic MP3/MP4/control/clock/exit smoke tests PASSED on Windows. No detailed test log, second-PC deployment check or final release yet.

**Unfinished student features:** LibraryStore (six-field Library.txt), insertion sort, accurate MP4 resolution, shuffle/repeat, Favorites/Liked Songs, original PNG skin, five-member splash and manuals/testing. See `../docs/PHASE_PLAN.md` and `../docs/INTEGRATION_CONTRACT.md` before editing.

Each feature is developed by its assigned teammate on a branch; lead integrates shared form/base/project hooks. Do not replace the working bootstrap or edit unrelated files.
