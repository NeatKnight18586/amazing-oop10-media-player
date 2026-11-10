using System;
using mediaPlayerPrj;
using Xunit;

namespace DarrelLibraryTests;

/// <summary>
/// Unit tests for invariant validation and string serialization on <see cref="MediaItem"/>.
/// </summary>
public class MediaItemTests
{
    private sealed class ConcreteMediaItem : MediaItem
    {
        public ConcreteMediaItem(string title, string artist, TimeSpan duration, string filePath, string extra = "TestExtra")
            : base(title, artist, duration, filePath)
        {
            ExtraValue = extra;
        }

        public string ExtraValue { get; set; }
        public override string MediaType => "Audio";
        public override string Extra => ExtraValue;
    }

    [Fact]
    public void Constructor_WithValidData_SetsPropertiesTrimmed()
    {
        var item = new ConcreteMediaItem("  Song Title  ", "  Band Name  ", TimeSpan.FromMinutes(3.5), @" C:\Music\song.mp3 ");

        Assert.Equal("Song Title", item.Title);
        Assert.Equal("Band Name", item.Artist);
        Assert.Equal(TimeSpan.FromMinutes(3.5), item.Duration);
        Assert.Equal(@"C:\Music\song.mp3", item.FilePath);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Title_WhenNullOrWhitespace_DefaultsToUnknownTitle(string? title)
    {
        var item = new ConcreteMediaItem(title!, "Artist", TimeSpan.FromSeconds(30), "file.mp3");
        Assert.Equal("Unknown Title", item.Title);

        item.Title = title!;
        Assert.Equal("Unknown Title", item.Title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Artist_WhenNullOrWhitespace_DefaultsToUnknownArtist(string? artist)
    {
        var item = new ConcreteMediaItem("Title", artist!, TimeSpan.FromSeconds(30), "file.mp3");
        Assert.Equal("Unknown Artist", item.Artist);

        item.Artist = artist!;
        Assert.Equal("Unknown Artist", item.Artist);
    }

    [Fact]
    public void Duration_WhenNegative_ClampedToZero()
    {
        var item = new ConcreteMediaItem("Title", "Artist", TimeSpan.FromSeconds(-45), "file.mp3");
        Assert.Equal(TimeSpan.Zero, item.Duration);

        item.Duration = TimeSpan.FromMinutes(-5);
        Assert.Equal(TimeSpan.Zero, item.Duration);
    }

    [Fact]
    public void FilePath_WhenNull_DefaultsToEmpty()
    {
        var item = new ConcreteMediaItem("Title", "Artist", TimeSpan.FromSeconds(10), null!);
        Assert.Equal(string.Empty, item.FilePath);

        item.FilePath = null!;
        Assert.Equal(string.Empty, item.FilePath);
    }

    [Fact]
    public void DurationText_FormatsHoursMinutesSecondsCorrectly()
    {
        var shortTrack = new ConcreteMediaItem("Short", "Artist", new TimeSpan(0, 3, 25), "path");
        Assert.Equal("00:03:25", shortTrack.DurationText);

        var longTrack = new ConcreteMediaItem("Long", "Artist", new TimeSpan(2, 14, 9), "path");
        Assert.Equal("02:14:09", longTrack.DurationText);
    }

    [Fact]
    public void ToFileLine_CleansEmbeddedPipes_AndFormatsSixFields()
    {
        var item = new ConcreteMediaItem("Track | Remaster", "Artist | Band", TimeSpan.FromMinutes(4), @"C:\Path|Dir\file.mp3", "Album | Bonus");
        var line = item.ToFileLine();

        var parts = line.Split('|');
        Assert.Equal(6, parts.Length);
        Assert.Equal("Audio", parts[0]);
        Assert.Equal("Track / Remaster", parts[1]);
        Assert.Equal("Artist / Band", parts[2]);
        Assert.Equal("00:04:00", parts[3]);
        Assert.Equal(@"C:\Path/Dir\file.mp3", parts[4]);
        Assert.Equal("Album / Bonus", parts[5]);
    }

    [Fact]
    public void ToString_ReturnsArtistAndTitle()
    {
        var item = new ConcreteMediaItem("Song", "Band", TimeSpan.Zero, "path");
        Assert.Equal("Band - Song", item.ToString());
    }
}

