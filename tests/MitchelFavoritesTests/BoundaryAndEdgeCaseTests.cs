using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediaPlayerPrj;
using System;
using System.Collections.Generic;
using System.IO;

namespace MitchelFavoritesTests;

/// <summary>
/// Dedicated unit tests for edge cases, boundary conditions, path normalization quirks,
/// and duplicate prevention across <see cref="FavoritesStore"/> and <see cref="FavoritesService"/>.
/// </summary>
[TestClass]
public class BoundaryAndEdgeCaseTests
{
    private string _tempFile = null!;
    private FavoritesStore _store = null!;
    private FavoritesService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _tempFile = Path.Combine(Path.GetTempPath(), "boundary_test_" + Guid.NewGuid() + ".txt");
        _store = new FavoritesStore(_tempFile);
        _service = new FavoritesService(_store);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (File.Exists(_tempFile))
        {
            try { File.Delete(_tempFile); } catch { }
        }
    }

    [TestMethod]
    public void PathWithSpaces_PreservesInternalSpacesWhileTrimmingPadding()
    {
        string baseDir = Path.GetTempPath();
        string raw = "   " + Path.Combine(baseDir, "My Media Library", "01 Favorite Track (Live).mp3") + "   ";

        string? normalized = FavoritesStore.NormalizePath(raw);

        Assert.IsNotNull(normalized);
        Assert.IsTrue(normalized.Contains("My Media Library"));
        Assert.IsTrue(normalized.Contains("01 Favorite Track (Live).mp3"));
        Assert.IsFalse(normalized.StartsWith(" "));
        Assert.IsFalse(normalized.EndsWith(" "));
    }

    [TestMethod]
    public void SlashConsistency_ForwardAndBackslashes_ResolveToSameFavorite()
    {
        string dir = Path.GetTempPath().Replace('\\', '/');
        string slashVariant = dir.TrimEnd('/') + "/subfolder/song.mp3";
        string backslashVariant = slashVariant.Replace('/', '\\');

        _service.Add(slashVariant);

        Assert.AreEqual(1, _service.Count);
        Assert.IsTrue(_service.IsFavorite(slashVariant));
        Assert.IsTrue(_service.IsFavorite(backslashVariant));
    }

    [TestMethod]
    public void Toggle_MixedCaseInput_CorrectlyRemovesExistingItem()
    {
        string lower = Path.Combine(Path.GetTempPath(), "mixed_case_test.mp3").ToLowerInvariant();
        string upper = lower.ToUpperInvariant();

        bool added = _service.Toggle(lower);
        Assert.IsTrue(added, "Initial toggle should add.");
        Assert.AreEqual(1, _service.Count);

        bool removed = _service.Toggle(upper);
        Assert.IsFalse(removed, "Toggling with uppercase should match and remove.");
        Assert.AreEqual(0, _service.Count);
    }

    [TestMethod]
    public void Remove_MixedCaseInput_Succeeds()
    {
        string path = Path.Combine(Path.GetTempPath(), "remove_case_test.mp3");
        _service.Add(path);

        bool removed = _service.Remove(path.ToUpperInvariant());

        Assert.IsTrue(removed);
        Assert.AreEqual(0, _service.Count);
        Assert.IsFalse(_service.IsFavorite(path));
    }

    [TestMethod]
    public void InvalidPathWithPipe_IsSafelyRejected()
    {
        // On Windows and Linux, pipe characters are illegal in file paths
        string pipePath = "C:\\Music|Song.mp3";

        string? normalized = FavoritesStore.NormalizePath(pipePath);

        // Should return null due to ArgumentException in Path.GetFullPath
        Assert.IsNull(normalized);
        Assert.IsFalse(_service.Add(pipePath));
        Assert.IsFalse(_service.IsFavorite(pipePath));
        Assert.IsFalse(_service.Toggle(pipePath));
    }

    [TestMethod]
    public void WhiteSpaceVariants_AllRejectedConsistently()
    {
        string[] badInputs = { "", " ", "   ", "\t", "\r\n", "\n", " \t \r\n " };
        foreach (var bad in badInputs)
        {
            Assert.IsNull(FavoritesStore.NormalizePath(bad));
            Assert.IsFalse(_service.Add(bad));
            Assert.IsFalse(_service.Remove(bad));
            Assert.IsFalse(_service.Toggle(bad));
            Assert.IsFalse(_service.IsFavorite(bad));
        }
    }

    [TestMethod]
    public void FilterFavorites_PreservesPolymorphicSubclassProperties()
    {
        string audioPath = Path.Combine(Path.GetTempPath(), "polymorphic_audio.mp3");
        string videoPath = Path.Combine(Path.GetTempPath(), "polymorphic_video.mp4");
        _service.Add(audioPath);
        _service.Add(videoPath);

        var audio = new AudioItem("Audio Title", "Audio Artist", TimeSpan.FromMinutes(3), audioPath, "Album XYZ");
        var video = new VideoItem("Video Title", "Video Creator", TimeSpan.FromMinutes(10), videoPath, "1920x1080");

        var filtered = _service.FilterFavorites(new MediaItem[] { audio, video });

        Assert.AreEqual(2, filtered.Count);

        var filteredAudio = filtered[0] as AudioItem;
        Assert.IsNotNull(filteredAudio);
        Assert.AreEqual("Album XYZ", filteredAudio.Album);
        Assert.AreEqual("Audio", filteredAudio.MediaType);

        var filteredVideo = filtered[1] as VideoItem;
        Assert.IsNotNull(filteredVideo);
        Assert.AreEqual("1920x1080", filteredVideo.Resolution);
        Assert.AreEqual("Video", filteredVideo.MediaType);
    }

    [TestMethod]
    public void SequenceOperations_AddRemoveToggle_StateRemainsConsistent()
    {
        var paths = new List<string>();
        for (int i = 0; i < 20; i++)
        {
            paths.Add(Path.Combine(Path.GetTempPath(), $"seq_{i}.mp3"));
        }

        // Add all
        foreach (var p in paths) _service.Add(p);
        Assert.AreEqual(20, _service.Count);

        // Remove first 5
        for (int i = 0; i < 5; i++) Assert.IsTrue(_service.Remove(paths[i]));
        Assert.AreEqual(15, _service.Count);

        // Toggle first 5 back on
        for (int i = 0; i < 5; i++) Assert.IsTrue(_service.Toggle(paths[i]));
        Assert.AreEqual(20, _service.Count);

        // Toggle next 5 off
        for (int i = 5; i < 10; i++) Assert.IsFalse(_service.Toggle(paths[i]));
        Assert.AreEqual(15, _service.Count);

        // Verify backing store persistence matches
        var reloaded = new FavoritesService(_store);
        Assert.AreEqual(15, reloaded.Count);
    }
}
