using mediaPlayerPrj;
using Xunit;

namespace PlaybackModes.Tests;

/// <summary>TC22: Repeat All from the last track wraps and plays the first.</summary>
public class RepeatAllTests
{
    [Fact]
    public void RepeatAll_LastItemEnds_WrapsToFirst()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.All };
        Assert.Equal(0, order.GetNextIndex(2, 3, AdvanceReason.MediaEnded));
    }

    [Fact]
    public void RepeatAll_MiddleItemEnds_ContinuesToNext()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.All };
        Assert.Equal(1, order.GetNextIndex(0, 3, AdvanceReason.MediaEnded));
    }

    [Fact]
    public void RepeatAll_PlaysPlaylistTwiceWithoutStopping()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.All };
        var visited = new List<int>();
        int index = 0;
        visited.Add(index);

        for (int i = 0; i < 5; i++)
        {
            index = order.GetNextIndex(index, 3, AdvanceReason.MediaEnded);
            visited.Add(index);
        }

        Assert.Equal(new[] { 0, 1, 2, 0, 1, 2 }, visited);
    }
}
