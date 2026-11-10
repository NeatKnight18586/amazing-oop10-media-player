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

