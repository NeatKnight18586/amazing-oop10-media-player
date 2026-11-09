using System;
using System.Collections.Generic;
using mediaPlayerPrj;
using Xunit;

namespace DarrelLibraryTests;

/// <summary>
/// Unit tests for <see cref="AudioItem"/> and <see cref="VideoItem"/> subclasses and polymorphism.
/// </summary>
public class MediaSubclassesTests
{
    [Fact]
    public void AudioItem_WithValidAlbum_SetsAlbumTrimmed()
    {
        var audio = new AudioItem("Song", "Artist", TimeSpan.FromMinutes(3), "song.mp3", "  Greatest Hits  ");
        Assert.Equal("Greatest Hits", audio.Album);
        Assert.Equal("Greatest Hits", audio.Extra);
        Assert.Equal("Audio", audio.MediaType);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AudioItem_WithNullOrWhitespaceAlbum_DefaultsToDash(string? album)
    {
        var audio = new AudioItem("Song", "Artist", TimeSpan.FromMinutes(3), "song.mp3", album);
        Assert.Equal("-", audio.Album);
        Assert.Equal("-", audio.Extra);

        audio.Album = album!;
        Assert.Equal("-", audio.Album);
    }

    [Fact]
    public void AudioItem_ToFileLine_SerializesAudioAndAlbum()
    {
        var audio = new AudioItem("Song", "Artist", new TimeSpan(0, 3, 45), @"C:\Music\song.mp3", "Album Name");
        var line = audio.ToFileLine();

        Assert.Equal(@"Audio|Song|Artist|00:03:45|C:\Music\song.mp3|Album Name", line);
    }

    [Fact]
    public void VideoItem_WithValidResolution_SetsResolutionTrimmed()
    {
        var video = new VideoItem("Movie", "Director", TimeSpan.FromHours(1.5), "movie.mp4", "  1920x1080  ");
        Assert.Equal("1920x1080", video.Resolution);
        Assert.Equal("1920x1080", video.Extra);
        Assert.Equal("Video", video.MediaType);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void VideoItem_WithNullOrWhitespaceResolution_DefaultsToUnknown(string? resolution)
    {
        var video = new VideoItem("Movie", "Director", TimeSpan.FromHours(1.5), "movie.mp4", resolution);
        Assert.Equal("Unknown", video.Resolution);
        Assert.Equal("Unknown", video.Extra);

        video.Resolution = resolution!;
        Assert.Equal("Unknown", video.Resolution);
    }

    [Fact]
    public void VideoItem_ToFileLine_SerializesVideoAndResolution()
    {
        var video = new VideoItem("Clip", "Editor", new TimeSpan(0, 1, 30), @"C:\Videos\clip.mp4", "1280x720");
        var line = video.ToFileLine();

        Assert.Equal(@"Video|Clip|Editor|00:01:30|C:\Videos\clip.mp4|1280x720", line);
    }

    [Fact]
    public void PolymorphicList_DispatchesMediaTypeAndExtraCorrectly()
    {
        var playlist = new List<MediaItem>
        {
            new AudioItem("Audio Track", "Artist A", TimeSpan.FromMinutes(2), "track.mp3", "Rock Album"),
            new VideoItem("Video Clip", "Creator B", TimeSpan.FromMinutes(5), "clip.mp4", "3840x2160")
        };

        Assert.Equal("Audio", playlist[0].MediaType);
        Assert.Equal("Rock Album", playlist[0].Extra);
        Assert.Equal("Video", playlist[1].MediaType);
        Assert.Equal("3840x2160", playlist[1].Extra);

        Assert.StartsWith("Audio|Audio Track", playlist[0].ToFileLine());
        Assert.StartsWith("Video|Video Clip", playlist[1].ToFileLine());
    }
}

