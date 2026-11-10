using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;

namespace mediaPlayerPrj;

/// <summary>
/// Concrete VLC-enabled base form adapted from the instructor's Form1 sample
/// to meet the abstract/OOP hierarchy in the final project guide.
/// </summary>
public class BasePlayerForm : Form, IMediaControls
{
    private LibVLC? _libVLC;
    private MediaPlayer? _player;
    private string? _currentPath;

    /// <summary>Native VLC surface. Must be attached to a dedicated host panel.</summary>
    protected VideoView? VideoSurface;

    /// <summary>Polymorphic ordered playlist.</summary>
    protected List<MediaItem> Playlist = new();

    /// <summary>Index of the current playlist item; -1 means none selected.</summary>
    protected int CurrentIndex = -1;

    /// <summary>Initializes the VLC engine while remaining safe for the designer.</summary>
    public BasePlayerForm()
    {
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

        Core.Initialize();
        _libVLC = new LibVLC();
        _player = new MediaPlayer(_libVLC);
        VideoSurface = new VideoView
        {
            Dock = DockStyle.Fill,
            MediaPlayer = _player
        };
        _player.EndReached += Player_EndReached;
    }

    /// <summary>Embeds the native video view in the provided WinForms control.</summary>
    protected void AttachVideo(Control host)
    {
        if (VideoSurface != null)
            host.Controls.Add(VideoSurface);
    }

    /// <summary>Starts or resumes the selected playlist item.</summary>
    public virtual void Play()
    {
        if (_player == null || _libVLC == null) return;
        if (Playlist.Count == 0)
        {
            SetStatus("The library is empty. Add an MP3 or MP4 first.");
            return;
        }

        if (CurrentIndex < 0 || CurrentIndex >= Playlist.Count)
            CurrentIndex = 0;

        MediaItem item = Playlist[CurrentIndex];
        if (!File.Exists(item.FilePath))
        {
            SetStatus("This file is missing: " + item.FilePath);
            return;
        }

        try
        {
            if (string.Equals(_currentPath, item.FilePath, StringComparison.OrdinalIgnoreCase)
                && _player.State == VLCState.Paused)
            {
                _player.SetPause(false);
                SetStatus("Playing");
                OnNowPlayingChanged(item);
                return;
            }

            using Media media = new Media(_libVLC, item.FilePath, FromType.FromPath);
            _player.Play(media);
            _currentPath = item.FilePath;
            OnNowPlayingChanged(item);
            SetStatus("Playing: " + item.Title);
        }
        catch (Exception ex)
        {
            SetStatus("Unable to play this file: " + ex.Message);
        }
    }

    /// <summary>Pauses media without changing selection.</summary>
    public virtual void Pause()
    {
        if (_player == null) return;
        _player.SetPause(true);
        SetStatus("Paused");
    }

    /// <summary>Stops the current media.</summary>
    public virtual void Stop()
    {
        _player?.Stop();
        SetStatus("Stopped");
    }

    /// <summary>Plays the following item, wrapping to the start.</summary>
    public virtual void Next()
    {
        if (Playlist.Count == 0) return;
        CurrentIndex = (CurrentIndex + 1) % Playlist.Count;
        Play();
    }

    /// <summary>Plays the preceding item, wrapping to the end.</summary>
    public virtual void Previous()
    {
        if (Playlist.Count == 0) return;
        CurrentIndex = (CurrentIndex - 1 + Playlist.Count) % Playlist.Count;
        Play();
    }

    /// <summary>Sets the VLC playback volume from 0 to 100.</summary>
    public virtual void SetVolume(int percent)
    {
        if (_player != null)
            _player.Volume = Math.Clamp(percent, 0, 100);
    }

    /// <summary>Seeks to a normalized relative position from 0 to 1.</summary>
    public virtual void Seek(float position)
    {
        if (_player != null)
            _player.Position = Math.Clamp(position, 0f, 1f);
    }

