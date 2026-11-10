using mediaPlayerPrj;
using Xunit;

namespace PlaybackModes.Tests;

public class PlaybackOrderTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(3, 4)]
    public void GetNextIndex_MovesToFollowingItem(int current, int expected)
    {
        var order = new PlaybackOrder();
        Assert.Equal(expected, order.GetNextIndex(current, 5, AdvanceReason.MediaEnded));
    }

    [Fact]
    public void GetNextIndex_NothingSelected_StartsAtFirstItem()
    {
        var order = new PlaybackOrder();
        Assert.Equal(0, order.GetNextIndex(-1, 5, AdvanceReason.UserNext));
    }

    [Fact]
    public void GetNextIndex_WalksWholePlaylistInOrder()
    {
        var order = new PlaybackOrder();
        var visited = new List<int>();
        int index = -1;

        while (true)
        {
            index = order.GetNextIndex(index, 4, AdvanceReason.MediaEnded);
            if (index == PlaybackOrder.NoNextIndex) break;
            visited.Add(index);
        }

        Assert.Equal(new[] { 0, 1, 2, 3 }, visited);
    }

    [Fact]
    public void GetNextIndex_AfterLastItem_ReturnsNoNextIndex()
    {
        var order = new PlaybackOrder();
        Assert.Equal(PlaybackOrder.NoNextIndex, order.GetNextIndex(4, 5, AdvanceReason.MediaEnded));
    }
}
