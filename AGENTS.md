# Mandatory AI-agent rules — Amazing Player

Read README.md, docs/PRD.md, docs/PHASE_PLAN.md, docs/INTEGRATION_CONTRACT.md and your assigned GitHub issue **before editing**.

1. `main` now has a **reconstructed, CI-compiled C# WinForms starter** derived from the instructor's direct Form1 sample and final PDF guide. Do not recreate or replace it. The PDF examples are abridged; follow the final requirements and the **actual checked-in source**.
2. Stay on your feature branch. Before edits, enumerate your exact file allowlist, dependencies, tests and acceptance criteria. Change nothing beyond your assigned files without explicit lead approval.
3. `BasePlayerForm.cs`, `Program.cs`, project files and shared integration hooks are owned by Mark; `Form1.cs` / UI by Shanen. Model, policy and Favorites contributors should request integration rather than editing those files.
4. Preserve interface `IMediaControls`, abstract `MediaItem`, concrete `AudioItem`/`VideoItem`, concrete designer-safe `BasePlayerForm` and derived `Form1`, with meaningful overrides. Keep file I/O, UI and theme separate. Use hand-written insertion sort; **do not use `List.Sort`/`OrderBy`** for FR12.
5. Keep File/Playback/Library/Help MenuStrip headings visible and shortcuts functional; never overlay the VLC `VideoView`. Put original skin assets in `app/MediaPlayerPrj/Assets/` with Copy to Output Directory and theme configuration in `Theme.cs`.
6. No unnecessary dependencies, framework changes, mass refactoring, artificial commit splitting, fabricated tests or attributed code from someone else.
7. Make incremental, compilable, testable changes. Catch file/media errors and report meaningful status; public classes/methods should have XML documentation. If tests cannot run in your environment, say **NOT RUN**.
8. Each PR states changed files, FR IDs, tests actually run, results, known risks and integration requests. Do not claim Windows GUI verification from a non-Windows environment.
9. Each contributor commits **their own real work** using their own GitHub identity; never force-push `main`. Lead reviews and merges PRs.
10. If an API is missing, request a narrow hook via GitHub issue rather than inventing a conflicting API or modifying another owner's code.

See docs/WORKSTREAM_ASSIGNMENTS.md and docs/INTEGRATION_CONTRACT.md.
