using System;
using System.IO;

namespace mediaPlayerPrj;

/// <summary>Shared metadata for audio and video files, following Appendix A.</summary>
public abstract class MediaItem
{
    private string _title = "Unknown Title";

    /// <summary>Validated media title.</summary>
    public string Title
    {
        get => _title;
        set => _title = string.IsNullOrWhiteSpace(value) ? "Unknown Title" : value.Trim();
    }

    /// <summary>Artist or creator.</summary>
    public string Artist { get; set; }

    /// <summary>Length of the media file.</summary>
    public TimeSpan Duration { get; set; }

    /// <summary>Absolute local path.</summary>
    public string FilePath { get; set; }

    /// <summary>Constructs validated common metadata.</summary>
    protected MediaItem(string title, string artist, TimeSpan duration, string filePath)
    {
        Title = title;
        Artist = string.IsNullOrWhiteSpace(artist) ? "Unknown Artist" : artist.Trim();
        Duration = duration < TimeSpan.Zero ? TimeSpan.Zero : duration;
        FilePath = filePath ?? string.Empty;
    }

    /// <summary>Audio or Video.</summary>
    public abstract string MediaType { get; }

    /// <summary>Album for audio or resolution for video.</summary>
    public abstract string Extra { get; }

    /// <summary>Total running time in HH:MM:SS format.</summary>
    public string DurationText =>
        string.Format("{0:00}:{1:00}:{2:00}", (int)Duration.TotalHours, Duration.Minutes, Duration.Seconds);

    /// <summary>Serializes the six fields specified in Appendix B of the final guide.</summary>
    public virtual string ToFileLine() =>
        string.Join("|", Clean(MediaType), Clean(Title), Clean(Artist),
            DurationText, Clean(FilePath), Clean(Extra));

    private static string Clean(string? value) => (value ?? string.Empty).Replace("|", "/");

    /// <inheritdoc/>
    public override string ToString() => Artist + " - " + Title;
}
