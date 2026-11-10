using System;
using System.Globalization;

namespace mediaPlayerPrj;

/// <summary>
/// Provides parsing and file I/O operations for the six-field media library (FR3, FR7).
/// Layout: Type|Title|Artist|HH:MM:SS|FilePath|AlbumOrResolution
/// </summary>
public static class LibraryStore
{
    public const int RequiredFieldCount = 6;

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

