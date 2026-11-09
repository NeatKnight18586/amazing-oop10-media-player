using mediaPlayerPrj;
using Xunit;

namespace PlaybackModes.Tests;

/// <summary>
/// Deterministic, step-by-step playback sessions. A scripted random source makes
/// every shuffle choice known in advance, so exact sequences can be checked.
/// </summary>
public class PlaybackTransitionTests
{
    /// <summary>Random source that returns pre-written choices in order.</summary>
    private sealed class ScriptedRandom : Random
    {
        private readonly Queue<int> _choices;

        public ScriptedRandom(params int[] choices) => _choices = new Queue<int>(choices);

        public override int Next(int maxValue)
        {
            int choice = _choices.Dequeue();
            Assert.InRange(choice, 0, maxValue - 1);
            return choice;
        }
    }

    /// <summary>Simulates natural track endings, like VLC's end-of-media event.</summary>
    private static List<int> PlayUntilStop(PlaybackOrder order, int start, int count, int maxSteps)
    {
        var played = new List<int> { start };
        int index = start;
        for (int i = 0; i < maxSteps; i++)
        {
            index = order.GetNextIndex(index, count, AdvanceReason.MediaEnded);
            if (index == PlaybackOrder.NoNextIndex) break;
            played.Add(index);
        }
        return played;
    }

    [Fact]
    public void TC21_RepeatOff_PlaysToEndThenStops()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.Off };
        Assert.Equal(new[] { 1, 2, 3 }, PlayUntilStop(order, 1, 4, 10));
    }

    [Fact]
    public void TC22_RepeatAll_WrapsFromLastToFirst()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.All };
        Assert.Equal(new[] { 2, 3, 0, 1, 2 }, PlayUntilStop(order, 2, 4, 4));
    }

    [Fact]
    public void TC23_RepeatOne_KeepsReplayingUntilUserPressesNext()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.One };
        Assert.Equal(new[] { 1, 1, 1, 1 }, PlayUntilStop(order, 1, 4, 3));

        // The user presses Next: playback moves on, and Repeat One applies to the new track.
        int next = order.GetNextIndex(1, 4, AdvanceReason.UserNext);
        Assert.Equal(2, next);
        Assert.Equal(2, order.GetNextIndex(next, 4, AdvanceReason.MediaEnded));
    }

    [Fact]
    public void TC24_ShuffleRepeatOff_ExactScriptedOrderThenStop()
    {
        // Bag after track 0 is [1, 2, 3]: pick position 2 -> 3, then [1, 2]: pick 0 -> 1, then [2] -> 2.
        var order = new PlaybackOrder(new ScriptedRandom(2, 0, 0))
        {
            Shuffle = true,
            Repeat = RepeatMode.Off
        };

        Assert.Equal(new[] { 0, 3, 1, 2 }, PlayUntilStop(order, 0, 4, 10));
    }

    [Fact]
    public void TC24_ShuffleRepeatAll_StartsNewRoundWithoutCurrentTrack()
    {
        // Round 1 from track 0: [1,2] -> pick 1 -> 2, then [1] -> 1.
        // Round 2 excludes current track 1: [0,2] -> pick 0 -> 0.
        var order = new PlaybackOrder(new ScriptedRandom(1, 0, 0))
        {
            Shuffle = true,
            Repeat = RepeatMode.All
        };

        Assert.Equal(new[] { 0, 2, 1, 0 }, PlayUntilStop(order, 0, 3, 3));
    }

    [Fact]
    public void TC25_OneItemPlaylist_EveryModeBehavesAsDocumented()
    {
        Assert.Equal(new[] { 0 }, PlayUntilStop(new PlaybackOrder { Repeat = RepeatMode.Off }, 0, 1, 5));
        Assert.Equal(new[] { 0, 0, 0 }, PlayUntilStop(new PlaybackOrder { Repeat = RepeatMode.All }, 0, 1, 2));
        Assert.Equal(new[] { 0, 0, 0 }, PlayUntilStop(new PlaybackOrder { Repeat = RepeatMode.One }, 0, 1, 2));
        Assert.Equal(new[] { 0 }, PlayUntilStop(new PlaybackOrder(new ScriptedRandom()) { Shuffle = true }, 0, 1, 5));
    }

    [Fact]
    public void UserNext_AndMediaEnded_DifferOnlyWhereTheRulesSaySo()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.Off };

        // Middle of playlist: both reasons move forward.
        Assert.Equal(order.GetNextIndex(1, 4, AdvanceReason.UserNext),
                     order.GetNextIndex(1, 4, AdvanceReason.MediaEnded));

        // Last track: natural end stops, user Next wraps.
        Assert.Equal(PlaybackOrder.NoNextIndex, order.GetNextIndex(3, 4, AdvanceReason.MediaEnded));
        Assert.Equal(0, order.GetNextIndex(3, 4, AdvanceReason.UserNext));
    }
}
