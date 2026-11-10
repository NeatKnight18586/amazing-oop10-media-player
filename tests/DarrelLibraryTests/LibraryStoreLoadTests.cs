using System;
using System.IO;
using mediaPlayerPrj;
using Xunit;

namespace DarrelLibraryTests;

/// <summary>
/// Unit tests for <see cref="LibraryStore.Load"/> covering file recovery, first-run safety, and skipped corrupt rows (FR3, FR7).
/// </summary>
public class LibraryStoreLoadTests : IDisposable
{
    private readonly string _tempDirectory;

    public LibraryStoreLoadTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "DarrelLibraryTests_" + Guid.NewGuid().ToString("N"));
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

    private string CreateTempFile(string content)
    {
        string filePath = Path.Combine(_tempDirectory, Guid.NewGuid().ToString("N") + ".txt");
        File.WriteAllText(filePath, content);
        return filePath;
    }

    [Fact]
    public void Load_MissingFile_ReturnsEmptyResultWithoutThrowing()
    {
        string nonExistentPath = Path.Combine(_tempDirectory, "does_not_exist.txt");

        var result = LibraryStore.Load(nonExistentPath);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.SkippedLineCount);
        Assert.Equal(0, result.TotalLineCount);
    }

    [Fact]
    public void Load_MissingFile_OutOverload_ReturnsEmptyListAndZeroSkipped()
    {
        string nonExistentPath = Path.Combine(_tempDirectory, "missing.txt");

        var items = LibraryStore.Load(nonExistentPath, out int skipped);

        Assert.NotNull(items);
        Assert.Empty(items);
        Assert.Equal(0, skipped);
    }

    [Fact]
    public void Load_EmptyFile_ReturnsEmptyResult()
    {
        string emptyPath = CreateTempFile(string.Empty);

        var result = LibraryStore.Load(emptyPath);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.SkippedLineCount);
        Assert.Equal(0, result.TotalLineCount);
    }

    [Fact]
    public void Load_ValidRecords_LoadsAllItemsPolymorphically()
    {
        string content = string.Join(Environment.NewLine,
            @"Audio|Song One|Artist A|00:03:30|C:\Music\one.mp3|Album A",
            @"Video|Clip One|Creator B|00:15:00|C:\Videos\clip.mp4|1920x1080",
            @"Audio|Song Two|Artist C|01:02:10|C:\Music\two.mp3|Album B");
        string filePath = CreateTempFile(content);

        var result = LibraryStore.Load(filePath);

        Assert.Equal(3, result.Items.Count);
        Assert.Equal(0, result.SkippedLineCount);
        Assert.Equal(3, result.TotalLineCount);

        Assert.IsType<AudioItem>(result.Items[0]);
        Assert.Equal("Song One", result.Items[0].Title);
        Assert.Equal("Artist A", result.Items[0].Artist);

        Assert.IsType<VideoItem>(result.Items[1]);
        Assert.Equal("Clip One", result.Items[1].Title);
        Assert.Equal("1920x1080", result.Items[1].Extra);

        Assert.IsType<AudioItem>(result.Items[2]);
        Assert.Equal("Song Two", result.Items[2].Title);
    }

    [Fact]
    public void Load_CorruptedLinesMixedWithValid_RecoversValidTracksAndCountsSkipped()
    {
        string content = string.Join(Environment.NewLine,
            @"Audio|Song One|Artist A|00:03:30|C:\Music\one.mp3|Album A",
            @"CorruptedRowWithoutPipes",
            @"Audio|MissingFields|Artist",
            @"Video|Valid Video|Creator B|00:05:00|C:\Videos\clip.mp4|1280x720",
            @"Audio|InvalidDuration|Artist|not-a-time|C:\Music\bad.mp3|Album",
            @"UnsupportedType|Title|Artist|00:01:00|file.wav|-",
            @"Audio|Song Two|Artist C|00:04:00|C:\Music\two.mp3|Album C");
        string filePath = CreateTempFile(content);

        var result = LibraryStore.Load(filePath);

        Assert.Equal(3, result.Items.Count);
        Assert.Equal(4, result.SkippedLineCount);
        Assert.Equal(7, result.TotalLineCount);

        Assert.Equal("Song One", result.Items[0].Title);
        Assert.Equal("Valid Video", result.Items[1].Title);
        Assert.Equal("Song Two", result.Items[2].Title);
    }

    [Fact]
    public void Load_AllCorruptedLines_ReturnsEmptyListAndAccurateSkippedCount()
    {
        string content = string.Join(Environment.NewLine,
            "corrupt 1",
            "corrupt 2",
            "Audio|incomplete");
        string filePath = CreateTempFile(content);

        var result = LibraryStore.Load(filePath);

        Assert.Empty(result.Items);
        Assert.Equal(3, result.SkippedLineCount);
        Assert.Equal(3, result.TotalLineCount);
    }

    [Fact]
    public void Load_OutOverload_MatchesResult()
    {
        string content = string.Join(Environment.NewLine,
            @"Audio|Track|Artist|00:02:30|C:\path.mp3|Album",
            @"bad|line");
        string filePath = CreateTempFile(content);

        var items = LibraryStore.Load(filePath, out int skipped);

        Assert.Single(items);
        Assert.Equal("Track", items[0].Title);
        Assert.Equal(1, skipped);
    }

    [Fact]
    public void Load_FromTextReader_ProcessesStreamCorrectly()
    {
        string content = @"Audio|From Stream|Singer|00:01:45|D:\stream.mp3|Single";
        using var reader = new StringReader(content);

        var result = LibraryStore.Load(reader);

        Assert.Single(result.Items);
        Assert.Equal("From Stream", result.Items[0].Title);
        Assert.Equal(0, result.SkippedLineCount);
        Assert.Equal(1, result.TotalLineCount);
    }

    [Fact]
    public void Load_NullReader_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => LibraryStore.Load((TextReader)null!));
    }

    [Fact]
    public void Load_PreservesExactOrderOfValidItems()
    {
        string content = string.Join(Environment.NewLine,
            @"Audio|First|Artist|00:01:00|C:\1.mp3|-",
            @"Audio|Second|Artist|00:02:00|C:\2.mp3|-",
            @"corrupt",
            @"Audio|Third|Artist|00:03:00|C:\3.mp3|-");
        string filePath = CreateTempFile(content);

        var result = LibraryStore.Load(filePath);

        Assert.Equal(3, result.Items.Count);
        Assert.Equal("First", result.Items[0].Title);
        Assert.Equal("Second", result.Items[1].Title);
        Assert.Equal("Third", result.Items[2].Title);
    }

    [Fact]
    public void Load_MissingDirectory_ReturnsEmptyResultWithoutThrowing()
    {
        string pathInNonExistentDir = Path.Combine(_tempDirectory, "missing_subdir_" + Guid.NewGuid().ToString("N"), "Library.txt");

        var result = LibraryStore.Load(pathInNonExistentDir);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.SkippedLineCount);
        Assert.Equal(0, result.TotalLineCount);
    }

    [Fact]
    public void Load_ExistingFileLockedExclusively_ThrowsIOException()
    {
        string filePath = CreateTempFile("Audio|Track|Artist|00:03:00|C:\\file.mp3|-");
        using var lockStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

        Assert.Throws<IOException>(() => LibraryStore.Load(filePath));
    }

    [Fact]
    public void Load_ExistingFileLockedExclusively_OutOverload_ThrowsIOException()
    {
        string filePath = CreateTempFile("Audio|Track|Artist|00:03:00|C:\\file.mp3|-");
        using var lockStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

        Assert.Throws<IOException>(() => LibraryStore.Load(filePath, out _));
    }

    [Fact]
    public void Load_DirectoryPathInsteadOfFile_ThrowsUnauthorizedAccessException()
    {
        Assert.Throws<UnauthorizedAccessException>(() => LibraryStore.Load(_tempDirectory));
    }
}

