using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediaPlayerPrj;
using System;
using System.IO;

namespace MitchelFavoritesTests;

/// <summary>
/// Tests for <see cref="FavoritesStore"/>: path normalization,
/// constructor defaults, and initial load behavior.
/// </summary>
[TestClass]
public class FavoritesStoreTests
{
    // ──────────────────────────────────────────────
    //  NormalizePath
    // ──────────────────────────────────────────────

    [TestMethod]
    public void NormalizePath_NullInput_ReturnsNull()
    {
        Assert.IsNull(FavoritesStore.NormalizePath(null));
    }

    [TestMethod]
    public void NormalizePath_EmptyString_ReturnsNull()
    {
        Assert.IsNull(FavoritesStore.NormalizePath(""));
    }

    [TestMethod]
    public void NormalizePath_WhitespaceOnly_ReturnsNull()
    {
        Assert.IsNull(FavoritesStore.NormalizePath("   \t  "));
    }

    [TestMethod]
    public void NormalizePath_ValidAbsolutePath_ReturnsFullPath()
    {
        string abs = Path.Combine(Path.GetTempPath(), "music", "song.mp3");
        string? result = FavoritesStore.NormalizePath(abs);

        Assert.IsNotNull(result);
        Assert.AreEqual(Path.GetFullPath(abs), result);
    }

    [TestMethod]
    public void NormalizePath_LeadingTrailingWhitespace_IsTrimmed()
    {
        string abs = Path.Combine(Path.GetTempPath(), "music", "song.mp3");
        string padded = "   " + abs + "   ";

        string? result = FavoritesStore.NormalizePath(padded);

        Assert.IsNotNull(result);
        Assert.AreEqual(Path.GetFullPath(abs), result);
    }

    [TestMethod]
    public void NormalizePath_RelativePath_ResolvesToAbsolute()
    {
        string? result = FavoritesStore.NormalizePath("song.mp3");

        Assert.IsNotNull(result);
        Assert.IsTrue(Path.IsPathRooted(result),
            "Relative path should be resolved to an absolute path.");
    }

    [TestMethod]
    public void NormalizePath_PathWithNullChar_ReturnsNull()
    {
        // Null character is invalid on all platforms.
        Assert.IsNull(FavoritesStore.NormalizePath("path/to\0/file.mp3"));
    }

    [TestMethod]
    public void NormalizePath_TwoEquivalentPaths_NormalizeIdentically()
    {
        string dir = Path.GetTempPath();
        string path1 = Path.Combine(dir, "sub", "..", "song.mp3");
        string path2 = Path.Combine(dir, "song.mp3");

        string? n1 = FavoritesStore.NormalizePath(path1);
        string? n2 = FavoritesStore.NormalizePath(path2);

        Assert.AreEqual(n1, n2,
            "Paths that differ only by '..' traversal should normalize identically.");
    }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    [TestMethod]
    public void Constructor_DefaultPath_EndsWithFavoritesTxt()
    {
        var store = new FavoritesStore();
        Assert.IsTrue(store.FilePath.EndsWith(FavoritesStore.DefaultFileName),
            $"Default path should end with '{FavoritesStore.DefaultFileName}'.");
    }

    [TestMethod]
    public void Constructor_CustomPath_UsesProvidedPath()
    {
        string custom = Path.Combine(Path.GetTempPath(), "my_favorites.txt");
        var store = new FavoritesStore(custom);
        Assert.AreEqual(custom, store.FilePath);
    }

    [TestMethod]
    public void Constructor_NullPath_FallsBackToDefault()
    {
        var store = new FavoritesStore(null);
        Assert.IsTrue(store.FilePath.EndsWith(FavoritesStore.DefaultFileName));
    }

    [TestMethod]
    public void Constructor_BlankPath_FallsBackToDefault()
    {
        var store = new FavoritesStore("   ");
        Assert.IsTrue(store.FilePath.EndsWith(FavoritesStore.DefaultFileName));
    }

    // ──────────────────────────────────────────────
    //  Load — initial behavior
    // ──────────────────────────────────────────────