    /// <summary>Gets elapsed playback time for the WinForms status clock.</summary>
    protected TimeSpan ElapsedTime =>
        TimeSpan.FromMilliseconds(Math.Max(0, _player?.Time ?? 0L));

    /// <summary>Updates the playlist while preserving the playing item by its file path.</summary>
    protected void SetPlaylist(List<MediaItem> items)
    {
        string? selected = CurrentIndex >= 0 && CurrentIndex < Playlist.Count
            ? Playlist[CurrentIndex].FilePath
            : null;
        Playlist = items ?? new List<MediaItem>();
        CurrentIndex = selected == null
            ? (Playlist.Count == 0 ? -1 : 0)
            : Playlist.FindIndex(item =>
                string.Equals(item.FilePath, selected, StringComparison.OrdinalIgnoreCase));
        if (CurrentIndex < 0 && Playlist.Count > 0) CurrentIndex = 0;
    }

    /// <summary>Retrieves local metadata using the instructor's Media.Parse approach.</summary>
    protected async Task<MediaItem> ReadMediaItemAsync(string path)
    {
        if (_libVLC == null) throw new InvalidOperationException("VLC is not initialized.");
        string ext = Path.GetExtension(path).ToLowerInvariant();
        if (ext != ".mp3" && ext != ".mp4")
            throw new NotSupportedException("Only MP3 and MP4 are supported.");
        if (!File.Exists(path)) throw new FileNotFoundException("Missing media", path);

        using Media media = new Media(_libVLC, path, FromType.FromPath);
        await media.Parse(MediaParseOptions.ParseLocal);
        string title = media.Meta(MetadataType.Title) ?? "";
        if (string.IsNullOrWhiteSpace(title))
            title = Path.GetFileNameWithoutExtension(path);

        string artist = media.Meta(MetadataType.Artist) ?? "";
        if (string.IsNullOrWhiteSpace(artist)) artist = "Unknown Artist";

        TimeSpan duration = TimeSpan.FromMilliseconds(Math.Max(0, media.Duration));
        if (ext == ".mp3")
        {
            string album = media.Meta(MetadataType.Album) ?? "-";
            return new AudioItem(title, artist, duration, path, album);
        }

        string resolution = "Unknown";
        foreach (MediaTrack track in media.Tracks)
        {
            if (track.TrackType != TrackType.Video) continue;
            uint width = track.Data.Video.Width;
            uint height = track.Data.Video.Height;
            if (width > 0 && height > 0)
            {
                resolution = width + "x" + height;
                break;
            }
        }
        return new VideoItem(title, artist, duration, path, resolution);
    }

    /// <summary>Notifies the derived form that the playing item changed.</summary>
    protected virtual void OnNowPlayingChanged(MediaItem item) { }

    /// <summary>Lets a derived form apply its visual skin.</summary>
    protected virtual void ApplyTheme() { }

    /// <summary>Displays an actionable status message in the derived form.</summary>
    protected virtual void SetStatus(string text) { }

    private void Player_EndReached(object? sender, EventArgs e)
    {
        if (!IsHandleCreated || IsDisposed || Disposing) return;

        try
        {
            BeginInvoke((Action)(() =>
            {
                if (!IsDisposed && !Disposing)
                    PlayNextOrStop();
            }));
        }
        catch (InvalidOperationException)
        {
            // Window disposed between the handle check and BeginInvoke.
        }
    }

    /// <summary>Default end-of-item behavior; Chris will extend this for FR13.</summary>
    protected virtual void PlayNextOrStop()
    {
        if (Playlist.Count == 0 || CurrentIndex >= Playlist.Count - 1)
            Stop();
        else
            Next();
    }

    /// <inheritdoc/>
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        if (_player != null)
        {
            _player.EndReached -= Player_EndReached;
            VideoSurface!.MediaPlayer = null;
            _player.Dispose();
            _player = null;
        }
        _libVLC?.Dispose();
        _libVLC = null;
        base.OnFormClosed(e);
    }
}
