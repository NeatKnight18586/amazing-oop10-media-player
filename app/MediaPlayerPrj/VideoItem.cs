using System;

namespace mediaPlayerPrj;

/// <summary>A local MP4 video with resolution metadata.</summary>
public sealed class VideoItem : MediaItem
{
    private string _resolution = "Unknown";

    /// <summary>Video resolution (e.g., 1920x1080), or Unknown when unavailable.</summary>
    public string Resolution
    {
        get => _resolution;
        set => _resolution = string.IsNullOrWhiteSpace(value) ? "Unknown" : value.Trim();
    }

    /// <summary>Creates a video item with validated resolution metadata.</summary>
    public VideoItem(string title, string artist, TimeSpan duration, string filePath, string? resolution = "Unknown")
        : base(title, artist, duration, filePath)
    {
        Resolution = resolution!;
    }

    /// <inheritdoc/>
    public override string MediaType => "Video";

    /// <inheritdoc/>
    public override string Extra => Resolution;
}