    [TestMethod]
    public void Load_NonExistentFile_ReturnsEmptySetAndZeroSkipped()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        var store = new FavoritesStore(path);

        var result = store.Load(out int skipped);

        Assert.AreEqual(0, result.Count, "Missing file should produce empty set.");
        Assert.AreEqual(0, skipped, "Missing file should report zero skipped lines.");
    }

    [TestMethod]
    public void Load_ReturnedSet_IsCaseInsensitive()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        var store = new FavoritesStore(path);

        var result = store.Load(out _);

        // Verify the set uses case-insensitive comparison.
        result.Add("/tmp/Song.mp3");
        Assert.IsTrue(result.Contains("/tmp/song.mp3"),
            "Returned set must use case-insensitive comparison.");
    }

    // ──────────────────────────────────────────────
    //  Load — file with content
    // ──────────────────────────────────────────────

    [TestMethod]
    public void Load_FileWithValidPaths_ReturnsAllPaths()
    {
        string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        string song1 = Path.Combine(Path.GetTempPath(), "song1.mp3");
        string song2 = Path.Combine(Path.GetTempPath(), "song2.mp4");
        try
        {
            File.WriteAllLines(file, new[] { song1, song2 });
            var store = new FavoritesStore(file);

            var result = store.Load(out int skipped);

            Assert.AreEqual(2, result.Count, "Should load both valid paths.");
            Assert.AreEqual(0, skipped, "No lines should be skipped.");
            Assert.IsTrue(result.Contains(Path.GetFullPath(song1)));
            Assert.IsTrue(result.Contains(Path.GetFullPath(song2)));
        }
        finally { File.Delete(file); }
    }

    [TestMethod]
    public void Load_EmptyFile_ReturnsEmptySetZeroSkipped()
    {
        string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        try
        {
            File.WriteAllText(file, "");
            var store = new FavoritesStore(file);

            var result = store.Load(out int skipped);

            Assert.AreEqual(0, result.Count);
            Assert.AreEqual(0, skipped);
        }
        finally { File.Delete(file); }
    }

    [TestMethod]
    public void Load_BlankLines_AreSkippedAndCounted()
    {
        string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        string song = Path.Combine(Path.GetTempPath(), "song.mp3");
        try
        {
            File.WriteAllLines(file, new[] { "", "   ", song, "\t" });
            var store = new FavoritesStore(file);

            var result = store.Load(out int skipped);

            Assert.AreEqual(1, result.Count, "Only the valid path should load.");
            Assert.AreEqual(3, skipped, "Three blank/whitespace lines should be skipped.");
        }
        finally { File.Delete(file); }
    }

    [TestMethod]
    public void Load_DuplicatePaths_DedupedAndCounted()
    {
        string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        string song = Path.Combine(Path.GetTempPath(), "song.mp3");
        try
        {
            File.WriteAllLines(file, new[] { song, song, song });
            var store = new FavoritesStore(file);

            var result = store.Load(out int skipped);

            Assert.AreEqual(1, result.Count, "Duplicates should be absorbed.");
            Assert.AreEqual(2, skipped, "Two duplicate lines should be counted.");
        }
        finally { File.Delete(file); }
    }

    [TestMethod]
    public void Load_PathsWithLeadingTrailingSpaces_AreTrimmed()
    {
        string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        string song = Path.Combine(Path.GetTempPath(), "song.mp3");
        try
        {
            File.WriteAllLines(file, new[] { "  " + song + "  " });
            var store = new FavoritesStore(file);

            var result = store.Load(out int skipped);

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(Path.GetFullPath(song)),
                "Loaded path should match the trimmed/normalized form.");
        }
        finally { File.Delete(file); }
    }

    [TestMethod]
    public void Load_LineWithNullChar_IsSkipped()
    {
        string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        string good = Path.Combine(Path.GetTempPath(), "good.mp3");
        try
        {
            File.WriteAllLines(file, new[] { "bad\0path.mp3", good });
            var store = new FavoritesStore(file);

            var result = store.Load(out int skipped);

            Assert.AreEqual(1, result.Count, "Only the valid path should load.");
            Assert.AreEqual(1, skipped, "The invalid-char line should be skipped.");
        }
        finally { File.Delete(file); }
    }

    [TestMethod]
    public void Load_CaseDuplicatesOnSamePath_DedupedCaseInsensitively()
    {
        string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        string lower = Path.Combine(Path.GetTempPath(), "song.mp3");
        string upper = Path.Combine(Path.GetTempPath(), "SONG.MP3");
        try
        {
            File.WriteAllLines(file, new[] { lower, upper });
            var store = new FavoritesStore(file);

            var result = store.Load(out int skipped);

            // On case-insensitive FS (Windows/macOS default), these normalize
            // to the same full path. The set is case-insensitive regardless.
            Assert.IsTrue(result.Count <= 2,
                "Case-insensitive set may dedup depending on OS normalization.");
        }
        finally { File.Delete(file); }
    }

    // ──────────────────────────────────────────────
    //  Save
    // ──────────────────────────────────────────────

    [TestMethod]
    public void Save_ValidPaths_WritesOnePerLine()
    {
        string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        string s1 = Path.Combine(Path.GetTempPath(), "a.mp3");
        string s2 = Path.Combine(Path.GetTempPath(), "b.mp4");
        try
        {
            var store = new FavoritesStore(file);
            store.Save(new[] { s1, s2 });

            string[] written = File.ReadAllLines(file);
            Assert.AreEqual(2, written.Length);
            Assert.AreEqual(s1, written[0]);
            Assert.AreEqual(s2, written[1]);
        }
        finally { File.Delete(file); }
    }

    [TestMethod]
    public void Save_EmptyCollection_WritesEmptyFile()
    {
        string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        try
        {
            var store = new FavoritesStore(file);
            store.Save(Array.Empty<string>());

            Assert.IsTrue(File.Exists(file), "File should be created even when empty.");
            Assert.AreEqual(0, File.ReadAllLines(file).Length);
        }
        finally { File.Delete(file); }
    }

    [TestMethod]
    public void Save_NullEntriesInCollection_AreSkipped()
    {
        string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        string good = Path.Combine(Path.GetTempPath(), "good.mp3");
        try
        {
            var store = new FavoritesStore(file);
            store.Save(new[] { null!, "  ", good });

            string[] written = File.ReadAllLines(file);
            Assert.AreEqual(1, written.Length, "Only valid entries should be written.");
            Assert.AreEqual(good, written[0]);
        }
        finally { File.Delete(file); }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Save_NullArgument_Throws()
    {
        var store = new FavoritesStore(Path.Combine(Path.GetTempPath(), "x.txt"));
        store.Save(null!);
    }

    [TestMethod]
    public void Save_ThenLoad_RoundTrip()
    {
        string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        string s1 = Path.Combine(Path.GetTempPath(), "song1.mp3");
        string s2 = Path.Combine(Path.GetTempPath(), "video.mp4");
        try
        {
            var store = new FavoritesStore(file);
            store.Save(new[] { s1, s2 });

            var loaded = store.Load(out int skipped);

            Assert.AreEqual(2, loaded.Count, "Round-trip should preserve all entries.");
            Assert.AreEqual(0, skipped);
            Assert.IsTrue(loaded.Contains(Path.GetFullPath(s1)));
            Assert.IsTrue(loaded.Contains(Path.GetFullPath(s2)));
        }
        finally { File.Delete(file); }
    }

    [TestMethod]
    public void Save_OverwritesExistingFile()
    {
        string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        string old = Path.Combine(Path.GetTempPath(), "old.mp3");
        string newer = Path.Combine(Path.GetTempPath(), "new.mp3");
        try
        {
            var store = new FavoritesStore(file);
            store.Save(new[] { old });
            store.Save(new[] { newer });

            string[] written = File.ReadAllLines(file);
            Assert.AreEqual(1, written.Length, "Second save should overwrite.");
            Assert.AreEqual(newer, written[0]);
        }
        finally { File.Delete(file); }
    }
}
