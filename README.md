# Amazing Player — OOP10

**BSCS2A · Group 1 · Amazing**  
Cango, Darrel · Garcia, Chris · Guevarra, Mark · Mirador, Shanen · Susi, Mitchel

A collaborative **C# Windows Forms** MP3/MP4 media player using LibVLCSharp. The application foundation was **reconstructed from the instructor's posted MediaPlayerPrj sample and final 21-page PDF guide**; it is not a byte-for-byte copy of an instructor-supplied Visual Studio project.

> **Current state (Nov 8, 2026): runnable bootstrap merged into `main` via [PR #8](https://github.com/NeatKnight18586/amazing-oop10-media-player/pull/8). GitHub Windows CI build and self-contained publish succeeded; Mark reports MP3, MP4, buttons, elapsed counter and closing work on a Windows PC.** This is a manual smoke-test report, not the final 15-case test report. See [baseline record](docs/BASELINE_VERIFICATION.md).

## App

Open `app/MediaPlayerPrj/MediaPlayerPrj.csproj` in Visual Studio 2022/2026 with the .NET desktop development workload, restore dependencies and press F5. Current form is a **temporary programmatic WinForms interface**; an original skin and the other required features still need implementing.

## Confirmed decisions

- Section **BSCS2A**, **Group 1**, team **Amazing**; working title **Amazing Player**.
- **FR8:** Favorites / Liked Songs, persisted separately from the required six-field `Library.txt`.
- **FR12:** hand-written **insertion sort** by Title and Artist, keeping the current item.
- Original minimalist PNG skin inside the project `Assets/`; standard Windows border by default.
- C# Windows Forms + LibVLCSharp, using instructor OOP hierarchy.

## Work together safely

1. Read [START_HERE.md](START_HERE.md), [AGENTS.md](AGENTS.md), [PRD](docs/PRD.md), and [integration contract](docs/INTEGRATION_CONTRACT.md).
2. Consult [team responsibilities](docs/WORKSTREAM_ASSIGNMENTS.md) and [phases](docs/PHASE_PLAN.md). Open your designated [GitHub issue](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues).
3. Work in your own feature branch; modify only your assigned files; review your AI's changes; commit real work under your own account; push and open a PR.
4. Lead reviews, merges and reruns the Windows build; UI/asset changes require a real Windows smoke test.

## Still to deliver

Six-field `Library.txt` load/save, resolution metadata, sorting, shuffle/repeat, Favorites, final skin and splash screen, actual UML, >=15 executed tests, bug log, manual, deployment on **another** Windows PC, recorded demo, portfolio PDF, and final ZIP.

**Instructor deadlines:** M1–M3 Nov 10, M4 Nov 12, M5 Nov 14, 2026. At least 10 **meaningful contributions committed by each member** (50+ genuine commits total). Invite the instructor to the **private** repository.

**Privacy alert:** GitHub reported this repository as **PUBLIC** on Nov 8 despite the guide requiring private. Repository owner must change Settings → General → Danger Zone → Change repository visibility → Private before sharing portraits or other sensitive materials.

## Submission

GitHub repository link, `G1_BSCS2A_Portfolio.pdf`, `G1_BSCS2A_Release.zip`, recorded demonstration, design source/credits and evidence. Do not commit copied commercial media, secrets, personal contact data, runtime `Library.txt`/`Favorites.txt`, `bin/`, `obj/`, or publish output.

**Instructor basis:** 21-page OOP10 final project guide + the professor's direct sample code. Source comparison: [docs/INSTRUCTOR_SAMPLE_RECONCILIATION.md](docs/INSTRUCTOR_SAMPLE_RECONCILIATION.md).
