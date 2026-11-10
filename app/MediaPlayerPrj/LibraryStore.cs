using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace mediaPlayerPrj;

/// <summary>Reads and writes local media records using the six fields in the project guide.</summary>
public static class LibraryStore
{
    /// <summary>Default library location beside the application executable.</summary>
    public static string DefaultPath => Path.Combine(AppContext.BaseDirectory, "Library.txt");

    /// <summary>Writes a library to a UTF-8 text file, one record per line.</summary>
    public static void Save(string filePath, IEnumerable<MediaItem> items)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentNullException.ThrowIfNull(items);

        string? directory = Path.GetDirectoryName(Path.GetFullPath(filePath));
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        var records = new List<string>();
        foreach (MediaItem item in items)
            records.Add(item.ToFileLine());

        // Write into the same directory so replacement is not a partial write.
        string temporary = filePath + ".tmp";
        try
        {
            File.WriteAllLines(temporary, records, new UTF8Encoding(false));
            File.Move(temporary, filePath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporary))
                File.Delete(temporary);
        }
    }

    /// <summary>Loads valid records, counting damaged lines rather than crashing.</summary>
    public static List<MediaItem> Load(string filePath, out int skippedLines)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        skippedLines = 0;
        var library = new List<MediaItem>();
        if (!File.Exists(filePath)) return library;

        bool legacyHeader = false;
        foreach (string raw in File.ReadLines(filePath, Encoding.UTF8))
        {
            if (string.IsNullOrWhiteSpace(raw))
                continue;

            // Professor Raymond's earlier Form1 example uses a different six-field
            // order and a header. Reading it is supported for existing libraries;
            // new saves always use the final guide's record order.
            if (raw.Trim().Equals("Title|Artist|Album|Duration|Type|FilePath",
                StringComparison.OrdinalIgnoreCase))
            {
                legacyHeader = true;
                continue;
            }

            if (TryParse(raw, legacyHeader, out MediaItem? item) && item != null)
                library.Add(item);
            else
                skippedLines++;
        }

        return library;
    }

    /// <summary>Parses one record from the final format, or the earlier header format.</summary>
    public static bool TryParse(string line, bool legacyFormat, out MediaItem? item)
    {
        item = null;
        if (string.IsNullOrWhiteSpace(line)) return false;

        string[] fields = line.Split('|');
        if (fields.Length != 6) return false;

        string type = (legacyFormat ? fields[4] : fields[0]).Trim();
        string title = (legacyFormat ? fields[0] : fields[1]).Trim();
        string artist = (legacyFormat ? fields[1] : fields[2]).Trim();
        string timeText = fields[3].Trim();
        string filePath = (legacyFormat ? fields[5] : fields[4]).Trim();
        string extra = (legacyFormat ? fields[2] : fields[5]).Trim();

        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        TimeSpan duration;
        if (legacyFormat && timeText.Split(':').Length == 2)
        {
            string[] parts = timeText.Split(':');
            if (!int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out int minutes) ||
                !int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out int seconds) ||
                minutes < 0 || seconds < 0 || seconds >= 60)
                return false;
            duration = TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds);
        }
        else if (!TimeSpan.TryParse(timeText, CultureInfo.InvariantCulture, out duration) ||
                 duration < TimeSpan.Zero)
        {
            return false;
        }

        if (type.Equals("Audio", StringComparison.OrdinalIgnoreCase) ||
            type.Equals("MP3", StringComparison.OrdinalIgnoreCase))
        {
            item = new AudioItem(title, artist, duration, filePath, extra);
            return true;
        }
        if (type.Equals("Video", StringComparison.OrdinalIgnoreCase) ||
            type.Equals("MP4", StringComparison.OrdinalIgnoreCase))
        {
            item = new VideoItem(title, artist, duration, filePath, extra);
            return true;
        }
        return false;
    }
}
