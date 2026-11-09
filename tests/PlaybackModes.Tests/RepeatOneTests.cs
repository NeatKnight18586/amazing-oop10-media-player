using mediaPlayerPrj;
using Xunit;

namespace PlaybackModes.Tests;

/// <summary>TC23: Repeat One restarts the same track.</summary>
public class RepeatOneTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void RepeatOne_TrackEnds_ReplaysSameTrack(int current)
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.One };
        Assert.Equal(current, order.GetNextIndex(current, 3, AdvanceReason.MediaEnded));
    }

    [Fact]
    public void RepeatOne_UserPressesNext_SkipsToFollowingTrack()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.One };
        Assert.Equal(2, order.GetNextIndex(1, 3, AdvanceReason.UserNext));
    }

    [Fact]
    public void RepeatOne_UserPressesNextOnLastTrack_WrapsToFirst()
    {
        var order = new PlaybackOrder { Repeat = RepeatMode.One };
        Assert.Equal(0, order.GetNextIndex(2, 3, AdvanceReason.UserNext));
    }
}
