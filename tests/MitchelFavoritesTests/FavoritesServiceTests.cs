using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediaPlayerPrj;
using System;
using System.IO;

namespace MitchelFavoritesTests;

/// <summary>
/// Unit tests for <see cref="FavoritesService"/> core operations: Add, Remove, Toggle, and Count.
/// </summary>
[TestClass]
public class FavoritesServiceTests
{
    private string _tempFilePath = null!;
    private FavoritesStore _store = null!;
    private FavoritesService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _tempFilePath = Path.Combine(Path.GetTempPath(), "fav_service_test_" + Guid.NewGuid() + ".txt");
        _store = new FavoritesStore(_tempFilePath);
        _service = new FavoritesService(_store);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (File.Exists(_tempFilePath))
        {
            try { File.Delete(_tempFilePath); } catch { }
        }
    }

    [TestMethod]
    public void Add_ValidPath_ReturnsTrueAndIncreasesCount()
    {
        string path = Path.Combine(Path.GetTempPath(), "track1.mp3");

        bool result = _service.Add(path);

        Assert.IsTrue(result, "Add should return true for a new valid path.");
        Assert.AreEqual(1, _service.Count);
    }

    [TestMethod]
    public void Add_DuplicatePath_ReturnsFalseAndCountUnchanged()
    {
        string path = Path.Combine(Path.GetTempPath(), "track1.mp3");
        _service.Add(path);

        bool duplicateResult = _service.Add(path);

        Assert.IsFalse(duplicateResult, "Add should return false for already favorited item.");
        Assert.AreEqual(1, _service.Count);
    }

    [TestMethod]
    public void Add_NullOrWhitespace_ReturnsFalse()
    {
        Assert.IsFalse(_service.Add(null));
        Assert.IsFalse(_service.Add(""));
        Assert.IsFalse(_service.Add("   "));
        Assert.AreEqual(0, _service.Count);
    }

    [TestMethod]
    public void Remove_ExistingFavorite_ReturnsTrueAndDecreasesCount()
    {
        string path = Path.Combine(Path.GetTempPath(), "track1.mp3");
        _service.Add(path);

        bool removed = _service.Remove(path);

        Assert.IsTrue(removed, "Remove should return true when removing an existing favorite.");
        Assert.AreEqual(0, _service.Count);
    }

    [TestMethod]
    public void Remove_NonExistingFavorite_ReturnsFalse()
    {
        string path = Path.Combine(Path.GetTempPath(), "nonexistent.mp3");

        bool removed = _service.Remove(path);

        Assert.IsFalse(removed, "Remove should return false for non-favorited path.");
        Assert.AreEqual(0, _service.Count);
    }

    [TestMethod]
    public void Remove_NullOrWhitespace_ReturnsFalse()
    {
        Assert.IsFalse(_service.Remove(null));
        Assert.IsFalse(_service.Remove(""));
        Assert.IsFalse(_service.Remove("   "));
    }

    [TestMethod]
    public void Toggle_WhenNotFavorite_AddsAndReturnsTrue()
    {
        string path = Path.Combine(Path.GetTempPath(), "track_toggle.mp3");

        bool status = _service.Toggle(path);

        Assert.IsTrue(status, "Toggling non-favorite should add it and return true.");
        Assert.AreEqual(1, _service.Count);
    }

    [TestMethod]
    public void Toggle_WhenAlreadyFavorite_RemovesAndReturnsFalse()
    {
        string path = Path.Combine(Path.GetTempPath(), "track_toggle.mp3");
        _service.Add(path);

        bool status = _service.Toggle(path);

        Assert.IsFalse(status, "Toggling existing favorite should remove it and return false.");
        Assert.AreEqual(0, _service.Count);
    }

    [TestMethod]
    public void Toggle_InvalidPath_ReturnsFalseAndCountUnchanged()
    {
        bool status = _service.Toggle(null);

        Assert.IsFalse(status);
        Assert.AreEqual(0, _service.Count);
    }

    [TestMethod]
    public void ToggleFavorite_Alias_BehavesSameAsToggle()
    {
        string path = Path.Combine(Path.GetTempPath(), "track_alias.mp3");

        bool firstToggle = _service.ToggleFavorite(path);
        Assert.IsTrue(firstToggle);
        Assert.AreEqual(1, _service.Count);

        bool secondToggle = _service.ToggleFavorite(path);
        Assert.IsFalse(secondToggle);
        Assert.AreEqual(0, _service.Count);
    }

    // ──────────────────────────────────────────────
    //  IsFavorite & Filter operations
    // ──────────────────────────────────────────────

    [TestMethod]
    public void IsFavorite_WhenFavorited_ReturnsTrue()
    {
        string path = Path.Combine(Path.GetTempPath(), "fav_track.mp3");
        _service.Add(path);

        Assert.IsTrue(_service.IsFavorite(path));
    }

    [TestMethod]
    public void IsFavorite_WhenNotFavorited_ReturnsFalse()
    {
        string path = Path.Combine(Path.GetTempPath(), "unfav_track.mp3");

        Assert.IsFalse(_service.IsFavorite(path));
    }

    [TestMethod]
    public void IsFavorite_NullOrEmpty_ReturnsFalse()
    {
        Assert.IsFalse(_service.IsFavorite(null));
        Assert.IsFalse(_service.IsFavorite(""));
        Assert.IsFalse(_service.IsFavorite("   "));
    }

    [TestMethod]
    public void IsFavorite_MediaItemOverload_ReturnsExpected()
    {
        string favPath = Path.Combine(Path.GetTempPath(), "fav_audio.mp3");
        string unfavPath = Path.Combine(Path.GetTempPath(), "unfav_audio.mp3");
        _service.Add(favPath);

        var favAudio = new AudioItem("Title A", "Artist A", TimeSpan.FromMinutes(3), favPath, "Album A");
        var unfavAudio = new AudioItem("Title B", "Artist B", TimeSpan.FromMinutes(4), unfavPath, "Album B");

        Assert.IsTrue(_service.IsFavorite(favAudio));
        Assert.IsFalse(_service.IsFavorite(unfavAudio));
        Assert.IsFalse(_service.IsFavorite((MediaItem?)null));
    }

    [TestMethod]
    public void GetAllFavorites_ReturnsAllTrackedPaths()
    {
        string path1 = Path.Combine(Path.GetTempPath(), "t1.mp3");
        string path2 = Path.Combine(Path.GetTempPath(), "t2.mp3");
        _service.Add(path1);
        _service.Add(path2);

        var all = _service.GetAllFavorites();

        Assert.AreEqual(2, all.Count);
    }

    [TestMethod]
    public void FilterFavorites_PreservesOrderAndFiltersOnlyFavorites()
    {
        string path1 = Path.Combine(Path.GetTempPath(), "f1.mp3");
        string path2 = Path.Combine(Path.GetTempPath(), "f2.mp3");
        string path3 = Path.Combine(Path.GetTempPath(), "f3.mp3");

        _service.Add(path1);
        _service.Add(path3);

        var item1 = new AudioItem("Song 1", "Artist 1", TimeSpan.FromMinutes(2), path1, "Album 1");
        var item2 = new AudioItem("Song 2", "Artist 2", TimeSpan.FromMinutes(3), path2, "Album 2");
        var item3 = new VideoItem("Video 3", "Creator 3", TimeSpan.FromMinutes(5), path3, "1080p");

        var playlist = new List<MediaItem> { item1, item2, item3 };

        var filtered = _service.FilterFavorites(playlist);

        Assert.AreEqual(2, filtered.Count);
        Assert.AreSame(item1, filtered[0]);
        Assert.AreSame(item3, filtered[1]);
    }

    [TestMethod]
    public void FilterFavorites_NullInput_ReturnsEmptyList()
    {
        var result = _service.FilterFavorites(null);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetFavorites_Alias_ReturnsSameAsFilterFavorites()
    {
        string path1 = Path.Combine(Path.GetTempPath(), "g1.mp3");
        _service.Add(path1);

        var item1 = new AudioItem("Song 1", "Artist 1", TimeSpan.FromMinutes(2), path1, "Album 1");
        var playlist = new List<MediaItem> { item1 };

        var filtered = _service.GetFavorites(playlist);

        Assert.AreEqual(1, filtered.Count);
        Assert.AreSame(item1, filtered[0]);
    }
}
