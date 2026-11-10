using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace mediaPlayerPrj;

/// <summary>
/// Encapsulates the result of loading a media library from file or stream (FR3, FR7).
/// </summary>
public sealed class LibraryLoadResult
{
    /// <summary>Successfully parsed media items.</summary>
    public List<MediaItem> Items { get; }

    /// <summary>Count of malformed or invalid lines skipped during loading.</summary>
    public int SkippedLineCount { get; }

    /// <summary>Total number of lines encountered in the source.</summary>
    public int TotalLineCount { get; }

    /// <summary>Initializes a new instance of <see cref="LibraryLoadResult"/>.</summary>
    public LibraryLoadResult(List<MediaItem> items, int skippedLineCount, int totalLineCount)
    {
        Items = items ?? new List<MediaItem>();
        SkippedLineCount = Math.Max(0, skippedLineCount);
        TotalLineCount = Math.Max(0, totalLineCount);
    }
}

/// <summary>
/// Provides parsing and file I/O operations for the six-field media library (FR3, FR7).
/// Layout: Type|Title|Artist|HH:MM:SS|FilePath|AlbumOrResolution
/// </summary>
public static class LibraryStore
{
    public const int RequiredFieldCount = 6;
    public const string DefaultFileName = "Library.txt";

    public static string DefaultPath => Path.Combine(AppContext.BaseDirectory, DefaultFileName);

    /// <summary>
    /// Loads media items from the specified file path, returning parsed items and the count of skipped corrupt lines.
    /// If the file is genuinely missing (e.g., first run), returns an empty result without throwing (FR3, FR7).
    /// If an existing file is inaccessible or unreadable due to permissions or locking, an exception is thrown.
    /// </summary>
    /// <param name="filePath">Target file path, or null/empty to use <see cref="DefaultFileName"/>.</param>
    /// <returns>A <see cref="LibraryLoadResult"/> containing parsed media items and skipped line count.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when access to the existing file or directory is denied.</exception>
    /// <exception cref="IOException">Thrown when the existing file is locked, in use, or cannot be read.</exception>
    public static LibraryLoadResult Load(string? filePath = null)
    {
        string targetPath = string.IsNullOrWhiteSpace(filePath) ? DefaultFileName : filePath;

        try
        {
            using var stream = new FileStream(targetPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return Load(reader);
        }
        catch (FileNotFoundException)
        {
            return new LibraryLoadResult(new List<MediaItem>(), 0, 0);
        }
        catch (DirectoryNotFoundException)
        {
            return new LibraryLoadResult(new List<MediaItem>(), 0, 0);
        }
    }

    /// <summary>
    /// Loads media items from the specified file path, outputting the count of skipped corrupt lines.
    /// If the file does not exist, returns an empty list and sets skippedLineCount to 0 (FR3, FR7).
    /// </summary>
    /// <param name="filePath">Target file path, or null/empty to use <see cref="DefaultFileName"/>.</param>
    /// <param name="skippedLineCount">The number of invalid/corrupt lines skipped during loading.</param>
    /// <returns>A list of successfully parsed <see cref="MediaItem"/> objects.</returns>
    public static List<MediaItem> Load(string? filePath, out int skippedLineCount)
    {
        var result = Load(filePath);
        skippedLineCount = result.SkippedLineCount;
        return result.Items;
    }

    /// <summary>
    /// Loads media items from an open <see cref="TextReader"/>, counting skipped malformed lines.
    /// </summary>
    /// <param name="reader">The text reader containing library lines.</param>
    /// <returns>A <see cref="LibraryLoadResult"/> containing parsed items and skipped line count.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="reader"/> is null.</exception>
    public static LibraryLoadResult Load(TextReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var items = new List<MediaItem>();
        int skippedCount = 0;
        int totalLines = 0;

        bool sampleFormat = false;
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            totalLines++;
            if (line.Trim().Equals("Title|Artist|Album|Duration|Type|FilePath",
                StringComparison.OrdinalIgnoreCase))
            {
                sampleFormat = true;
                continue;
            }

            MediaItem? item = sampleFormat ? ParseSampleLine(line) : ParseLine(line);
            if (item != null)
                items.Add(item);
            else
                skippedCount++;
        }

        return new LibraryLoadResult(items, skippedCount, totalLines);
    }

