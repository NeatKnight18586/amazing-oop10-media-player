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

    private readonly Random _random;
    private readonly List<int> _shuffleRemaining = new();
    private int _shuffleCount = -1;
    private bool _shuffle;

    /// <summary>Creates a playback order that uses a non-deterministic random source.</summary>
    public PlaybackOrder() : this(new Random()) { }

    /// <summary>
    /// Creates a playback order with a specific random source.
    /// Tests pass a seeded or fake <see cref="Random"/> so shuffle results are repeatable.
    /// </summary>
    /// <param name="random">Random source used to pick shuffled tracks.</param>
    public PlaybackOrder(Random random)
    {
        _random = random ?? throw new ArgumentNullException(nameof(random));
    }

    /// <summary>Current repeat mode. Defaults to <see cref="RepeatMode.Off"/>.</summary>
    public RepeatMode Repeat { get; set; } = RepeatMode.Off;

    /// <summary>
    /// When true, tracks play in random order. Every track plays once before any
    /// track repeats. Changing this value starts a fresh shuffle round.
    /// </summary>
    public bool Shuffle
    {
        get => _shuffle;
        set
        {
            _shuffle = value;
            ResetShuffle();
        }
    }

    /// <summary>
    /// Forgets which tracks were already played in the current shuffle round.
    /// Call this when the playlist is replaced, sorted or edited.
    /// </summary>
    public void ResetShuffle()
    {
        _shuffleRemaining.Clear();
        _shuffleCount = -1;
    }

    /// <summary>
    /// Decides which playlist index should play after <paramref name="currentIndex"/>.
    /// </summary>
    /// <param name="currentIndex">Index of the current item, or -1 when nothing is selected.</param>
    /// <param name="count">Number of items in the playlist.</param>
    /// <param name="reason">Whether the user pressed Next or the track ended naturally.</param>
    /// <returns>A valid index from 0 to <paramref name="count"/> - 1, or <see cref="NoNextIndex"/>.</returns>
    public int GetNextIndex(int currentIndex, int count, AdvanceReason reason)
    {
        // Repeat One: a track that ends on its own plays again.
        // Pressing Next still skips, so the user is never stuck on one song.
        if (currentIndex >= 0 && Repeat == RepeatMode.One && reason == AdvanceReason.MediaEnded)
            return currentIndex;

        if (Shuffle)
            return GetShuffledIndex(currentIndex, count, reason);

        // Nothing selected yet: start from the first item.
        if (currentIndex < 0)
            return 0;

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

    /// <summary>Picks a random track that has not been played yet in this shuffle round.</summary>
    private int GetShuffledIndex(int currentIndex, int count, AdvanceReason reason)
    {
        // The playlist size changed (or this is the first shuffle): start a new round.
        if (_shuffleCount != count)
            StartShuffleRound(currentIndex, count);

        // The current track counts as played in this round.
        _shuffleRemaining.Remove(currentIndex);

        if (_shuffleRemaining.Count == 0)
        {
            // Every track has played once. Repeat Off ends here, like the normal order.
            if (reason == AdvanceReason.MediaEnded && Repeat == RepeatMode.Off)
            {
                ResetShuffle();
                return NoNextIndex;
            }

            // Repeat All, or the user pressed Next: begin another shuffled round.
            StartShuffleRound(currentIndex, count);
        }

        int pick = _random.Next(_shuffleRemaining.Count);
        int next = _shuffleRemaining[pick];
        _shuffleRemaining.RemoveAt(pick);
        return next;
    }

    /// <summary>Fills the shuffle bag with every index except the current one.</summary>
    private void StartShuffleRound(int currentIndex, int count)
    {
        _shuffleRemaining.Clear();
        for (int i = 0; i < count; i++)
        {
            if (i != currentIndex)
                _shuffleRemaining.Add(i);
        }
        _shuffleCount = count;
    }
}
