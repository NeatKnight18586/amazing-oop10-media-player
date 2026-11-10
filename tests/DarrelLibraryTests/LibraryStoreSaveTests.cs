using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using mediaPlayerPrj;
using Xunit;

namespace DarrelLibraryTests;

/// <summary>
/// Unit tests for <see cref="LibraryStore.Save"/> covering UTF-8 persistence, six-field format, and I/O exceptions (FR3, FR7).
/// </summary>
public class LibraryStoreSaveTests : IDisposable
{
    private readonly string _tempDirectory;

    public LibraryStoreSaveTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "DarrelLibrarySaveTests_" + Guid.NewGuid().ToString("N"));
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
    public void Save_NullItems_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => LibraryStore.Save(null!, GetTempFilePath()));
    }

    [Fact]
    public void Save_NullWriter_ThrowsArgumentNullException()
    {
        var items = new List<MediaItem>();
        Assert.Throws<ArgumentNullException>(() => LibraryStore.Save((TextWriter)null!, items));
    }

    [Fact]
    public void Save_EmptyCollection_CreatesEmptyFile()
    {
        string path = GetTempFilePath();

        LibraryStore.Save(new List<MediaItem>(), path);

        Assert.True(File.Exists(path));
        Assert.Equal(0, new FileInfo(path).Length);
    }

    [Fact]
    public void Save_ValidItems_WritesUtf8SixFieldFormat()
    {
        string path = GetTempFilePath();
        var items = new List<MediaItem>
        {
            new AudioItem("Song 1", "Artist A", new TimeSpan(0, 3, 20), @"C:\Music\1.mp3", "Album X"),
            new VideoItem("Video 1", "Creator B", new TimeSpan(1, 10, 0), @"C:\Videos\1.mp4", "1920x1080")
        };

        LibraryStore.Save(items, path);

        string[] lines = File.ReadAllLines(path, Encoding.UTF8);
        Assert.Equal(2, lines.Length);
        Assert.Equal(@"Audio|Song 1|Artist A|00:03:20|C:\Music\1.mp3|Album X", lines[0]);
        Assert.Equal(@"Video|Video 1|Creator B|01:10:00|C:\Videos\1.mp4|1920x1080", lines[1]);
    }

    [Fact]
    public void Save_WritesUtf8WithoutBom()
    {
        string path = GetTempFilePath();
        var items = new List<MediaItem>
        {
            new AudioItem("Track", "Artist", new TimeSpan(0, 3, 0), "track.mp3", "-")
        };

        LibraryStore.Save(items, path);

        byte[] bytes = File.ReadAllBytes(path);
        Assert.True(bytes.Length >= 3);
        // UTF-8 BOM is 0xEF, 0xBB, 0xBF
        bool hasBom = bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF;
        Assert.False(hasBom, "Saved library file should not contain a UTF-8 BOM.");
    }

    [Fact]
    public void Save_DurationFormatting_IsCultureIndependent()
    {
        var originalCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        try
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr-FR");

            string path = GetTempFilePath();
            var items = new List<MediaItem>
            {
                new AudioItem("Song", "Artist", new TimeSpan(1, 23, 45), "song.mp3", "-")
            };

            LibraryStore.Save(items, path);

            string line = File.ReadAllLines(path, Encoding.UTF8)[0];
            Assert.Contains("|01:23:45|", line);
        }
        finally
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public void Save_ToTextWriter_WritesLinesCorrectly()
    {
        var items = new List<MediaItem>
        {
            new AudioItem("Track", "Singer", new TimeSpan(0, 2, 15), "track.mp3", "Single")
        };
        var sb = new StringBuilder();
        using (var writer = new StringWriter(sb))
        {
            LibraryStore.Save(writer, items);
        }

        string output = sb.ToString().TrimEnd();
        Assert.Equal("Audio|Track|Singer|00:02:15|track.mp3|Single", output);
    }

    [Fact]
    public void Save_SkipsNullElementsInCollection()
    {
        string path = GetTempFilePath();
        var items = new List<MediaItem?>
        {
            new AudioItem("Only Valid", "Artist", new TimeSpan(0, 1, 0), "valid.mp3", "-"),
            null
        };

        LibraryStore.Save(items!, path);

        string[] lines = File.ReadAllLines(path, Encoding.UTF8);
        Assert.Single(lines);
        Assert.StartsWith("Audio|Only Valid", lines[0]);
    }

    [Fact]
    public void Save_CreatesParentDirectoryIfMissing()
    {
        string nestedPath = Path.Combine(_tempDirectory, "new_folder", "nested_folder", "Library.txt");
        var items = new List<MediaItem>
        {
            new AudioItem("Nested", "Artist", new TimeSpan(0, 2, 0), "nested.mp3", "-")
        };

        LibraryStore.Save(items, nestedPath);

        Assert.True(File.Exists(nestedPath));
    }

    [Fact]
    public void Save_FileLockedExclusively_ThrowsIOException()
    {
        string path = GetTempFilePath();
        File.WriteAllText(path, "existing content");

        using var lockStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

        var items = new List<MediaItem>
        {
            new AudioItem("Locked", "Artist", new TimeSpan(0, 1, 0), "locked.mp3", "-")
        };

        Assert.Throws<IOException>(() => LibraryStore.Save(items, path));
    }

    [Fact]
    public void Save_TargetIsDirectory_ThrowsUnauthorizedAccessException()
    {
        var items = new List<MediaItem>();

        Assert.Throws<UnauthorizedAccessException>(() => LibraryStore.Save(items, _tempDirectory));
    }
}

