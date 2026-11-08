# Original skin handoff — Shanen / design team

**Art direction:** restrained, readable original minimal media-player skin. Normal window frame unless the team approves a different design. Approx. 1100×700 logical-pixel concept only; actual form/client coordinates must be approved after inspecting starter and 100%/125% display scaling.

## Required assets
1. `Assets/player_background.png` — original Canva/Photoshop-exported PNG at the approved form client size.
2. `Assets/logo.png` — original transparent ~512x512 logo, recognizable at small size.
3. `Assets/logo.ico` — icons including 16/32/48/256px sizes, for form/exe.
4. `Assets/Team/` — five consented portraits (approx. 200x200 source each), named unambiguously. Splash includes full name + role and handles missing pictures with placeholders.
5. Optional coherent icon PNGs for play/pause/stop/next/prev, heart, shuffle, repeat, close/minimize **if needed**.
6. Editable design source: Canva view-access link or PSD; licensing/attribution list for reused fonts/icons; screenshot of the actual running form.

## Required visible UI zones
- MenuStrip (File / Playback / Library / Help), keyboard shortcut discoverability.
- VLC VideoView in its own **uncovered** host panel (do not overlay labels or skin on video).
- Playlist with title, artist, album/resolution, duration, favorites indication and sort controls.
- Now Playing: current title/artist/type/extra, elapsed/total.
- Play, Pause, Stop, Next, Previous; volume OR seek, shuffle toggle, repeat mode, Add Files, Favorite heart, Favorites filter, status and actionable errors.
- Stable, high-contrast text, keyboard use, selected/focused states and enough space for long titles.
- If borderless chosen later: visible Close/Minimize, mouse drag in Theme; status always visible without title bar.

## Engineer handoff
Provide PNG dimensions and placement rectangles for playlist, VideoView host, menu, buttons, labels, status, clock and heart. Store ALL actual skin assets within the real WinForms project Assets/ folder (not just top-level handoff). Set Copy to Output Directory. Theme.cs/ApplyTheme controls skin behavior. Do not edit VLC playback from a design ticket.

**Do not claim a Canva mockup is the final app screenshot.** Test actual Windows form at 100% and 125% scaling.
