# Mandatory AI-agent rules — Amazing Player

Read README.md, docs/PRD.md, docs/PHASE_PLAN.md, docs/INTEGRATION_CONTRACT.md and the assigned GitHub issue **before editing**.

1. The complete instructor starter project is authoritative. Appendix A of the PDF is **abridged**; do not fabricate missing playback implementation or duplicate starter classes.
2. Stay on your assigned feature branch. Before changes, state your exact file allowlist, dependencies, acceptance criteria, and testing plan. Change nothing outside that list without lead approval.
3. Do not modify shared `BasePlayerForm.cs`, `Program.cs`, `.sln`, `.csproj`, or other contributors' files unless the issue explicitly permits it. Request an API change rather than inventing incompatible methods.
4. Required architecture: C# Windows Forms, LibVLCSharp, IMediaControls, abstract MediaItem, AudioItem/VideoItem, concrete BasePlayerForm, derived player form with at least two meaningful overrides, separate LibraryStore/Theme and hand-written insertion sort.
5. Keep all four MenuStrip headings visible; do not cover VLC VideoView. Put skin configuration in Theme.cs and assets in the real WinForms project's Assets directory, copied to output.
6. No unrelated packages, framework changes, mass reformatting, artificial commit splitting, invented tests or fabricated authorship.
7. Keep each increment compiling **once the real project is imported**. Handle exceptions visibly, validate inputs, use XML docs for public classes and methods.
8. Before offering changes for review, report files changed, FRs covered, commands actually run, results, outstanding risks and the proposed PR.
9. Commits must reflect substantive work by the actual authenticated teammate. No forced pushes to main. The lead reviews and merges pull requests.
10. If the real starter/API is unavailable, stop at a documented, clearly marked proposal or pure helper; do not claim app compilation or playback.

Use docs/PHASE_PLAN.md and docs/WORKSTREAM_ASSIGNMENTS.md as the file-ownership policy.
