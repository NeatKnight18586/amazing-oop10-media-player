using mediaPlayerPrj;
using Xunit;

namespace PlaybackModes.Tests;

/// <summary>TC25 and boundary cases: empty, single-item and stale-index playlists.</summary>
public class PlaylistEdgeTests
{
    public static IEnumerable<object[]> AllModes()
    {
        foreach (bool shuffle in new[] { false, true })
            foreach (RepeatMode repeat in Enum.GetValues<RepeatMode>())
                foreach (AdvanceReason reason in Enum.GetValues<AdvanceReason>())
                    yield return new object[] { shuffle, repeat, reason };
    }

    [Theory]
    [MemberData(nameof(AllModes))]
    public void EmptyPlaylist_AlwaysReturnsNoNextIndex(bool shuffle, RepeatMode repeat, AdvanceReason reason)
    {
        var order = new PlaybackOrder(new Random(1)) { Shuffle = shuffle, Repeat = repeat };
        Assert.Equal(PlaybackOrder.NoNextIndex, order.GetNextIndex(-1, 0, reason));
        Assert.Equal(PlaybackOrder.NoNextIndex, order.GetNextIndex(0, 0, reason));
    }

    [Theory]
    [MemberData(nameof(AllModes))]
    public void SingleItem_NeverCrashesAndReturnsValidResult(bool shuffle, RepeatMode repeat, AdvanceReason reason)
    {
        var order = new PlaybackOrder(new Random(1)) { Shuffle = shuffle, Repeat = repeat };
        int next = order.GetNextIndex(0, 1, reason);
        Assert.True(next == 0 || next == PlaybackOrder.NoNextIndex);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SingleItem_RepeatOffEnds_Stops(bool shuffle)
    {
        var order = new PlaybackOrder(new Random(1)) { Shuffle = shuffle, Repeat = RepeatMode.Off };
        Assert.Equal(PlaybackOrder.NoNextIndex, order.GetNextIndex(0, 1, AdvanceReason.MediaEnded));
    }

    [Theory]
    [InlineData(false, RepeatMode.All)]
    [InlineData(true, RepeatMode.All)]
    [InlineData(false, RepeatMode.One)]
    [InlineData(true, RepeatMode.One)]
    public void SingleItem_RepeatAllOrOneEnds_ReplaysIt(bool shuffle, RepeatMode repeat)
    {
        var order = new PlaybackOrder(new Random(1)) { Shuffle = shuffle, Repeat = repeat };
        Assert.Equal(0, order.GetNextIndex(0, 1, AdvanceReason.MediaEnded));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SingleItem_UserPressesNext_ReplaysIt(bool shuffle)
    {
        var order = new PlaybackOrder(new Random(1)) { Shuffle = shuffle };
        Assert.Equal(0, order.GetNextIndex(0, 1, AdvanceReason.UserNext));
    }

    [Theory]
    [MemberData(nameof(AllModes))]
    public void StaleIndexAfterPlaylistShrinks_ReturnsValidIndexOrStop(bool shuffle, RepeatMode repeat, AdvanceReason reason)
    {
        var order = new PlaybackOrder(new Random(1)) { Shuffle = shuffle, Repeat = repeat };
        int next = order.GetNextIndex(7, 3, reason);
        Assert.True(next == PlaybackOrder.NoNextIndex || (next >= 0 && next < 3), $"Got invalid index {next}");
    }
}