    /// <summary>
    /// Persists media items to the specified file path in UTF-8 six-field format (FR3, Appendix B).
    /// Creates the target directory if it does not already exist.
    /// </summary>
    /// <param name="items">Collection of media items to persist.</param>
    /// <param name="filePath">Target file path, or null/empty to use <see cref="DefaultFileName"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when access to the file or directory is denied.</exception>
    /// <exception cref="IOException">Thrown when the file cannot be written, is locked, or disk error occurs.</exception>
    public static void Save(IEnumerable<MediaItem> items, string? filePath = null)
    {
        ArgumentNullException.ThrowIfNull(items);

        string targetPath = string.IsNullOrWhiteSpace(filePath) ? DefaultFileName : filePath;
        string? directory = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var stream = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.None);
        using var writer = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        Save(writer, items);
    }

    /// <summary>Compatibility overload used by the player form.</summary>
    public static void Save(string filePath, IEnumerable<MediaItem> items) =>
        Save(items, filePath);

    /// <summary>
    /// Serializes media items to an open <see cref="TextWriter"/> in six-field format.
    /// </summary>
    /// <param name="writer">Target text writer.</param>
    /// <param name="items">Media items to serialize.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="writer"/> or <paramref name="items"/> is null.</exception>
    public static void Save(TextWriter writer, IEnumerable<MediaItem> items)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(items);

        foreach (var item in items)
        {
            if (item != null)
            {
                writer.WriteLine(item.ToFileLine());
            }
        }
    }

    /// <summary>
    /// Parses a single line from Library.txt into a concrete <see cref="MediaItem"/> subclass.
    /// Returns null if the line is malformed, missing fields, or contains invalid data.
    /// </summary>
    /// <param name="line">Pipe-delimited text line.</param>
    /// <returns>A valid <see cref="AudioItem"/>, <see cref="VideoItem"/>, or null if malformed.</returns>
    public static MediaItem? ParseLine(string? line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return null;
        }

        var parts = line.Split('|');
        if (parts.Length != RequiredFieldCount)
        {
            return null;
        }

        string type = parts[0].Trim();
        string title = parts[1];
        string artist = parts[2];
        string durationRaw = parts[3];
        string filePath = parts[4];
        string extra = parts[5];

        if (!TryParseDuration(durationRaw, out var duration))
        {
            return null;
        }

        if (string.Equals(type, "Audio", StringComparison.OrdinalIgnoreCase))
        {
            return new AudioItem(title, artist, duration, filePath, extra);
        }

        if (string.Equals(type, "Video", StringComparison.OrdinalIgnoreCase))
        {
            return new VideoItem(title, artist, duration, filePath, extra);
        }

        // Unknown or unsupported media type
        return null;
    }

    /// <summary>Reads records from the professor's earlier sample header format.</summary>
    private static MediaItem? ParseSampleLine(string line)
    {
        string[] parts = line.Split('|');
        if (parts.Length != RequiredFieldCount || !TryParseDuration(parts[3], out var duration))
            return null;

        string type = parts[4].Trim();
        string path = parts[5].Trim();
        if (string.IsNullOrWhiteSpace(path)) return null;
        if (type.Equals("MP3", StringComparison.OrdinalIgnoreCase) ||
            type.Equals("Audio", StringComparison.OrdinalIgnoreCase))
            return new AudioItem(parts[0], parts[1], duration, path, parts[2]);

        if (type.Equals("MP4", StringComparison.OrdinalIgnoreCase) ||
            type.Equals("Video", StringComparison.OrdinalIgnoreCase))
            return new VideoItem(parts[0], parts[1], duration, path, parts[2]);

        return null;
    }

    /// <summary>
    /// Attempts to parse a line, returning true if successful.
    /// </summary>
    public static bool TryParseLine(string? line, out MediaItem? item)
    {
        item = ParseLine(line);
        return item != null;
    }

    /// <summary>
    /// Robustly parses HH:MM:SS or MM:SS duration strings into a <see cref="TimeSpan"/>.
    /// Handles durations spanning multiple hours without throwing.
    /// </summary>
    public static bool TryParseDuration(string? text, out TimeSpan duration)
    {
        duration = TimeSpan.Zero;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        var parts = text.Trim().Split(':');
        if (parts.Length == 3)
        {
            if (int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out int hours) &&
                int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out int minutes) &&
                int.TryParse(parts[2], NumberStyles.None, CultureInfo.InvariantCulture, out int seconds) &&
                hours >= 0 && minutes >= 0 && minutes < 60 && seconds >= 0 && seconds < 60)
            {
                duration = new TimeSpan(hours, minutes, seconds);
                return true;
            }
        }
        else if (parts.Length == 2)
        {
            if (int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out int minutes) &&
                int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out int seconds) &&
                minutes >= 0 && seconds >= 0 && seconds < 60)
            {
                duration = new TimeSpan(0, minutes, seconds);
                return true;
            }
        }

        return false;
    }
}
