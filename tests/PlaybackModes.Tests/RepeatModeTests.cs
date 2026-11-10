using mediaPlayerPrj;
using Xunit;

namespace PlaybackModes.Tests;

public class RepeatModeTests
{
    [Theory]
    [InlineData(RepeatMode.Off, RepeatMode.All)]
    [InlineData(RepeatMode.All, RepeatMode.One)]
    [InlineData(RepeatMode.One, RepeatMode.Off)]
    public void NextMode_CyclesOffAllOne(RepeatMode current, RepeatMode expected)
    {
        Assert.Equal(expected, current.NextMode());
    }

    [Fact]
    public void NextMode_ThreeClicksReturnToStart()
    {
        RepeatMode mode = RepeatMode.Off;
        mode = mode.NextMode().NextMode().NextMode();
        Assert.Equal(RepeatMode.Off, mode);
    }

    [Theory]
    [InlineData(RepeatMode.Off, "Repeat: Off")]
    [InlineData(RepeatMode.All, "Repeat: All")]
    [InlineData(RepeatMode.One, "Repeat: One")]
    public void ToDisplayText_ReturnsReadableLabel(RepeatMode mode, string expected)
    {
        Assert.Equal(expected, mode.ToDisplayText());
    }

    [Fact]
    public void DefaultValue_IsOff()
    {
        Assert.Equal(RepeatMode.Off, default(RepeatMode));
    }
}
