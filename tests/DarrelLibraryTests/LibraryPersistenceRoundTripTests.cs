using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using mediaPlayerPrj;
using Xunit;

namespace DarrelLibraryTests;

/// <summary>
/// Unit tests covering end-to-end save and load round trips for MP3 and MP4 media entries (FR3, FR7, TC06).
/// Verifies polymorphic deserialization, idempotency, international characters, and edge-case sanitization.
/// </summary>
public class LibraryPersistenceRoundTripTests : IDisposable
{
    private readonly string _tempDirectory;

    public LibraryPersistenceRoundTripTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "DarrelRoundTripTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempDirectory);
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, recursive: true);
            }
        }
        catch
        {
            // Best-effort cleanup
        }
    }

    private string GetTempFilePath() =>
        Path.Combine(_tempDirectory, Guid.NewGuid().ToString("N") + ".txt");

    [Fact]
    public void RoundTrip_SingleAudioItem_PreservesAllProperties()
    {
        string filePath = GetTempFilePath();
        var original = new AudioItem(
            title: "Hotel California",
            artist: "Eagles",
            duration: new TimeSpan(0, 6, 30),
            filePath: @"C:\Music\Eagles\hotel.mp3",
            album: "Hotel California (2013 Remaster)"
        );

        LibraryStore.Save(new[] { original }, filePath);
        var result = LibraryStore.Load(filePath);

        Assert.Single(result.Items);
        Assert.Equal(0, result.SkippedLineCount);
        Assert.Equal(1, result.TotalLineCount);

        var loaded = Assert.IsType<AudioItem>(result.Items[0]);
        Assert.Equal(original.Title, loaded.Title);
        Assert.Equal(original.Artist, loaded.Artist);
        Assert.Equal(original.Duration, loaded.Duration);
        Assert.Equal(original.FilePath, loaded.FilePath);
        Assert.Equal(original.Album, loaded.Album);
        Assert.Equal(original.Extra, loaded.Extra);
        Assert.Equal("Audio", loaded.MediaType);
    }

    [Fact]
    public void RoundTrip_SingleVideoItem_PreservesAllProperties()
    {
        string filePath = GetTempFilePath();
        var original = new VideoItem(
            title: "Planet Earth II",
            artist: "David Attenborough",
            duration: new TimeSpan(0, 58, 42),
            filePath: @"D:\Documentaries\earth.mp4",
            resolution: "3840x2160"
        );

        LibraryStore.Save(new[] { original }, filePath);
        var result = LibraryStore.Load(filePath);

        Assert.Single(result.Items);
        Assert.Equal(0, result.SkippedLineCount);
        Assert.Equal(1, result.TotalLineCount);

        var loaded = Assert.IsType<VideoItem>(result.Items[0]);
        Assert.Equal(original.Title, loaded.Title);
        Assert.Equal(original.Artist, loaded.Artist);
        Assert.Equal(original.Duration, loaded.Duration);
        Assert.Equal(original.FilePath, loaded.FilePath);
        Assert.Equal(original.Resolution, loaded.Resolution);
        Assert.Equal(original.Extra, loaded.Extra);
        Assert.Equal("Video", loaded.MediaType);
    }

    [Fact]
    public void RoundTrip_MixedPlaylist_PreservesOrderAndPolymorphicSubclasses()
    {
        string filePath = GetTempFilePath();
        var playlist = new List<MediaItem>
        {
            new AudioItem("Track 1", "Artist A", new TimeSpan(0, 3, 12), @"C:\media\1.mp3", "Album 1"),
            new VideoItem("Video 2", "Creator B", new TimeSpan(0, 12, 45), @"C:\media\2.mp4", "1920x1080"),
            new AudioItem("Track 3", "Artist C", new TimeSpan(0, 4, 50), @"C:\media\3.mp3", "-"),
            new VideoItem("Video 4", "Creator D", new TimeSpan(1, 45, 10), @"C:\media\4.mp4", "1280x720")
        };

        LibraryStore.Save(playlist, filePath);
        var result = LibraryStore.Load(filePath);

        Assert.Equal(4, result.Items.Count);
        Assert.Equal(0, result.SkippedLineCount);
        Assert.Equal(4, result.TotalLineCount);

        Assert.IsType<AudioItem>(result.Items[0]);
        Assert.Equal("Track 1", result.Items[0].Title);

        Assert.IsType<VideoItem>(result.Items[1]);
        Assert.Equal("Video 2", result.Items[1].Title);
        Assert.Equal("1920x1080", result.Items[1].Extra);

        Assert.IsType<AudioItem>(result.Items[2]);
        Assert.Equal("Track 3", result.Items[2].Title);
        Assert.Equal("-", result.Items[2].Extra);

        Assert.IsType<VideoItem>(result.Items[3]);
        Assert.Equal("Video 4", result.Items[3].Title);
    }

    [Fact]
    public void RoundTrip_MultipleSaveLoadCycles_IsIdempotent()
    {
        string path1 = GetTempFilePath();
        string path2 = GetTempFilePath();

        var initialList = new List<MediaItem>
        {
            new AudioItem("Song", "Band", new TimeSpan(0, 4, 15), @"C:\test.mp3", "Album"),
            new VideoItem("Film", "Director", new TimeSpan(2, 5, 30), @"C:\film.mp4", "1920x1080")
        };

        // First cycle
        LibraryStore.Save(initialList, path1);
        var loaded1 = LibraryStore.Load(path1);

        // Second cycle
        LibraryStore.Save(loaded1.Items, path2);
        var loaded2 = LibraryStore.Load(path2);

        byte[] bytes1 = File.ReadAllBytes(path1);
        byte[] bytes2 = File.ReadAllBytes(path2);

        Assert.Equal(bytes1, bytes2);
        Assert.Equal(loaded1.Items.Count, loaded2.Items.Count);
        for (int i = 0; i < loaded1.Items.Count; i++)
        {
            Assert.Equal(loaded1.Items[i].ToFileLine(), loaded2.Items[i].ToFileLine());
        }
    }

    [Fact]
    public void RoundTrip_InternationalUtf8Characters_PreservesFidelity()
    {
        string filePath = GetTempFilePath();
        var playlist = new List<MediaItem>
        {
            new AudioItem("夜に駆ける", "YOASOBI", new TimeSpan(0, 4, 21), @"C:\Music\yoasobi.mp3", "THE BOOK"),
            new AudioItem("Canción de Cuna", "Mercedes Sosa", new TimeSpan(0, 3, 50), @"C:\Music\sosa.mp3", "Homenaje"),
            new VideoItem("강남스타일", "PSY", new TimeSpan(0, 4, 13), @"C:\Videos\psy.mp4", "3840x2160")
        };

        LibraryStore.Save(playlist, filePath);
        var result = LibraryStore.Load(filePath);

        Assert.Equal(3, result.Items.Count);
        Assert.Equal(0, result.SkippedLineCount);

        Assert.Equal("夜に駆ける", result.Items[0].Title);
        Assert.Equal("YOASOBI", result.Items[0].Artist);

        Assert.Equal("Canción de Cuna", result.Items[1].Title);
        Assert.Equal("Mercedes Sosa", result.Items[1].Artist);

        Assert.Equal("강남스타일", result.Items[2].Title);
        Assert.Equal("PSY", result.Items[2].Artist);
    }

    [Fact]
    public void RoundTrip_MetadataWithPipes_SanitizedWithoutCorruptingFieldLayout()
    {
        string filePath = GetTempFilePath();
        var original = new AudioItem(
            title: "Track | Special | Version",
            artist: "Artist | Band",
            duration: new TimeSpan(0, 5, 0),
            filePath: @"C:\Music|Special\file.mp3",
            album: "Album | Deluxe"
        );

        LibraryStore.Save(new[] { original }, filePath);
        var result = LibraryStore.Load(filePath);

        Assert.Single(result.Items);
        Assert.Equal(0, result.SkippedLineCount);

        var loaded = Assert.IsType<AudioItem>(result.Items[0]);
        // Embedded pipes are replaced with slashes by ToFileLine to prevent delimiter collision
        Assert.Equal("Track / Special / Version", loaded.Title);
        Assert.Equal("Artist / Band", loaded.Artist);
        Assert.Equal(@"C:\Music/Special\file.mp3", loaded.FilePath);
        Assert.Equal("Album / Deluxe", loaded.Album);
    }

    [Fact]
    public void RoundTrip_LongDurationsExceeding24Hours_PreservesTotalHours()
    {
        string filePath = GetTempFilePath();
        var original = new VideoItem(
            title: "Long Livestream Archive",
            artist: "Streamer",
            duration: new TimeSpan(36, 15, 45),
            filePath: @"D:\Streams\archive.mp4",
            resolution: "1920x1080"
        );

        LibraryStore.Save(new[] { original }, filePath);
        var result = LibraryStore.Load(filePath);

        Assert.Single(result.Items);
        var loaded = Assert.IsType<VideoItem>(result.Items[0]);
        Assert.Equal(new TimeSpan(36, 15, 45), loaded.Duration);
        Assert.Equal("36:15:45", loaded.DurationText);
    }
}

