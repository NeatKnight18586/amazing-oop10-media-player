# Release deployment guide — execution pending

Instructor Appendix E recommends Windows 10/11 x64, Release/self-contained .NET 8 (verify against actual starter target), `win-x64`, and retaining the native VLC plugin/runtime folders. Test on a **different** Windows PC.

## Before publishing
- [ ] Full source restored/builds in Visual Studio
- [ ] MP3/MP4, controls, menu, sorting, shuffle/repeat, Favorites, splash all work
- [ ] Original skin/Logo/Team assets in real WinForms `Assets/` with Copy to Output Directory
- [ ] Product/company/version metadata and logo.ico set
- [ ] App writes Library.txt/Favorites.txt to a folder the user can write to
- [ ] System requirements and known limitations documented

## Publish (adapt actual project path and target)
```powershell
dotnet publish -c Release -r win-x64 --self-contained true -o publish
```
Use folder publishing; do not enable trimming or single-file if that compromises VLC. Confirm *.exe, Assets, native libvlc and plug-ins present. ZIP **entire** output folder, not exe alone.

## Second-PC verification
- [ ] Windows PC has no dependency on original developer environment
- [ ] Extract into Desktop/Documents (not restricted Program Files)
- [ ] Splash/photos/logo load; MP3 and MP4 play
- [ ] Controls, shortcuts, sorting, shuffle/repeat, Favorites and clock work
- [ ] Close/reopen retains library and favorites
- [ ] Exercise a missing-file or invalid-line error
- [ ] Evidence: machine info, screenshots, actual result and recorded issues

**Status:** NOT RUN. No release ZIP claimed.
