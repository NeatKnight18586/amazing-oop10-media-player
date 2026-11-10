using System;

namespace mediaPlayerPrj;

/// <summary>A local MP3 recording with album metadata.</summary>
public sealed class AudioItem : MediaItem
{
    private string _album = "-";

    /// <summary>Album name, or dash (-) when unknown.</summary>
    public string Album
    {
        get => _album;
        set => _album = string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();
    }

    /// <summary>Creates an audio item with validated album metadata.</summary>
    public AudioItem(string title, string artist, TimeSpan duration, string filePath, string? album = "-")
        : base(title, artist, duration, filePath)
    {
        Album = album!;
    }

    /// <inheritdoc/>
    public override string MediaType => "Audio";

    /// <inheritdoc/>
    public override string Extra => Album;
}
