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
}
