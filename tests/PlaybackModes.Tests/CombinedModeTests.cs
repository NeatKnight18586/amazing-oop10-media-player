using mediaPlayerPrj;
using Xunit;

namespace PlaybackModes.Tests;

/// <summary>Shuffle and repeat interactions, and switching modes during playback.</summary>
public class CombinedModeTests
{
    [Fact]
    public void ShuffleWithRepeatAll_NeverStopsAndNeverRepeatsBackToBack()
    {
        var order = new PlaybackOrder(new Random(5)) { Shuffle = true, Repeat = RepeatMode.All };
        var counts = new int[4];
        int index = 0;
        counts[index]++;

        for (int i = 0; i < 20; i++)
        {
            int next = order.GetNextIndex(index, 4, AdvanceReason.MediaEnded);
            Assert.NotEqual(PlaybackOrder.NoNextIndex, next);
            Assert.NotEqual(index, next);
            index = next;
            counts[index]++;
        }

        Assert.All(counts, c => Assert.True(c >= 3, "Every track should keep coming back."));
    }

    [Fact]
    public void ShuffleWithRepeatOne_TrackEnds_ReplaysSameTrack()
    {
        var order = new PlaybackOrder(new Random(5)) { Shuffle = true, Repeat = RepeatMode.One };
        Assert.Equal(3, order.GetNextIndex(3, 6, AdvanceReason.MediaEnded));
    }

    [Fact]
    public void ShuffleWithRepeatOne_UserPressesNext_MovesToAnotherTrack()
    {
        var order = new PlaybackOrder(new Random(5)) { Shuffle = true, Repeat = RepeatMode.One };
        int next = order.GetNextIndex(3, 6, AdvanceReason.UserNext);
        Assert.InRange(next, 0, 5);
        Assert.NotEqual(3, next);
    }

    [Fact]
    public void ShuffleWithRepeatOff_UserPressesNextAfterRoundEnds_KeepsPlaying()
    {
        var order = new PlaybackOrder(new Random(5)) { Shuffle = true, Repeat = RepeatMode.Off };
        int index = 0;
        for (int i = 0; i < 2; i++)
            index = order.GetNextIndex(index, 3, AdvanceReason.MediaEnded);

        // All 3 tracks have played; a natural end would stop, but Next must still work.
        int next = order.GetNextIndex(index, 3, AdvanceReason.UserNext);
        Assert.InRange(next, 0, 2);
    }

    [Fact]
    public void TurningShuffleOff_ReturnsToNormalOrderFromCurrentTrack()
    {
        var order = new PlaybackOrder(new Random(5)) { Shuffle = true };
        order.GetNextIndex(0, 5, AdvanceReason.MediaEnded);

        order.Shuffle = false;
        Assert.Equal(3, order.GetNextIndex(2, 5, AdvanceReason.MediaEnded));
    }

    [Fact]
    public void ChangingRepeatOnLastTrack_TakesEffectImmediately()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.Off };
        Assert.Equal(PlaybackOrder.NoNextIndex, order.GetNextIndex(2, 3, AdvanceReason.MediaEnded));

        order.Repeat = RepeatMode.All;
        Assert.Equal(0, order.GetNextIndex(2, 3, AdvanceReason.MediaEnded));

        order.Repeat = RepeatMode.One;
        Assert.Equal(2, order.GetNextIndex(2, 3, AdvanceReason.MediaEnded));
    }

    [Fact]
    public void Shuffle_PlaylistGrowsOrShrinks_OnlyReturnsIndicesForNewSize()
    {
        var order = new PlaybackOrder(new Random(5)) { Shuffle = true, Repeat = RepeatMode.All };
        int index = order.GetNextIndex(0, 10, AdvanceReason.MediaEnded);

        // Playlist shrinks to 3 items, e.g. after removing tracks.
        for (int i = 0; i < 10; i++)
        {
            index = order.GetNextIndex(Math.Min(index, 2), 3, AdvanceReason.MediaEnded);
            Assert.InRange(index, 0, 2);
        }
    }
}
