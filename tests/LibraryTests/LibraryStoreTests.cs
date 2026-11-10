using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediaPlayerPrj;

namespace LibraryTests;

[TestClass]
public class LibraryStoreTests
{
    private static string FileForTest() =>
        Path.Combine(Path.GetTempPath(), "AmazingPlayerTests", Guid.NewGuid() + ".txt");

    [TestMethod]
    public void MissingFileStartsWithEmptyLibrary()
    {
        List<MediaItem> result = LibraryStore.Load(FileForTest(), out int skipped);
        Assert.AreEqual(0, result.Count);
        Assert.AreEqual(0, skipped);
    }

    [TestMethod]
    public void AudioAndVideoSurviveSaveAndReload()
    {
        string path = FileForTest();
        var source = new List<MediaItem>
        {
            new AudioItem("Song", "Band", TimeSpan.FromMinutes(3), @"C:\music\song.mp3", "Album"),
            new VideoItem("Clip", "Author", TimeSpan.FromSeconds(80), @"C:\music\clip.mp4", "1920x1080")
        };
        try
        {
            LibraryStore.Save(path, source);
            List<MediaItem> loaded = LibraryStore.Load(path, out int skipped);
            Assert.AreEqual(0, skipped);
            Assert.AreEqual(2, loaded.Count);
            Assert.IsInstanceOfType(loaded[0], typeof(AudioItem));
            Assert.IsInstanceOfType(loaded[1], typeof(VideoItem));
            Assert.AreEqual("1920x1080", loaded[1].Extra);
        }
        finally { File.Delete(path); }
    }

    [TestMethod]
    public void CorruptedRowsAreSkippedButGoodRowsRemain()
    {
        string path = FileForTest();
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllLines(path, new []
        {
            "Audio|Good|Artist|00:03:30|C:/music/good.mp3|Album",
            "Invalid",
            "Video|Bad|Artist|nonsense|C:/music/no.mp4|720p"
        });
        try
        {
            var items = LibraryStore.Load(path, out int skipped);
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(2, skipped);
        }
        finally { File.Delete(path); }
    }

    [TestMethod]
    public void ProfessorSampleHeaderCanBeLoaded()
    {
        string path = FileForTest();
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllLines(path, new []
        {
            "Title|Artist|Album|Duration|Type|FilePath",
            "Old Track|Old Band|Old Album|03:10|MP3|C:/old.mp3"
        });
        try
        {
            var items = LibraryStore.Load(path, out int skipped);
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(0, skipped);
            Assert.AreEqual("Old Track", items[0].Title);
        }
        finally { File.Delete(path); }
    }
}
