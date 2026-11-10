# Player artwork

Place these original exported images in this directory before making the final release:

- `cassette_art.png` — cassette-display artwork, background for the Now Playing stage;
- `paper_texture.png` — original paper-texture crop used on the window and button areas;
- `skin_background.png` — full original retro skin reference and design source;
- `logo.png` and `logo.ico` — app badge and Windows icon;
- `splash_background.png` — approved splash artwork reference;
- `Team/darrel.png`, `chris.png`, `mark.png`, `shanen.png`, `mitchel.png` — five consented team photos.

The main form and splash use actual WinForms controls defined in `Form1.Designer.cs` and `SplashForm.Designer.cs`. They can be selected, moved and resized in Visual Studio's graphical Designer. Artwork is decorative; do not flatten interactive buttons, playlists, video views or photo cards into a single picture.

All assets are copied to build/publish output by the project file. Missing artwork falls back to readable colors or a labeled photo placeholder. Check original design rights, editable source and credits before submission.
