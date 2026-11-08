using System;

namespace mediaPlayerPrj;

/// <summary>A local MP3 recording with album metadata.</summary>
public sealed class AudioItem : MediaItem
{
    /// <summary>Album or dash when unknown.</summary>
    public string Album { get; set; }

    /// <summary>Creates an audio item.</summary>
    public AudioItem(string title, string artist, TimeSpan duration, string filePath, string album)
        : base(title, artist, duration, filePath)
    {
        Album = string.IsNullOrWhiteSpace(album) ? "-" : album;
    }

    /// <inheritdoc/>
    public override string MediaType => "Audio";

    /// <inheritdoc/>
    public override string Extra => Album;
}
