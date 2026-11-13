using System;
using mediaPlayerPrj;
using Xunit;

namespace DarrelLibraryTests;

/// <summary>
/// Unit tests for parsing six-field library records in <see cref="LibraryStore.ParseLine"/>.
/// </summary>
public class LibraryStoreParserTests
{
    [Fact]
    public void ParseLine_ValidAudioRecord_ReturnsAudioItem()
    {
        string line = @"Audio|Bohemian Rhapsody|Queen|00:05:55|C:\Music\queen.mp3|A Night at the Opera";
        var item = LibraryStore.ParseLine(line);

        Assert.NotNull(item);
        Assert.IsType<AudioItem>(item);
        var audio = (AudioItem)item;
        Assert.Equal("Bohemian Rhapsody", audio.Title);
        Assert.Equal("Queen", audio.Artist);
        Assert.Equal(new TimeSpan(0, 5, 55), audio.Duration);
        Assert.Equal(@"C:\Music\queen.mp3", audio.FilePath);
        Assert.Equal("A Night at the Opera", audio.Album);
        Assert.Equal("Audio", audio.MediaType);
    }

    [Fact]
    public void ParseLine_ValidVideoRecord_ReturnsVideoItem()
    {
        string line = @"Video|Nature Documentary|BBC|01:30:15|C:\Videos\nature.mp4|1920x1080";
        var item = LibraryStore.ParseLine(line);

        Assert.NotNull(item);
        Assert.IsType<VideoItem>(item);
        var video = (VideoItem)item;
        Assert.Equal("Nature Documentary", video.Title);
        Assert.Equal("BBC", video.Artist);
        Assert.Equal(new TimeSpan(1, 30, 15), video.Duration);
        Assert.Equal(@"C:\Videos\nature.mp4", video.FilePath);
        Assert.Equal("1920x1080", video.Resolution);
        Assert.Equal("Video", video.MediaType);
    }

    [Theory]
    [InlineData("audio")]
    [InlineData("AUDIO")]
    [InlineData("Audio")]
    [InlineData("video")]
    [InlineData("VIDEO")]
    public void ParseLine_MediaTypeIsCaseInsensitive(string mediaType)
    {
        string line = $"{mediaType}|Sample|Artist|00:02:00|C:\\file.mp3|-";
        var item = LibraryStore.ParseLine(line);

        Assert.NotNull(item);
        if (mediaType.StartsWith("a", StringComparison.OrdinalIgnoreCase))
        {
            Assert.IsType<AudioItem>(item);
        }
        else
        {
            Assert.IsType<VideoItem>(item);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void ParseLine_NullOrWhitespace_ReturnsNull(string? line)
    {
        var item = LibraryStore.ParseLine(line);
        Assert.Null(item);
    }

    [Theory]
    [InlineData("Audio|Song|Artist|00:03:00|C:\\path")]
    [InlineData("Audio|Song|Artist|00:03:00")]
    [InlineData("Audio|Song|Artist|00:03:00|C:\\path|Album|ExtraField")]
    public void ParseLine_InvalidFieldCount_ReturnsNull(string line)
    {
        var item = LibraryStore.ParseLine(line);
        Assert.Null(item);
    }

    [Theory]
    [InlineData("Podcast|Episode 1|Host|00:45:00|path.mp3|-")]
    [InlineData("PDF|Manual|Author|00:00:00|manual.pdf|-")]
    [InlineData("123|Title|Artist|00:01:00|file.mp3|-")]
    public void ParseLine_UnsupportedMediaType_ReturnsNull(string line)
    {
        var item = LibraryStore.ParseLine(line);
        Assert.Null(item);
    }

    [Theory]
    [InlineData("not-a-duration")]
    [InlineData("00:65:00")]
    [InlineData("00:00:75")]
    [InlineData("-01:00:00")]
    public void ParseLine_InvalidDuration_ReturnsNull(string invalidDuration)
    {
        string line = $"Audio|Song|Artist|{invalidDuration}|path.mp3|Album";
        var item = LibraryStore.ParseLine(line);
        Assert.Null(item);
    }

    [Fact]
    public void TryParseLine_ReturnsTrueForValidAndFalseForInvalid()
    {
        bool success = LibraryStore.TryParseLine(@"Audio|Track|Artist|00:03:00|C:\path.mp3|Album", out var validItem);
        Assert.True(success);
        Assert.NotNull(validItem);

        bool fail = LibraryStore.TryParseLine("corrupt|row", out var invalidItem);
        Assert.False(fail);
        Assert.Null(invalidItem);
    }

    // Checkpoint 10: Additional parser edge-case tests

    [Fact]
    public void ParseLine_ZeroDuration_ParsesAsTimeSpanZero()
    {
        string line = @"Audio|Silent Track|Artist|00:00:00|C:\Music\silent.mp3|Single";
        var item = LibraryStore.ParseLine(line);

        Assert.NotNull(item);
        Assert.IsType<AudioItem>(item);
        Assert.Equal(TimeSpan.Zero, item.Duration);
        Assert.Equal("Silent Track", item.Title);
    }

    [Fact]
    public void ParseLine_WhitespaceTitle_NormalizesToUnknownTitle()
    {
        string line = @"Audio|   |Artist|00:03:00|C:\Music\track.mp3|Album";
        var item = LibraryStore.ParseLine(line);

        Assert.NotNull(item);
        Assert.Equal("Unknown Title", item!.Title);
    }

    [Fact]
    public void ParseLine_ValidDurationMM_SS_ParsesCorrectly()
    {
        string line = @"Audio|Short Track|Artist|03:45|C:\Music\short.mp3|EP";
        var item = LibraryStore.ParseLine(line);

        Assert.NotNull(item);
        Assert.IsType<AudioItem>(item);
        Assert.Equal(new TimeSpan(0, 3, 45), item!.Duration);
    }
}
