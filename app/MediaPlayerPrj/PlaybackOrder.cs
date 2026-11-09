namespace mediaPlayerPrj;

/// <summary>
/// Why the player wants to move to another playlist item.
/// </summary>
public enum AdvanceReason
{
    /// <summary>The user pressed the Next button or used the Next menu/shortcut.</summary>
    UserNext,

    /// <summary>The current track finished playing on its own (VLC end-of-media).</summary>
    MediaEnded
}

/// <summary>
/// Pure, UI-independent policy that decides which playlist index should play next (FR13).
/// It never touches VLC or WinForms, so it can be tested in isolation.
/// </summary>
public sealed class PlaybackOrder
{
    /// <summary>Returned when playback should stop instead of moving to another item.</summary>
    public const int NoNextIndex = -1;

    /// <summary>Current repeat mode. Defaults to <see cref="RepeatMode.Off"/>.</summary>
    public RepeatMode Repeat { get; set; } = RepeatMode.Off;

    /// <summary>
    /// Decides which playlist index should play after <paramref name="currentIndex"/>.
    /// </summary>
    /// <param name="currentIndex">Index of the current item, or -1 when nothing is selected.</param>
    /// <param name="count">Number of items in the playlist.</param>
    /// <param name="reason">Whether the user pressed Next or the track ended naturally.</param>
    /// <returns>A valid index from 0 to <paramref name="count"/> - 1, or <see cref="NoNextIndex"/>.</returns>
    public int GetNextIndex(int currentIndex, int count, AdvanceReason reason)
    {
        // Nothing selected yet: start from the first item.
        if (currentIndex < 0)
            return 0;

        // Repeat One: a track that ends on its own plays again.
        // Pressing Next still skips, so the user is never stuck on one song.
        if (Repeat == RepeatMode.One && reason == AdvanceReason.MediaEnded)
            return currentIndex;

        // Normal order: move to the following item while one exists.
        if (currentIndex < count - 1)
            return currentIndex + 1;

        // Reached the last item.
        // A user pressing Next always gets a song: wrap to the first item,
        // matching the existing BasePlayerForm.Next() behavior.
        if (reason == AdvanceReason.UserNext)
            return 0;

        // Repeat All: the playlist finished naturally, so start again from the top.
        if (Repeat == RepeatMode.All)
            return 0;

        // Repeat Off: the playlist finished naturally, so playback stops.
        return NoNextIndex;
    }
}
