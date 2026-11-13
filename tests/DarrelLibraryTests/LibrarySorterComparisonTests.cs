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

    // ─── InsertionSort tests (FR12) ───────────────────────────────────────────

    [Fact]
    public void InsertionSort_ByTitle_SortsAscendingAlphabetically()
    {
        var c = new AudioItem("Cherry", "Artist", TimeSpan.FromMinutes(1), "c.mp3");
        var a = new AudioItem("Apple", "Artist", TimeSpan.FromMinutes(1), "a.mp3");
        var b = new AudioItem("Banana", "Artist", TimeSpan.FromMinutes(1), "b.mp3");
        var list = new List<MediaItem> { c, a, b };

        LibrarySorter.InsertionSort(list, SortKey.Title);

        Assert.Equal("Apple",  list[0].Title);
        Assert.Equal("Banana", list[1].Title);
        Assert.Equal("Cherry", list[2].Title);
    }

    [Fact]
    public void InsertionSort_ByArtist_SortsAscendingAlphabetically()
    {
        var z = new AudioItem("Song", "Zara",  TimeSpan.FromMinutes(1), "z.mp3");
        var m = new AudioItem("Song", "Maria", TimeSpan.FromMinutes(1), "m.mp3");
        var a = new AudioItem("Song", "Aaron", TimeSpan.FromMinutes(1), "a.mp3");
        var list = new List<MediaItem> { z, m, a };

        LibrarySorter.InsertionSort(list, SortKey.Artist);

        Assert.Equal("Aaron", list[0].Artist);
        Assert.Equal("Maria", list[1].Artist);
        Assert.Equal("Zara",  list[2].Artist);
    }

    [Fact]
    public void InsertionSort_ByTitle_IsStable_PreservesRelativeOrderForEqualTitles()
    {
        // All three have the same title; original insertion order must be preserved.
        var first  = new AudioItem("Same Title", "Artist A", TimeSpan.FromMinutes(1), "1.mp3");
        var second = new AudioItem("Same Title", "Artist B", TimeSpan.FromMinutes(2), "2.mp3");
        var third  = new AudioItem("Same Title", "Artist C", TimeSpan.FromMinutes(3), "3.mp3");
        var list = new List<MediaItem> { first, second, third };

        LibrarySorter.InsertionSort(list, SortKey.Title);

        // References must be in the original order (stable).
        Assert.Same(first,  list[0]);
        Assert.Same(second, list[1]);
        Assert.Same(third,  list[2]);
    }

    [Fact]
    public void InsertionSort_MixedAudioAndVideo_SortsByTitleRegardlessOfSubclass()
    {
        var video = new VideoItem("Zulu Documentary", "Director", TimeSpan.FromMinutes(45), "z.mp4", "1080p");
        var audio = new AudioItem("Alpha Song",       "Musician", TimeSpan.FromMinutes(3),  "a.mp3");
        var list  = new List<MediaItem> { video, audio };

        LibrarySorter.InsertionSort(list, SortKey.Title);

        Assert.IsType<AudioItem>(list[0]);    // "Alpha Song" comes first
        Assert.IsType<VideoItem>(list[1]);    // "Zulu Documentary" follows
    }

    [Fact]
    public void InsertionSort_EmptyList_DoesNotThrow()
    {
        var list = new List<MediaItem>();
        // Should complete silently with no exception.
        LibrarySorter.InsertionSort(list, SortKey.Title);
        Assert.Empty(list);
    }

    [Fact]
    public void InsertionSort_SingleItem_IsUnchanged()
    {
        var item = new AudioItem("Only Track", "Solo Artist", TimeSpan.FromMinutes(4), "only.mp3");
        var list = new List<MediaItem> { item };

        LibrarySorter.InsertionSort(list, SortKey.Title);

        Assert.Single(list);
        Assert.Same(item, list[0]);
    }

    [Fact]
    public void InsertionSort_NullList_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            LibrarySorter.InsertionSort(null!, SortKey.Title));
    }

    [Fact]
    public void InsertionSort_InvalidSortKey_ThrowsArgumentOutOfRangeException()
    {
        var list = new List<MediaItem>
        {
            new AudioItem("Track", "Artist", TimeSpan.FromMinutes(1), "t.mp3")
        };
        var badKey = (SortKey)99;

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            LibrarySorter.InsertionSort(list, badKey));
    }

    [Fact]
    public void InsertionSort_AlreadySortedList_RemainsCorrect()
    {
        var a = new AudioItem("Alpha", "Artist", TimeSpan.FromMinutes(1), "a.mp3");
        var b = new AudioItem("Beta",  "Artist", TimeSpan.FromMinutes(1), "b.mp3");
        var c = new AudioItem("Gamma", "Artist", TimeSpan.FromMinutes(1), "c.mp3");
        var list = new List<MediaItem> { a, b, c };

        LibrarySorter.InsertionSort(list, SortKey.Title);

        Assert.Same(a, list[0]);
        Assert.Same(b, list[1]);
        Assert.Same(c, list[2]);
    }

    [Fact]
    public void InsertionSort_ReverseSortedList_SortsCorrectly()
    {
        var c = new AudioItem("Gamma", "Artist", TimeSpan.FromMinutes(1), "c.mp3");
        var b = new AudioItem("Beta",  "Artist", TimeSpan.FromMinutes(1), "b.mp3");
        var a = new AudioItem("Alpha", "Artist", TimeSpan.FromMinutes(1), "a.mp3");
        var list = new List<MediaItem> { c, b, a };

        LibrarySorter.InsertionSort(list, SortKey.Title);

        Assert.Equal("Alpha", list[0].Title);
        Assert.Equal("Beta",  list[1].Title);
        Assert.Equal("Gamma", list[2].Title);
    }

    [Fact]
    public void InsertionSort_DoesNotRecreateItems_MovesOriginalReferences()
    {
        var item1 = new AudioItem("Bravo", "Artist", TimeSpan.FromMinutes(1), "b.mp3");
        var item2 = new AudioItem("Alpha", "Artist", TimeSpan.FromMinutes(1), "a.mp3");
        var list  = new List<MediaItem> { item1, item2 };

        LibrarySorter.InsertionSort(list, SortKey.Title);

        // The same object references are present (not new copies).
        Assert.Same(item2, list[0]);
        Assert.Same(item1, list[1]);
    }

    // ─── Checkpoint 9: Robust sorting tests ───────────────────────────────────

    [Fact]
    public void InsertionSort_ByArtist_IsStable_PreservesRelativeOrderForEqualArtists()
    {
        // All three share the same artist name; original relative order must survive.
        var first  = new AudioItem("Track 1", "Same Band", TimeSpan.FromMinutes(1), "1.mp3");
        var second = new AudioItem("Track 2", "Same Band", TimeSpan.FromMinutes(2), "2.mp3");
        var third  = new AudioItem("Track 3", "Same Band", TimeSpan.FromMinutes(3), "3.mp3");
        var list   = new List<MediaItem> { first, second, third };

        LibrarySorter.InsertionSort(list, SortKey.Artist);

        // Stability: equal-keyed items retain their original insertion order.
        Assert.Same(first,  list[0]);
        Assert.Same(second, list[1]);
        Assert.Same(third,  list[2]);
    }

    [Fact]
    public void InsertionSort_ByTitle_IsCaseInsensitive_SortsLowerAndUpperCaseCorrectly()
    {
        // "apple" < "Banana" < "CHERRY" when case-insensitive — verify the sort
        // actually uses the same CurrentCultureIgnoreCase rule as Compare().
        var c = new AudioItem("CHERRY", "Artist", TimeSpan.FromMinutes(1), "c.mp3");
        var a = new AudioItem("apple",  "Artist", TimeSpan.FromMinutes(1), "a.mp3");
        var b = new AudioItem("Banana", "Artist", TimeSpan.FromMinutes(1), "b.mp3");
        var list = new List<MediaItem> { c, a, b };

        LibrarySorter.InsertionSort(list, SortKey.Title);

        Assert.Equal("apple",  list[0].Title);
        Assert.Equal("Banana", list[1].Title);
        Assert.Equal("CHERRY", list[2].Title);
    }

    [Fact]
    public void InsertionSort_ByArtist_IsCaseInsensitive_SortsLowerAndUpperCaseCorrectly()
    {
        // "zara" > "Maria" > "aaron" when case-insensitive descending input.
        var z = new AudioItem("Song", "zara",  TimeSpan.FromMinutes(1), "z.mp3");
        var m = new AudioItem("Song", "Maria", TimeSpan.FromMinutes(1), "m.mp3");
        var a = new AudioItem("Song", "aaron", TimeSpan.FromMinutes(1), "a.mp3");
        var list = new List<MediaItem> { z, m, a };

        LibrarySorter.InsertionSort(list, SortKey.Artist);

        Assert.Equal("aaron", list[0].Artist);
        Assert.Equal("Maria", list[1].Artist);
        Assert.Equal("zara",  list[2].Artist);
    }

    [Fact]
    public void InsertionSort_MixedAudioAndVideo_SortsByArtistRegardlessOfSubclass()
    {
        // Mix subclasses and sort by Artist — the sort must be subclass-agnostic.
        var video = new VideoItem("Documentary", "Zane Director", TimeSpan.FromMinutes(60), "v.mp4", "4K");
        var audio = new AudioItem("Ballad",      "Amy Singer",    TimeSpan.FromMinutes(4),  "a.mp3");
        var list  = new List<MediaItem> { video, audio };

        LibrarySorter.InsertionSort(list, SortKey.Artist);

        Assert.IsType<AudioItem>(list[0]);    // "Amy Singer" precedes "Zane Director"
        Assert.IsType<VideoItem>(list[1]);
    }

    [Fact]
    public void InsertionSort_AlreadySortedByArtist_RemainsCorrect()
    {
        var a = new AudioItem("Song", "Adele",    TimeSpan.FromMinutes(1), "a.mp3");
        var b = new AudioItem("Song", "Beyoncé",  TimeSpan.FromMinutes(1), "b.mp3");
        var c = new AudioItem("Song", "Coldplay", TimeSpan.FromMinutes(1), "c.mp3");
        var list = new List<MediaItem> { a, b, c };

        LibrarySorter.InsertionSort(list, SortKey.Artist);

        // Already in order; references must be unchanged.
        Assert.Same(a, list[0]);
        Assert.Same(b, list[1]);
        Assert.Same(c, list[2]);
    }

    [Fact]
    public void InsertionSort_ReverseSortedByArtist_SortsCorrectly()
    {
        var c = new AudioItem("Song", "Coldplay", TimeSpan.FromMinutes(1), "c.mp3");
        var b = new AudioItem("Song", "Beyoncé",  TimeSpan.FromMinutes(1), "b.mp3");
        var a = new AudioItem("Song", "Adele",    TimeSpan.FromMinutes(1), "a.mp3");
        var list = new List<MediaItem> { c, b, a };

        LibrarySorter.InsertionSort(list, SortKey.Artist);

        Assert.Equal("Adele",    list[0].Artist);
        Assert.Equal("Beyoncé",  list[1].Artist);
        Assert.Equal("Coldplay", list[2].Artist);
    }

    [Fact]
    public void InsertionSort_LargerList_AllOriginalReferencesPreserved_NoDuplicates()
    {
        // 10-item list in deliberately scrambled order; after sorting every
        // original reference must be present exactly once and the list length
        // must not change (no items created, dropped, or duplicated).
        var items = new List<MediaItem>
        {
            new AudioItem("Jupiter",  "Band J", TimeSpan.FromMinutes(4), "j.mp3"),
            new VideoItem("Alpha",    "Band A", TimeSpan.FromMinutes(5), "a.mp4", "1080p"),
            new AudioItem("Hotel",    "Band H", TimeSpan.FromMinutes(3), "h.mp3"),
            new VideoItem("Delta",    "Band D", TimeSpan.FromMinutes(7), "d.mp4", "720p"),
            new AudioItem("Echo",     "Band E", TimeSpan.FromMinutes(2), "e.mp3"),
            new AudioItem("Foxtrot",  "Band F", TimeSpan.FromMinutes(6), "f.mp3"),
            new VideoItem("Bravo",    "Band B", TimeSpan.FromMinutes(8), "b.mp4"),
            new AudioItem("India",    "Band I", TimeSpan.FromMinutes(1), "i.mp3"),
            new AudioItem("Golf",     "Band G", TimeSpan.FromMinutes(3), "g.mp3"),
            new VideoItem("Charlie",  "Band C", TimeSpan.FromMinutes(9), "c.mp4", "4K"),
        };

        // Capture originals before sorting.
        var originals = new List<MediaItem>(items);

        LibrarySorter.InsertionSort(items, SortKey.Title);

        // 1. Count must be unchanged.
        Assert.Equal(originals.Count, items.Count);

        // 2. Every original reference must appear in the sorted list exactly once.
        foreach (MediaItem original in originals)
        {
            int occurrences = 0;
            foreach (MediaItem sorted in items)
            {
                if (ReferenceEquals(original, sorted))
                {
                    occurrences++;
                }
            }
            Assert.Equal(1, occurrences);
        }

        // 3. Titles must be in ascending order.
        for (int i = 0; i < items.Count - 1; i++)
        {
            int cmp = string.Compare(items[i].Title, items[i + 1].Title,
                                     StringComparison.CurrentCultureIgnoreCase);
            Assert.True(cmp <= 0,
                $"Expected '{items[i].Title}' <= '{items[i + 1].Title}' at positions {i}/{i + 1}.");
        }
    }
}

