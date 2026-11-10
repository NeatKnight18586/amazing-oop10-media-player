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
}
