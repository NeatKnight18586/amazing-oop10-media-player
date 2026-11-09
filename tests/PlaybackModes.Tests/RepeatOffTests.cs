using mediaPlayerPrj;
using Xunit;

namespace PlaybackModes.Tests;

/// <summary>TC21: Repeat Off from the last track stops playback.</summary>
public class RepeatOffTests
{
    [Fact]
    public void NewPlaybackOrder_DefaultsToRepeatOff()
    {
        Assert.Equal(RepeatMode.Off, new PlaybackOrder().Repeat);
    }

    [Fact]
    public void RepeatOff_LastItemEnds_Stops()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.Off };
        Assert.Equal(PlaybackOrder.NoNextIndex, order.GetNextIndex(2, 3, AdvanceReason.MediaEnded));
    }

    [Fact]
    public void RepeatOff_MiddleItemEnds_ContinuesToNext()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.Off };
        Assert.Equal(2, order.GetNextIndex(1, 3, AdvanceReason.MediaEnded));
    }

    [Fact]
    public void RepeatOff_UserPressesNextOnLastItem_WrapsToFirst()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.Off };
        Assert.Equal(0, order.GetNextIndex(2, 3, AdvanceReason.UserNext));
    }
}
