# Antigravity workflow — Amazing Player

**Current baseline:** `main` has the professor-sample-derived, CI-compiled C# WinForms/VLC application (PR #8 merged). Do not create a new project or request an unavailable original ZIP. Read `docs/INTEGRATION_CONTRACT.md` for exact existing methods and unimplemented features.

**Before sharing media or team photos:** the GitHub repository must be **private**, as required by the instructor. GitHub currently reports it public; only the owner can change visibility.

## Individual branches already created
| Member | Existing branch | Issue |
|---|---|---|
| Darrel Cango | `feat/darrel-library` | [#3](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/3) |
| Chris Garcia | `feat/chris-playback-modes` | [#4](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/4) |
| Shanen Mirador | `feat/shanen-ui` | [#5](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/5) |
| Mitchel Susi | `feat/mitchel-favorites` | [#6](https://github.com/NeatKnight18586/amazing-oop10-media-player/issues/6) |

## Plain-language steps for each teammate

1. Accept Mark's GitHub invitation using **your own account**. Open or clone the repository in Antigravity. Choose **your existing branch** from the table; do not work directly on `main`.
2. Read `AGENTS.md`, `docs/PRD.md`, `docs/INTEGRATION_CONTRACT.md`, your GitHub issue, and `docs/TEAM_HANDOFF.md`. Give the assistant only your assigned issue and branch.
3. Ask the AI to inspect the existing code *first* and propose a small file-limited change. Review each diff carefully; test it if possible. If a method is missing from a file owned by someone else, ask Mark for an integration hook.
4. Commit **meaningful steps of real work** under your Git identity with messages such as `feat:`, `test:`, `fix:`, `docs:`, `refactor:`. No empty or artificially split commits and no commits under someone else's name.
5. Push your branch and open a GitHub **pull request into main**. Include FR requirements, files changed, tests run (or NOT RUN) and any integration request. Do not merge it yourself unless Mark approves.
6. Mark reviews the PR, performs Windows integration/tests and merges when safe. Sync your branch from the latest `main` before further changes.

## General copy-paste prompt

```text
You are assisting my assigned Amazing Player (OOP10) workstream.
Repository: https://github.com/NeatKnight18586/amazing-oop10-media-player
My issue: [paste assigned issue URL].
My branch: [paste my existing feat/... branch].

Read AGENTS.md, docs/PRD.md, docs/INTEGRATION_CONTRACT.md,
docs/PHASE_PLAN.md, and my issue before proposing changes.
Use the actual checked-in C# WinForms code; do not regenerate a
new application or replace the tested LibVLC player.

Tell me which files you will touch, the acceptance checks and the
smallest implementation step. Work only in my file ownership area.
If a change needs Form1.cs, BasePlayerForm.cs, Program.cs or .csproj
outside my ownership, prepare an integration request for Mark instead.

After each meaningful step: explain in simple language what changed,
show the diff, run the tests that are possible, and report any tests
that could not be run. Let me review and commit my own real work.
Do not fabricate test results, commits, screenshots or PR approvals.
```

For role-specific instructions see `docs/TEAM_HANDOFF.md`.
