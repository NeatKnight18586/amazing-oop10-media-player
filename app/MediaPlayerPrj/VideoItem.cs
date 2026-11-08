using System;

namespace mediaPlayerPrj;

/// <summary>A local MP4 video with resolution metadata.</summary>
public sealed class VideoItem : MediaItem
{
    /// <summary>Video resolution when available.</summary>
    public string Resolution { get; set; }

    /// <summary>Creates a video item.</summary>
    public VideoItem(string title, string artist, TimeSpan duration, string filePath, string resolution)
        : base(title, artist, duration, filePath)
    {
        Resolution = string.IsNullOrWhiteSpace(resolution) ? "Unknown" : resolution;
    }

    /// <inheritdoc/>
    public override string MediaType => "Video";

    /// <inheritdoc/>
    public override string Extra => Resolution;
}
