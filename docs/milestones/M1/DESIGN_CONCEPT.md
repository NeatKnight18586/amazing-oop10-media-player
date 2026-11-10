# Original skin concept — cassette player

**Theme:** warm worn-paper and cassette-tape style, with aged gold, espresso-brown ink and restrained red stripes. The team provided a cassette-player reference; the new layout removes all franchise branding.

**Main form:** standard Windows frame and visible MenuStrip; large cassette/display on the left, real playlist ListView on the right; labeled transport buttons below; seek, elapsed time, volume and readable status at the bottom.

**Dynamic visuals:** current track title appears on a WinForms Label over the cassette display. MP4 video takes the dedicated VLC panel rather than being covered by the artwork. Actual playlist rows, status, buttons and timers are separate WinForms controls.

**Splash:** app logo, Amazing / Group 1 / BSCS2A, five members' roles and photos; missing image fallback; click/key skip.

**Editable implementation:** the source contains Form1.Designer.cs and SplashForm.Designer.cs with named controls, rather than rebuilding the control tree in the Form1 runtime constructor. The visual concepts remain references; the final screenshots must be taken from Visual Studio after importing the assets.

**Asset handoff:** place cassette_art.png, skin_background.png, logo.png, logo.ico and splash_background.png in app/MediaPlayerPrj/Assets. Portraits belong in Assets/Team. Confirm the editable source, artwork rights and any required credits for the submission.