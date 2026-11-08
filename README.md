# Amazing Player — OOP10

**BSCS2A · Group 1 · Amazing**  
Cango, Darrel · Garcia, Chris · Guevarra, Mark · Mirador, Shanen · Susi, Mitchel

A collaborative C# Windows Forms MP3/MP4 media player built from the instructor's full starter project, using LibVLCSharp.

> **Current state: planning only. Not buildable yet.** The PDF guide contains abridged source examples, not the complete instructor starter solution. Do not create replacement playback code until the actual `.sln` / `.csproj` and full `BasePlayerForm.cs` have been imported and inspected.

## Decisions

- Team name: **Amazing**; section **BSCS2A**; group **1**.
- App working title: **Amazing Player**.
- FR8 enhancement: **Favorites / Liked Songs**, persisted separately from the prescribed six-field `Library.txt`.
- FR12: **hand-written insertion sort** by Title and Artist, preserving the playing item.
- Original minimal player skin as PNG in the WinForms project's `Assets/` folder; keep a conventional window border.
- User interface: WinForms with a visible menu strip, original splash screen, and unobstructed LibVLC video surface.

## Start here

1. Read [START_HERE.md](START_HERE.md) and [AGENTS.md](AGENTS.md).
2. Review [PRD](docs/PRD.md), [phased plan](docs/PHASE_PLAN.md), [team responsibilities](docs/WORKSTREAM_ASSIGNMENTS.md), and [integration contract](docs/INTEGRATION_CONTRACT.md).
3. Import the **complete instructor starter** into `app/` preserving its existing layout; run a baseline Windows/Visual Studio build before independent coding starts.
4. Build features in separate branches, submit pull requests, and keep actual test results and review evidence.

## Deadlines from instructor guide

**M1–M3: Nov 10, 2026. M4: Nov 12. M5: Nov 14.** Confirm exact cutoff times with instructor. Each of the five members must have **at least 10 meaningful commits** of their own work. The repository must remain private, with the instructor invited as a collaborator.

## Submission

Repository URL, final portfolio PDF, Release ZIP tested on a different Windows PC, recorded demo, actual UML/test evidence, source/design credits. Do not commit actual MP3/MP4 files without rights, secrets, local `Library.txt` or `Favorites.txt`, `bin/`, `obj/`, or publish output.

**Requirement source:** instructor's 21-page “Media Player Application Project — OOP10” guide, §§1–8 and Appendices A–E. The source PDF remains in the group's original handoff ZIP and should be retained by the group; this repository documents page references in `docs/REQUIREMENTS_CHECKLIST.md`.
