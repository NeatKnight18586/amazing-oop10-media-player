# Antigravity contributor workflow (feature branch + PR)

Prerequisite: Mark imports the actual starter, records stable API in INTEGRATION_CONTRACT.md, and assigns a scoped issue. Do not invent the missing core player from PDF snippets.

1. Open the repository in Antigravity, fetch main and create a new feature branch (e.g. `feat/darrel-library`, `feat/chris-playback-modes`, `feat/shanen-ui`, `feat/mitchel-favorites`).
2. Read AGENTS.md, docs/PRD.md, docs/PHASE_PLAN.md and your issue; inspect the existing code and only edit paths your issue permits.
3. Ask the AI to propose a *small* change and its validation. Review diff yourself and run relevant tests where possible. For untestable Windows UI, record NOT RUN and request Mark's Windows check.
4. Save substantive changes and create commits using your own Git identity. Push your feature branch and open a PR targeting main. Tell Mark the PR URL.
5. Mark reviews diff, checks real build/integration, requests fixes if needed and merges. Refresh your branch from updated main before new work.

## Copy-paste prompt for assigned task
```text
You are working on the Amazing Player C# WinForms project.
Read AGENTS.md, docs/PRD.md, docs/PHASE_PLAN.md,
docs/WORKSTREAM_ASSIGNMENTS.md and docs/INTEGRATION_CONTRACT.md.
My assigned GitHub issue is: [ISSUE URL].
My branch is: [FEATURE BRANCH].
Work only in files explicitly owned by this issue.
Before editing: inspect actual existing source, enumerate exact files,
dependencies, requirements and acceptance tests. Propose the smallest
change that fulfills this issue without breaking the instructor starter.
If APIs are absent or conflict, stop and write a brief integration
request for Mark. Never invent the VLC base player or modify someone
else's files. Implement, explain the result in simple terms, run tests
that the environment supports, and truthfully report NOT RUN items.
Show the diff before I commit. I will review and push my own work.
```

## Mark's Codex review prompt
```text
Review this Amazing Player PR [URL]. Read AGENTS.md, the PRD, phase
plan, integration contract and actual instructor starter. Check
requirements, allowed file paths, OOP correctness, WinForms designer
safety, playback event threading, error handling and tests.
Do not refactor unrelated files. Identify integration hooks needed in
my owned files. Report which tests actually ran and which did not.
Recommend merge only if changes are scoped and safe.
```

Each member should have substantive reviewable progress throughout the assignment. No sham commits, fake test reports, or shared impersonated authorship.
