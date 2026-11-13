using System;
using System.Collections.Generic;

namespace mediaPlayerPrj;

/// <summary>
/// Specifies the field by which media items are sorted (FR6, FR12).
/// </summary>
public enum SortKey
{
    /// <summary>Sort alphabetically by track or media title.</summary>
    Title,

    /// <summary>Sort alphabetically by artist or creator name.</summary>
    Artist
}

/// <summary>
/// Provides comparison and insertion sort algorithms for the media library playlist (FR6, FR12).
/// </summary>
public static class LibrarySorter
{
    /// <summary>
    /// Compares two <see cref="MediaItem"/> instances according to the specified <see cref="SortKey"/> (FR6, FR12).
    /// Comparison is case-insensitive using current culture rules.
    /// Handles null items and null/empty fields gracefully.
    /// </summary>
    /// <param name="x">First item to compare.</param>
    /// <param name="y">Second item to compare.</param>
    /// <param name="key">The sort field to compare by.</param>
    /// <returns>
    /// Negative integer if <paramref name="x"/> precedes <paramref name="y"/>;
    /// Zero if <paramref name="x"/> and <paramref name="y"/> have identical sort keys;
    /// Positive integer if <paramref name="x"/> follows <paramref name="y"/>.
    /// </returns>
    public static int Compare(MediaItem? x, MediaItem? y, SortKey key)
    {
        if (!Enum.IsDefined(typeof(SortKey), key))
        {
            throw new ArgumentOutOfRangeException(nameof(key), key, $"Unsupported sort key: {key}");
        }

        if (ReferenceEquals(x, y))
        {
            return 0;
        }

        if (x is null)
        {
            return -1;
        }

        if (y is null)
        {
            return 1;
        }

        string valX = GetSortKey(x, key);
        string valY = GetSortKey(y, key);

        return string.Compare(valX, valY, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>
    /// Extracts the string value corresponding to the specified <see cref="SortKey"/>.
    /// </summary>
    /// <param name="item">Media item from which to extract the key.</param>
    /// <param name="key">The target sort field.</param>
    /// <returns>The string value of the key, or empty string if item is null.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an undefined <see cref="SortKey"/> is passed.</exception>
    public static string GetSortKey(MediaItem? item, SortKey key)
    {
        if (item is null)
        {
            return string.Empty;
        }

        return key switch
        {
            SortKey.Title => item.Title ?? string.Empty,
            SortKey.Artist => item.Artist ?? string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(key), key, $"Unsupported sort key: {key}")
        };
    }

    /// <summary>
    /// Returns an <see cref="IComparer{MediaItem}"/> instance that evaluates items using the specified <see cref="SortKey"/>.
    /// </summary>
    /// <param name="key">The primary sort field.</param>
    /// <returns>A reusable comparer for <see cref="MediaItem"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an undefined <see cref="SortKey"/> is passed.</exception>
    public static IComparer<MediaItem> GetComparer(SortKey key)
    {
        if (!Enum.IsDefined(typeof(SortKey), key))
        {
            throw new ArgumentOutOfRangeException(nameof(key), key, $"Unsupported sort key: {key}");
        }

        return new SortKeyComparer(key);
    }

    /// <summary>
    /// Sorts <paramref name="items"/> in place using a stable, hand-written insertion sort (FR12).
    /// Equal items (by the chosen <paramref name="key"/>) keep the same relative order they had
    /// before the call, which satisfies the stability requirement.
    /// </summary>
    /// <param name="items">The list of media items to sort. Must not be null.</param>
    /// <param name="key">The field to sort by: <see cref="SortKey.Title"/> or <see cref="SortKey.Artist"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="key"/> is not a defined <see cref="SortKey"/> value.</exception>
    public static void InsertionSort(List<MediaItem> items, SortKey key)
    {
        if (items is null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        if (!Enum.IsDefined(typeof(SortKey), key))
        {
            throw new ArgumentOutOfRangeException(nameof(key), key, $"Unsupported sort key: {key}");
        }

        // 0 or 1 items are trivially sorted.
        if (items.Count <= 1)
        {
            return;
        }

        // Standard insertion sort:
        // Walk forward from index 1; take the current item and shift
        // every preceding item that compares *greater* one slot to the right,
        // then drop the current item into the gap.
        // Because we only shift items that are strictly greater (Compare > 0),
        // equal items are never moved past each other → the sort is stable.
        for (int i = 1; i < items.Count; i++)
        {
            MediaItem current = items[i];
            int j = i - 1;

            while (j >= 0 && Compare(items[j], current, key) > 0)
            {
                items[j + 1] = items[j];
                j--;
            }

            items[j + 1] = current;
        }
    }

    private sealed class SortKeyComparer : IComparer<MediaItem>
    {
        private readonly SortKey _key;

        public SortKeyComparer(SortKey key)
        {
            _key = key;
        }

        public int Compare(MediaItem? x, MediaItem? y) => LibrarySorter.Compare(x, y, _key);
    }
}

