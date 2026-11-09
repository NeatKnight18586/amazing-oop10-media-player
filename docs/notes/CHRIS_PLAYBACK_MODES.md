# Playback modes — technical notes (Chris Garcia)

**Issue:** #4 · **Branch:** `feat/chris-playback-modes` · **Requirement:** FR13 (Shuffle on/off; Repeat Off/All/One; automatic next on media end, including 1-item and last-item cases)

## Files

| File | Purpose |
|---|---|
| `app/MediaPlayerPrj/RepeatMode.cs` | `RepeatMode` enum (Off/All/One) + `NextMode()` and `ToDisplayText()` helpers for a single repeat button |
| `app/MediaPlayerPrj/PlaybackOrder.cs` | `AdvanceReason` enum + `PlaybackOrder`, the pure "which index plays next?" policy |
| `tests/PlaybackModes.Tests/` | Isolated xUnit tests (plain `net8.0`, no WinForms/VLC). Source files are linked, not copied |

Neither class references WinForms or VLC. No existing file owned by Mark or Shanen was changed.

## Rules

`GetNextIndex(currentIndex, count, reason)` returns a valid index (`0` to `count - 1`) or `PlaybackOrder.NoNextIndex` (`-1`, meaning stop).

| Situation | Song ended on its own (`MediaEnded`) | User pressed Next (`UserNext`) |
|---|---|---|
| Middle of playlist, shuffle off | next item | next item |
| Last item, Repeat Off | **stop** | wrap to first |
| Last item, Repeat All | wrap to first | wrap to first |
| Any item, Repeat One | **same item again** | next item (wraps at end) |
| Shuffle on | random unplayed item; after every item has played once: stop (Off) or start a new round (All) | random unplayed item; starts a new round instead of stopping |
| Nothing selected (`-1`) | first item (random item if shuffle) | first item (random item if shuffle) |
| Empty playlist | stop | stop |
| One-item playlist | Off: stop · All/One: replay | replay |
| Stale index (`currentIndex >= count`) | treated as nothing selected | treated as nothing selected |

Design decisions:
- **User Next never stops playback.** It wraps at the end, matching the existing `BasePlayerForm.Next()` behavior.
- **Repeat One only affects natural endings.** Pressing Next still skips, so the user is never stuck on one song.
- **Shuffle uses a "bag".** Every track plays once before any track repeats, and a new round never starts with the track that just played.
- **Randomness is injectable.** `new PlaybackOrder(Random)` accepts a seeded or fake random source so tests are deterministic. The parameterless constructor uses a normal `Random`.

## Public API

```csharp
public enum RepeatMode { Off, All, One }
public static RepeatMode NextMode(this RepeatMode mode);      // Off -> All -> One -> Off
public static string ToDisplayText(this RepeatMode mode);     // "Repeat: Off" / "Repeat: All" / "Repeat: One"

public enum AdvanceReason { UserNext, MediaEnded }

public sealed class PlaybackOrder
{
    public const int NoNextIndex = -1;
    public PlaybackOrder();
    public PlaybackOrder(Random random);
    public RepeatMode Repeat { get; set; }   // default Off
    public bool Shuffle { get; set; }        // default false; setting it starts a fresh shuffle round
    public void ResetShuffle();              // call when the playlist is replaced, sorted or edited
    public int GetNextIndex(int currentIndex, int count, AdvanceReason reason);
}
```

## Integration request for Mark (`BasePlayerForm.cs`)

Requested hook. Mark owns this file, so the following is a suggestion, not a change I made:

```csharp
/// <summary>Shuffle/repeat policy shared with the derived form's mode controls.</summary>
protected readonly PlaybackOrder PlaybackModes = new();

protected virtual void PlayNextOrStop()
{
    int next = PlaybackModes.GetNextIndex(CurrentIndex, Playlist.Count, AdvanceReason.MediaEnded);
    if (next == PlaybackOrder.NoNextIndex)
    {
        Stop();
        return;
    }
    CurrentIndex = next;
    Play();
}

public virtual void Next()
{
    int next = PlaybackModes.GetNextIndex(CurrentIndex, Playlist.Count, AdvanceReason.UserNext);
    if (next == PlaybackOrder.NoNextIndex) return;
    CurrentIndex = next;
    Play();
}
```

Also call `PlaybackModes.ResetShuffle();` at the end of `SetPlaylist(...)`, so sorting or reloading the library starts a fresh shuffle round.

Notes for Mark:
- Repeat One returns the same index. `Play()` then creates a new `Media` because the player state after end-of-media is not `Paused`, so the track restarts from the beginning. Please confirm this on Windows.
- `PlayNextOrStop()` already runs on the UI thread through `BeginInvoke`, so calling `Play()` from it does not block VLC's event thread.
- `Previous()` is unchanged; FR13 does not define shuffle history for Previous.

## Integration request for Shanen (`Form1.cs`)

Once `PlaybackModes` exists in `BasePlayerForm`:

| Control | Behavior |
|---|---|
| **Shuffle** toggle (button and/or checkable item under the *Playback* menu) | `PlaybackModes.Shuffle = !PlaybackModes.Shuffle;` and show the on/off state |
| **Repeat** button (and/or *Playback* menu item) | `PlaybackModes.Repeat = PlaybackModes.Repeat.NextMode();` |
| Mode indicator (button text, tooltip or status bar) | `PlaybackModes.Repeat.ToDisplayText()` and "Shuffle: On/Off" |

Shortcut keys are Shanen's choice; they should not clash with the existing File/Playback/Library/Help shortcuts.

## Test evidence

Command (run from the repository root):

```
dotnet test tests\PlaybackModes.Tests
```

| Environment | Result |
|---|---|
| Linux, .NET SDK 8.0.133 | **PASS:** 88 passed, 0 failed |
| Chris's Windows laptop, .NET 8 | Recorded on each checkpoint commit (`dotnet test` run before committing) |
| WinForms app build with these files | **NOT RUN locally.** Checked by the GitHub Actions *Windows build* workflow on push |
| Manual GUI tests TC21–TC25 in the running player | **NOT RUN.** Requires Mark's hook and Shanen's controls first |

Test plan coverage (automated, policy level):

| Test case | Covered by |
|---|---|
| TC21 Repeat Off from last track | `RepeatOffTests`, `PlaybackTransitionTests.TC21_*` |
| TC22 Repeat All from last track | `RepeatAllTests`, `PlaybackTransitionTests.TC22_*` |
| TC23 Repeat One | `RepeatOneTests`, `PlaybackTransitionTests.TC23_*` |
| TC24 Shuffle with several items | `ShuffleTests`, `CombinedModeTests`, `PlaybackTransitionTests.TC24_*` |
| TC25 One-item playlist in every mode | `PlaylistEdgeTests`, `PlaybackTransitionTests.TC25_*` |

Bug found and fixed during development (`fix: handle playlist edges`): before the fix, an empty playlist returned index `0`, shuffle crashed on a one-item playlist, and Repeat One could return an index past the end after the playlist shrank (19 failing edge-case tests, then all passing).
