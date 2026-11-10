using System;
using System.Collections.Generic;

namespace mediaPlayerPrj;

/// <summary>
/// Service managing user favorite media tracks (FR8).
/// Maintains a collection of normalized favorite file paths backed by <see cref="FavoritesStore"/>
/// and persists them separately from Library.txt.
/// </summary>
public class FavoritesService
{
    private readonly FavoritesStore _store;
    private readonly HashSet<string> _favorites;

    /// <summary>Number of lines skipped during the last load operation due to format/duplicate issues.</summary>
    public int LastSkippedCount { get; private set; }

    /// <summary>Total number of currently tracked unique favorite paths.</summary>
    public int Count => _favorites.Count;

    /// <summary>
    /// Initializes a new instance of <see cref="FavoritesService"/> using the specified store.
    /// Automatically loads persisted favorites on startup.
    /// </summary>
    /// <param name="store">The favorites persistence store, or null to use the default.</param>
    public FavoritesService(FavoritesStore? store = null)
    {
        _store = store ?? new FavoritesStore();
        _favorites = _store.Load(out int skipped);
        LastSkippedCount = skipped;
    }

    /// <summary>
    /// Adds a file path to favorites and persists the change.
    /// </summary>
    /// <param name="path">The media file path to favorite.</param>
    /// <returns>True if the path was valid and newly added; false if already a favorite or invalid.</returns>
    public bool Add(string? path)
    {
        string? normalized = FavoritesStore.NormalizePath(path);
        if (normalized == null)
            return false;

        if (_favorites.Add(normalized))
        {
            Save();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Removes a file path from favorites and persists the change.
    /// </summary>
    /// <param name="path">The media file path to unfavorite.</param>
    /// <returns>True if the path was present and removed; false if not found or invalid.</returns>
    public bool Remove(string? path)
    {
        string? normalized = FavoritesStore.NormalizePath(path);
        if (normalized == null)
            return false;

        if (_favorites.Remove(normalized))
        {
            Save();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Toggles the favorite status for a given file path (FR8 Heart button).
    /// If currently favorited, removes it; otherwise adds it.
    /// </summary>
    /// <param name="path">The media file path.</param>
    /// <returns>True if the item is now a favorite; false if it is now unfavorited or path was invalid.</returns>
    public bool Toggle(string? path)
    {
        string? normalized = FavoritesStore.NormalizePath(path);
        if (normalized == null)
            return false;

        if (_favorites.Contains(normalized))
        {
            _favorites.Remove(normalized);
            Save();
            return false;
        }
        else
        {
            _favorites.Add(normalized);
            Save();
            return true;
        }
    }

    /// <summary>Alias for <see cref="Toggle"/> to explicitly match UI naming requirements.</summary>
    public bool ToggleFavorite(string? path) => Toggle(path);

    /// <summary>
    /// Explicitly persists current favorites to the backing store.
    /// </summary>
    public void Save()
    {
        _store.Save(_favorites);
    }

    /// <summary>
    /// Reloads favorites from the backing store.
    /// </summary>
    public void Reload()
    {
        _favorites.Clear();
        var loaded = _store.Load(out int skipped);
        foreach (var item in loaded)
        {
            _favorites.Add(item);
        }
        LastSkippedCount = skipped;
    }
}
