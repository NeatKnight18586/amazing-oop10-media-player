using System;
using System.Collections.Generic;
using mediaPlayerPrj;
using Xunit;

namespace DarrelLibraryTests;

/// <summary>
/// Unit tests for <see cref="LibrarySorter.Compare"/>, sort key extraction, and case-insensitive comparison rules (FR6, FR12).
/// </summary>
public class LibrarySorterComparisonTests
{
    [Fact]
    public void Compare_ByTitle_AlphabeticalOrder()
    {
        var itemA = new AudioItem("Acoustic Melody", "Artist", TimeSpan.FromMinutes(3), "a.mp3");
        var itemB = new AudioItem("Bass Harmony", "Artist", TimeSpan.FromMinutes(3), "b.mp3");

        int result1 = LibrarySorter.Compare(itemA, itemB, SortKey.Title);
        int result2 = LibrarySorter.Compare(itemB, itemA, SortKey.Title);

        Assert.True(result1 < 0, "Item A should precede Item B alphabetically by title.");
        Assert.True(result2 > 0, "Item B should succeed Item A alphabetically by title.");
    }

    [Theory]
    [InlineData("Bohemian Rhapsody", "bohemian rhapsody")]
    [InlineData("HOTEL CALIFORNIA", "hotel california")]
    [InlineData("Imagine", "IMAGINE")]
    public void Compare_ByTitle_IsCaseInsensitive(string title1, string title2)
    {
        var item1 = new AudioItem(title1, "Queen", TimeSpan.FromMinutes(5), "1.mp3");
        var item2 = new AudioItem(title2, "Different Artist", TimeSpan.FromMinutes(5), "2.mp3");

        int result = LibrarySorter.Compare(item1, item2, SortKey.Title);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Compare_ByArtist_AlphabeticalOrder()
    {
        var itemA = new AudioItem("Song 1", "Adele", TimeSpan.FromMinutes(3), "1.mp3");
        var itemB = new AudioItem("Song 2", "Coldplay", TimeSpan.FromMinutes(4), "2.mp3");

        int result1 = LibrarySorter.Compare(itemA, itemB, SortKey.Artist);
        int result2 = LibrarySorter.Compare(itemB, itemA, SortKey.Artist);

        Assert.True(result1 < 0, "Adele should precede Coldplay alphabetically by artist.");
        Assert.True(result2 > 0, "Coldplay should follow Adele alphabetically by artist.");
    }

    [Theory]
    [InlineData("Taylor Swift", "taylor swift")]
    [InlineData("THE BEATLES", "the beatles")]
    [InlineData("Ed Sheeran", "ED SHEERAN")]
    public void Compare_ByArtist_IsCaseInsensitive(string artist1, string artist2)
    {
        var item1 = new AudioItem("Song A", artist1, TimeSpan.FromMinutes(3), "a.mp3");
        var item2 = new AudioItem("Song B", artist2, TimeSpan.FromMinutes(4), "b.mp3");

        int result = LibrarySorter.Compare(item1, item2, SortKey.Artist);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Compare_SameReference_ReturnsZero()
    {
        var item = new AudioItem("Identical", "Artist", TimeSpan.FromMinutes(3), "same.mp3");

        Assert.Equal(0, LibrarySorter.Compare(item, item, SortKey.Title));
        Assert.Equal(0, LibrarySorter.Compare(item, item, SortKey.Artist));
    }

    [Fact]
    public void Compare_NullHandling_OrdersNullsDeterministically()
    {
        var item = new AudioItem("Title", "Artist", TimeSpan.FromMinutes(3), "file.mp3");

        Assert.Equal(0, LibrarySorter.Compare(null, null, SortKey.Title));
        Assert.True(LibrarySorter.Compare(null, item, SortKey.Title) < 0, "Null should precede non-null item.");
        Assert.True(LibrarySorter.Compare(item, null, SortKey.Title) > 0, "Non-null should follow null item.");
    }

    [Fact]
    public void Compare_MixedMediaSubclasses_ComparesPolymorphicItemsByTitle()
    {
        var audio = new AudioItem("Alpha Audio", "Musician", TimeSpan.FromMinutes(3), "a.mp3", "Album");
        var video = new VideoItem("Beta Video", "Director", TimeSpan.FromMinutes(10), "b.mp4", "1920x1080");

        int result = LibrarySorter.Compare(audio, video, SortKey.Title);

        Assert.True(result < 0, "Alpha Audio should precede Beta Video by title.");
    }

    [Fact]
    public void Compare_MixedMediaSubclasses_ComparesPolymorphicItemsByArtist()
    {
        var audio = new AudioItem("Track", "Zack Artist", TimeSpan.FromMinutes(3), "a.mp3");
        var video = new VideoItem("Documentary", "Alan Creator", TimeSpan.FromMinutes(45), "v.mp4");

        int result = LibrarySorter.Compare(audio, video, SortKey.Artist);

        Assert.True(result > 0, "Zack Artist should follow Alan Creator by artist.");
    }

    [Fact]
    public void GetSortKey_ReturnsMatchingProperty()
    {
        var item = new AudioItem("Super Title", "Cool Artist", TimeSpan.FromMinutes(2), "file.mp3");

        Assert.Equal("Super Title", LibrarySorter.GetSortKey(item, SortKey.Title));
        Assert.Equal("Cool Artist", LibrarySorter.GetSortKey(item, SortKey.Artist));
        Assert.Equal(string.Empty, LibrarySorter.GetSortKey(null, SortKey.Title));
    }

    [Fact]
    public void Compare_InvalidSortKey_ThrowsArgumentOutOfRangeException()
    {
        var item1 = new AudioItem("T1", "A1", TimeSpan.FromMinutes(1), "1.mp3");
        var item2 = new AudioItem("T2", "A2", TimeSpan.FromMinutes(1), "2.mp3");
        var invalidKey = (SortKey)99;

        Assert.Throws<ArgumentOutOfRangeException>(() => LibrarySorter.Compare(item1, item2, invalidKey));
        Assert.Throws<ArgumentOutOfRangeException>(() => LibrarySorter.Compare(null, null, invalidKey));
    }

    [Fact]
    public void GetComparer_InvalidSortKey_ThrowsArgumentOutOfRangeException()
    {
        var invalidKey = (SortKey)123;
        Assert.Throws<ArgumentOutOfRangeException>(() => LibrarySorter.GetComparer(invalidKey));
    }

    [Fact]
    public void Compare_BlankOrWhitespaceTitle_UsesDefaultsAndOrdersPredictably()
    {
        var blankItem = new AudioItem("   ", "Artist", TimeSpan.FromMinutes(2), "1.mp3");
        var emptyItem = new AudioItem("", "Artist", TimeSpan.FromMinutes(2), "2.mp3");
        var normalItem = new AudioItem("Beethoven Symphony", "Artist", TimeSpan.FromMinutes(2), "3.mp3");

        // Both blank and empty normalize to "Unknown Title"
        Assert.Equal(0, LibrarySorter.Compare(blankItem, emptyItem, SortKey.Title));

        // "Beethoven Symphony" precedes "Unknown Title"
        Assert.True(LibrarySorter.Compare(normalItem, blankItem, SortKey.Title) < 0);
        Assert.True(LibrarySorter.Compare(blankItem, normalItem, SortKey.Title) > 0);
    }

    [Fact]
    public void Compare_BlankOrWhitespaceArtist_UsesDefaultsAndOrdersPredictably()
    {
        var blankArtistItem = new AudioItem("Song", "   ", TimeSpan.FromMinutes(2), "1.mp3");
        var emptyArtistItem = new AudioItem("Song", null!, TimeSpan.FromMinutes(2), "2.mp3");
        var zackItem = new AudioItem("Song", "Zack Band", TimeSpan.FromMinutes(2), "3.mp3");

        // Both blank and null normalize to "Unknown Artist"
        Assert.Equal(0, LibrarySorter.Compare(blankArtistItem, emptyArtistItem, SortKey.Artist));

        // "Unknown Artist" precedes "Zack Band"
        Assert.True(LibrarySorter.Compare(blankArtistItem, zackItem, SortKey.Artist) < 0);
        Assert.True(LibrarySorter.Compare(zackItem, blankArtistItem, SortKey.Artist) > 0);
    }

    [Fact]
    public void GetComparer_DelegatesToCompareAccurately()
    {
        var itemA = new AudioItem("A Title", "Artist", TimeSpan.FromMinutes(1), "a.mp3");
        var itemB = new AudioItem("B Title", "Artist", TimeSpan.FromMinutes(1), "b.mp3");

        IComparer<MediaItem> comparer = LibrarySorter.GetComparer(SortKey.Title);

        Assert.True(comparer.Compare(itemA, itemB) < 0);
        Assert.True(comparer.Compare(itemB, itemA) > 0);
        Assert.Equal(0, comparer.Compare(itemA, itemA));
    }
}

