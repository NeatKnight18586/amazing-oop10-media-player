namespace mediaPlayerPrj;

/// <summary>
/// Controls what happens when the player reaches the end of a track or the playlist (FR13).
/// </summary>
public enum RepeatMode
{
    /// <summary>Play through the playlist once, then stop after the last item.</summary>
    Off,

    /// <summary>After the last item, wrap around and continue from the first item.</summary>
    All,

    /// <summary>When a track ends on its own, play the same track again.</summary>
    One
}

/// <summary>
/// Helper methods for <see cref="RepeatMode"/> that the UI can use for a single repeat button.
/// </summary>
public static class RepeatModeExtensions
{
    /// <summary>
    /// Returns the mode a repeat button should switch to when clicked:
    /// Off → All → One → Off.
    /// </summary>
    /// <param name="mode">The current repeat mode.</param>
    /// <returns>The next repeat mode in the cycle.</returns>
    public static RepeatMode NextMode(this RepeatMode mode) => mode switch
    {
        RepeatMode.Off => RepeatMode.All,
        RepeatMode.All => RepeatMode.One,
        _ => RepeatMode.Off
    };

    /// <summary>Returns a short, user-friendly label for menus and status text.</summary>
    /// <param name="mode">The repeat mode to describe.</param>
    /// <returns>"Repeat: Off", "Repeat: All" or "Repeat: One".</returns>
    public static string ToDisplayText(this RepeatMode mode) => mode switch
    {
        RepeatMode.All => "Repeat: All",
        RepeatMode.One => "Repeat: One",
        _ => "Repeat: Off"
    };
}
