using System;

namespace mediaPlayerPrj;

/// <summary>Shared metadata for audio and video files, following Appendix A.</summary>
public abstract class MediaItem
{
    private string _title = "Unknown Title";
    private string _artist = "Unknown Artist";
    private TimeSpan _duration = TimeSpan.Zero;
    private string _filePath = string.Empty;

    /// <summary>Validated media title. Defaults to 'Unknown Title' when null or whitespace.</summary>
    public string Title
    {
        get => _title;
        set => _title = string.IsNullOrWhiteSpace(value) ? "Unknown Title" : value.Trim();
    }

    /// <summary>Artist or creator. Defaults to 'Unknown Artist' when null or whitespace.</summary>
    public string Artist
    {
        get => _artist;
        set => _artist = string.IsNullOrWhiteSpace(value) ? "Unknown Artist" : value.Trim();
    }

    /// <summary>Length of the media file, clamped to non-negative values.</summary>
    public TimeSpan Duration
    {
        get => _duration;
        set => _duration = value < TimeSpan.Zero ? TimeSpan.Zero : value;
    }

    /// <summary>Absolute local path to media file.</summary>
    public string FilePath
    {
        get => _filePath;
        set => _filePath = (value ?? string.Empty).Trim();
    }

    /// <summary>Constructs validated common metadata.</summary>
    protected MediaItem(string title, string artist, TimeSpan duration, string filePath)
    {
        Title = title;
        Artist = artist;
        Duration = duration;
        FilePath = filePath;
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
