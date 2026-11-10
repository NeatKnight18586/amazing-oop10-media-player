using System;
using System.Collections.Generic;

namespace mediaPlayerPrj;

/// <summary>Supported playlist sort keys from the project guide.</summary>
public enum SortKey
{
    Title,
    Artist,
    AlbumOrResolution,
    Duration
}

/// <summary>Hand-written stable insertion sort; no built-in sorting method is used.</summary>
public static class LibrarySorter
{
    /// <summary>Compares two media items using the requested key.</summary>
    public static int Compare(MediaItem first, MediaItem second, SortKey key)
    {
        return key switch
        {
            SortKey.Artist => string.Compare(first.Artist, second.Artist, StringComparison.OrdinalIgnoreCase),
            SortKey.AlbumOrResolution => string.Compare(first.Extra, second.Extra, StringComparison.OrdinalIgnoreCase),
            SortKey.Duration => first.Duration.CompareTo(second.Duration),
            _ => string.Compare(first.Title, second.Title, StringComparison.OrdinalIgnoreCase)
        };
    }

    /// <summary>Sorts the supplied list in place using insertion sort.</summary>
    public static void InsertionSort(List<MediaItem> items, SortKey key)
    {
        ArgumentNullException.ThrowIfNull(items);
        for (int index = 1; index < items.Count; index++)
        {
            MediaItem moving = items[index];
            int previous = index - 1;
            while (previous >= 0 && Compare(items[previous], moving, key) > 0)
            {
                items[previous + 1] = items[previous];
                previous--;
            }
            items[previous + 1] = moving;
        }
    }
}
