using mediaPlayerPrj;
using Xunit;

namespace PlaybackModes.Tests;

/// <summary>TC24: Shuffle on with several items picks valid tracks and obeys the policy.</summary>
public class ShuffleTests
{
    [Fact]
    public void Shuffle_IsOffByDefault()
    {
        Assert.False(new PlaybackOrder().Shuffle);
    }

    [Fact]
    public void Shuffle_NextIndexIsValidAndNotTheCurrentTrack()
    {
        var order = new PlaybackOrder(new Random(1234)) { Shuffle = true };
        for (int current = 0; current < 6; current++)
        {
            order.ResetShuffle();
            int next = order.GetNextIndex(current, 6, AdvanceReason.UserNext);
            Assert.InRange(next, 0, 5);
            Assert.NotEqual(current, next);
        }
    }

    [Fact]
    public void Shuffle_PlaysEveryTrackOnceBeforeStopping_WhenRepeatOff()
    {
        var order = new PlaybackOrder(new Random(7)) { Shuffle = true, Repeat = RepeatMode.Off };
        var played = new List<int> { 0 };
        int index = 0;

        while (true)
        {
            index = order.GetNextIndex(index, 5, AdvanceReason.MediaEnded);
            if (index == PlaybackOrder.NoNextIndex) break;
            played.Add(index);
            Assert.True(played.Count <= 5, "Shuffle repeated a track before finishing the round.");
        }

        Assert.Equal(new[] { 0, 1, 2, 3, 4 }, played.OrderBy(i => i).ToArray());
    }

    [Fact]
    public void Shuffle_SameSeed_GivesSameOrder()
    {
        List<int> Run()
        {
            var order = new PlaybackOrder(new Random(99)) { Shuffle = true };
            var result = new List<int>();
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                index = order.GetNextIndex(index, 5, AdvanceReason.MediaEnded);
                result.Add(index);
            }
            return result;
        }

        Assert.Equal(Run(), Run());
    }
}
