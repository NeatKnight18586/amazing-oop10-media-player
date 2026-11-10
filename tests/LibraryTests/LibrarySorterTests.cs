using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediaPlayerPrj;

namespace LibraryTests;

[TestClass]
public class LibrarySorterTests
{
    private static MediaItem Song(string title, string artist) =>
        new AudioItem(title, artist, TimeSpan.FromMinutes(2), title + ".mp3", "-");

    [TestMethod]
    public void TitleSortIsStableAndAscending()
    {
        MediaItem firstA = Song("Alpha", "Z");
        MediaItem secondA = Song("alpha", "A");
        var items = new List<MediaItem> { Song("Zeta", "C"), firstA, Song("Beta", "B"), secondA };
        LibrarySorter.InsertionSort(items, SortKey.Title);
        Assert.AreSame(firstA, items[0]);
        Assert.AreSame(secondA, items[1]);
        Assert.AreEqual("Beta", items[2].Title);
        Assert.AreEqual("Zeta", items[3].Title);
    }

    [TestMethod]
    public void ArtistSortOrdersNamesIgnoringCase()
    {
        var items = new List<MediaItem> { Song("x", "Zoe"), Song("y", "alex"), Song("z", "Bob") };
        LibrarySorter.InsertionSort(items, SortKey.Artist);
        Assert.AreEqual("alex", items[0].Artist);
        Assert.AreEqual("Bob", items[1].Artist);
        Assert.AreEqual("Zoe", items[2].Artist);
    }
}
