using System;
using System.Collections.Generic;
using System.IO;

namespace mediaPlayerPrj;

/// <summary>
/// Reads and writes the user's favorite media paths to a plain-text file
/// (<c>Favorites.txt</c>), entirely separate from the required six-field
/// <c>Library.txt</c> format. Each line stores one normalized absolute
/// file path. Comparisons are case-insensitive to match Windows NTFS behavior.
/// </summary>
public class FavoritesStore
{
    /// <summary>Default file name for favorites persistence.</summary>
    public const string DefaultFileName = "Favorites.txt";

    private readonly string _filePath;

    /// <summary>
    /// Creates a store bound to the given favorites text file.
    /// </summary>
    /// <param name="filePath">
    /// Absolute or relative path to the text file.  When <c>null</c> or
    /// blank, defaults to <c>Favorites.txt</c> beside the running executable.
    /// </param>
    public FavoritesStore(string? filePath = null)
    {
        _filePath = string.IsNullOrWhiteSpace(filePath)
            ? Path.Combine(AppContext.BaseDirectory, DefaultFileName)
            : filePath;
    }

    /// <summary>Gets the resolved path of the backing text file.</summary>
    public string FilePath => _filePath;

    /// <summary>
    /// Loads favorite paths from the backing text file.
    /// Returns an empty set when the file does not exist (first-run).
    /// Skips blank, whitespace-only, duplicate, and malformed lines.
    /// </summary>
    /// <param name="skippedCount">
    /// The number of lines that were skipped because they were blank,
    /// duplicate, or contained an invalid path.
    /// </param>
    /// <returns>
    /// A case-insensitive <see cref="HashSet{String}"/> of normalized paths.
    /// </returns>
    public HashSet<string> Load(out int skippedCount)
    {
        skippedCount = 0;
        var favorites = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (!File.Exists(_filePath))
            return favorites;

        string[] lines;
        try
        {
            lines = File.ReadAllLines(_filePath);
        }
        catch (IOException ex)
        {
            throw new FavoritesIOException(
                "Could not read favorites file: " + _filePath, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new FavoritesIOException(
                "Access denied reading favorites file: " + _filePath, ex);
        }

        foreach (string raw in lines)
        {
            // Skip blank and whitespace-only lines.
            if (string.IsNullOrWhiteSpace(raw))
            {
                skippedCount++;
                continue;
            }

            string? normalized = NormalizePath(raw);
            if (normalized == null)
            {
                // Line contains invalid path characters.
                skippedCount++;
                continue;
            }

            // Duplicates are silently absorbed by the HashSet;
            // count them so callers can report if desired.
            if (!favorites.Add(normalized))
                skippedCount++;
        }

        return favorites;
    }

    /// <summary>
    /// Persists the given favorite paths to the backing text file,
    /// one normalized path per line, encoded as UTF-8 without BOM.
    /// </summary>
    /// <param name="paths">Paths to write; <c>null</c> entries are ignored.</param>
    /// <exception cref="FavoritesIOException">
    /// Wraps any underlying I/O error with a user-friendly message.
    /// </exception>
    public void Save(IEnumerable<string> paths)
    {
        // Stub — implemented in increment 4.
    }

    /// <summary>
    /// Normalizes a file path for consistent, case-insensitive matching:
    /// trims surrounding whitespace, resolves to an absolute path via
    /// <see cref="Path.GetFullPath(string)"/>, and normalizes directory
    /// separators for the current platform.
    /// </summary>
    /// <param name="path">A raw path string from any input source.</param>
    /// <returns>
    /// The cleaned absolute path, or <c>null</c> when the input is
    /// <c>null</c>, blank, or contains characters illegal in a file path.
    /// </returns>
    public static string? NormalizePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        try
        {
            return Path.GetFullPath(path.Trim());
        }
        catch (Exception) // ArgumentException, NotSupportedException, PathTooLongException
        {
            return null;
        }
    }
}

/// <summary>
/// Indicates that favorites persistence encountered a file-system error.
/// Wraps the underlying <see cref="IOException"/> with a user-facing message.
/// </summary>
public class FavoritesIOException : Exception
{
    /// <summary>Creates a favorites I/O exception with a message and inner cause.</summary>
    public FavoritesIOException(string message, Exception inner)
        : base(message, inner) { }
}
