using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mediaPlayerPrj;

/// <summary>Main player window, using controls created by the Windows Forms Designer.</summary>
public partial class Form1 : BasePlayerForm
{
    private readonly PlaybackOrder _playbackOrder = new();
    private readonly HashSet<string> _extensions =
        new(StringComparer.OrdinalIgnoreCase) { ".mp3", ".mp4" };
    private readonly System.Windows.Forms.Timer _elapsedTimer = new() { Interval = 500 };
    private FavoritesService _favorites = null!;
    private bool _busy;
    private bool _seeking;

    /// <summary>Creates the player window and attaches the native video surface.</summary>
    public Form1()
    {
        InitializeComponent();
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        AttachVideo(pnlVideo);
        pnlVideo.Visible = false;
        InitializeFavorites();
        ConnectControls();
        ApplyTheme();
        SetVolume(trkVolume.Value);
        _elapsedTimer.Tick += (_, _) => UpdateElapsed();
        _elapsedTimer.Start();
        Load += (_, _) => LoadLibrary();
    }

    private void InitializeFavorites()
    {
        try
        {
            _favorites = new FavoritesService();
        }
        catch (FavoritesIOException ex)
        {
            // The app should remain usable even when its normal data folder is read-only.
            string backup = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "AmazingPlayer", "Favorites.txt");
            _favorites = new FavoritesService(new FavoritesStore(backup));
            SetStatus("Favorites moved to user data folder: " + ex.Message);
        }
    }

    private void ConnectControls()
    {
        btnAdd.Click += async (_, _) => await AddFilesAsync();
        btnPlay.Click += (_, _) => PlaySelected();
        btnPause.Click += (_, _) => Pause();
        btnStop.Click += (_, _) => Stop();
        btnPrevious.Click += (_, _) => Previous();
        btnNext.Click += (_, _) => Next();
        btnFavorite.Click += (_, _) => ToggleFavorite();
        btnShuffle.Click += (_, _) => ToggleShuffle();
        btnRepeat.Click += (_, _) => CycleRepeat();
        btnSortTitle.Click += (_, _) => SortLibrary(SortKey.Title);
        btnSortArtist.Click += (_, _) => SortLibrary(SortKey.Artist);
        chkFavoritesOnly.CheckedChanged += (_, _) => RefreshListView();
        txtSearch.TextChanged += (_, _) => RefreshListView();
        lstPlaylist.DoubleClick += (_, _) => PlaySelected();
        lstPlaylist.SelectedIndexChanged += (_, _) => UpdateFavoriteButton();

        trkVolume.Scroll += (_, _) => SetVolume(trkVolume.Value);
        trkSeek.MouseDown += (_, _) => _seeking = true;
        trkSeek.MouseUp += (_, _) =>
        {
            if (trkSeek.Enabled)
                Seek(trkSeek.Value / 1000f);
            _seeking = false;
        };

        miAdd.Click += async (_, _) => await AddFilesAsync();
        miSave.Click += (_, _) => SaveLibrary();
        miReload.Click += (_, _) => LoadLibrary();
        miPlay.Click += (_, _) => PlaySelected();
        miPause.Click += (_, _) => Pause();
        miStop.Click += (_, _) => Stop();
        miNext.Click += (_, _) => Next();
        miPrevious.Click += (_, _) => Previous();
        miShuffle.Click += (_, _) => ToggleShuffle();
        miRepeat.Click += (_, _) => CycleRepeat();
        miSortTitle.Click += (_, _) => SortLibrary(SortKey.Title);
        miSortArtist.Click += (_, _) => SortLibrary(SortKey.Artist);
        miFavorites.Click += (_, _) =>
            chkFavoritesOnly.Checked = !chkFavoritesOnly.Checked;
        miAbout.Click += (_, _) =>
        {
            using var about = new SplashForm();
            about.ShowDialog(this);
        };
    }

    private async Task AddFilesAsync()
    {
        if (_busy) return;

        using var dialog = new OpenFileDialog
        {
            Filter = "Media files (*.mp3;*.mp4)|*.mp3;*.mp4|All files (*.*)|*.*",
            Multiselect = true,
            Title = "Choose music or video files"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        _busy = true;
        btnAdd.Enabled = false;
        miAdd.Enabled = false;
        try
        {
            var knownPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (MediaItem item in Playlist)
                knownPaths.Add(item.FilePath);

            int added = 0;
            int failed = 0;
            foreach (string filePath in dialog.FileNames)
            {
                if (!_extensions.Contains(Path.GetExtension(filePath)) || !knownPaths.Add(filePath))
                    continue;
                try
                {
                    MediaItem item = await ReadMediaItemAsync(filePath);
                    Playlist.Add(item);
                    added++;
                }
                catch (Exception ex)
                {
                    failed++;
                    SetStatus("Skipped " + Path.GetFileName(filePath) + ": " + ex.Message);
                }
            }

            if (CurrentIndex < 0 && Playlist.Count > 0)
                CurrentIndex = 0;
            _playbackOrder.ResetShuffle();
            RefreshListView();
            if (added > 0) SaveLibrary();
            SetStatus($"Added {added} media file(s). Could not load: {failed}.");
        }
        finally
        {
            _busy = false;
            btnAdd.Enabled = true;
            miAdd.Enabled = true;
        }
    }

    private void LoadLibrary()
    {
        try
        {
            List<MediaItem> items = LibraryStore.Load(LibraryStore.DefaultPath, out int skipped);
            SetPlaylist(items);
            _playbackOrder.ResetShuffle();
            RefreshListView();
            SetStatus($"Loaded {items.Count} item(s) from Library.txt. Skipped {skipped} invalid record(s).");
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            SetStatus("Unable to read library: " + ex.Message);
            MessageBox.Show(this, ex.Message, "Library error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void SaveLibrary()
    {
        try
        {
            LibraryStore.Save(LibraryStore.DefaultPath, Playlist);
            SetStatus("Library saved.");
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            SetStatus("Unable to save library: " + ex.Message);
            MessageBox.Show(this, ex.Message, "Library error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void RefreshListView()
    {
        MediaItem? selectedItem = CurrentIndex >= 0 && CurrentIndex < Playlist.Count
            ? Playlist[CurrentIndex] : null;
        string search = txtSearch.Text.Trim();
        lstPlaylist.BeginUpdate();
        try
        {
            lstPlaylist.Items.Clear();
            foreach (MediaItem item in Playlist)
            {
                if (chkFavoritesOnly.Checked && !_favorites.IsFavorite(item))
                    continue;
                if (search.Length > 0 &&
                    item.Title.IndexOf(search, StringComparison.OrdinalIgnoreCase) < 0 &&
                    item.Artist.IndexOf(search, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                var row = new ListViewItem((_favorites.IsFavorite(item) ? "♥ " : "") + item.Title)
                {
                    Tag = item,
                    ToolTipText = item.FilePath
                };
                row.SubItems.Add(item.Artist);
                row.SubItems.Add(item.Extra);
                row.SubItems.Add(item.DurationText);
                row.SubItems.Add(item.MediaType);
                row.SubItems.Add(item.FilePath);
                lstPlaylist.Items.Add(row);
                if (ReferenceEquals(item, selectedItem))
                    row.Selected = true;
            }
        }
        finally
        {
            lstPlaylist.EndUpdate();
        }
        UpdateFavoriteButton();
    }

    private void PlaySelected()
    {
        if (lstPlaylist.SelectedItems.Count > 0 &&
            lstPlaylist.SelectedItems[0].Tag is MediaItem item)
        {
            int index = Playlist.FindIndex(existing => ReferenceEquals(existing, item));
            if (index >= 0) CurrentIndex = index;
        }
        Play();
    }

    private void SortLibrary(SortKey key)
    {
        if (Playlist.Count < 2)
        {
            SetStatus("Add more media before sorting.");
            return;
        }
        var sorted = new List<MediaItem>(Playlist);
        LibrarySorter.InsertionSort(sorted, key);
        SetPlaylist(sorted);
        _playbackOrder.ResetShuffle();
        RefreshListView();
        SaveLibrary();
        SetStatus("Sorted playlist by " + key + ".");
    }

    private void ToggleFavorite()
    {
        MediaItem? selected = lstPlaylist.SelectedItems.Count > 0
            ? lstPlaylist.SelectedItems[0].Tag as MediaItem
            : (CurrentIndex >= 0 && CurrentIndex < Playlist.Count ? Playlist[CurrentIndex] : null);
        if (selected == null)
        {
            SetStatus("Select a track to add to favorites.");
            return;
        }
        try
        {
            bool liked = _favorites.ToggleFavorite(selected.FilePath);
            RefreshListView();
            SetStatus(liked ? "Added to favorites." : "Removed from favorites.");
        }
        catch (FavoritesIOException ex)
        {
            SetStatus(ex.Message);
            MessageBox.Show(this, ex.Message, "Favorites error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void UpdateFavoriteButton()
    {
        MediaItem? selected = lstPlaylist.SelectedItems.Count > 0
            ? lstPlaylist.SelectedItems[0].Tag as MediaItem
            : (CurrentIndex >= 0 && CurrentIndex < Playlist.Count ? Playlist[CurrentIndex] : null);
        btnFavorite.Text = selected != null && _favorites.IsFavorite(selected)
            ? "♥  Unlike" : "♡  Like";
    }

    private void ToggleShuffle()
    {
        _playbackOrder.Shuffle = !_playbackOrder.Shuffle;
        string label = "Shuffle: " + (_playbackOrder.Shuffle ? "On" : "Off");
        btnShuffle.Text = label;
        miShuffle.Text = label;
        SetStatus(label);
    }

    private void CycleRepeat()
    {
        _playbackOrder.Repeat = _playbackOrder.Repeat.NextMode();
        string label = _playbackOrder.Repeat.ToDisplayText();
        btnRepeat.Text = label;
        miRepeat.Text = label;
        SetStatus(label);
    }

    /// <summary>
    /// Prepares the native video panel before LibVLC receives the Play command.
    /// The cassette art remains visible for audio tracks.
    /// </summary>
    public override void Play()
    {
        if (CurrentIndex >= 0 && CurrentIndex < Playlist.Count)
        {
            bool video = Playlist[CurrentIndex] is VideoItem;
            pnlVideo.Visible = video;
            if (video)
            {
                pnlVideo.BringToFront();
                _ = pnlVideo.Handle;
                if (VideoSurface != null)
                    _ = VideoSurface.Handle;
            }
        }
        base.Play();
    }

    /// <inheritdoc/>
    public override void Next()
    {
        int next = _playbackOrder.GetNextIndex(
            CurrentIndex, Playlist.Count, AdvanceReason.UserNext);
        if (next == PlaybackOrder.NoNextIndex) return;
        CurrentIndex = next;
        Play();
    }

    /// <inheritdoc/>
    public override void Stop()
    {
        base.Stop();
        trkSeek.Value = 0;
        UpdateElapsed();
    }

    /// <inheritdoc/>
    protected override void PlayNextOrStop()
    {
        int next = _playbackOrder.GetNextIndex(
            CurrentIndex, Playlist.Count, AdvanceReason.MediaEnded);
        if (next == PlaybackOrder.NoNextIndex)
            Stop();
        else
        {
            CurrentIndex = next;
            Play();
        }
    }

    /// <inheritdoc/>
    protected override void OnNowPlayingChanged(MediaItem item)
    {
        string title = item.Title;
        lblCassetteTitle.Text = title.Length > 31 ? title[..28] + "..." : title;
        lblNowPlaying.Text = item.Artist + " — " + item.Title +
            (item.Extra == "-" ? "" : "  •  " + item.Extra);

        bool video = item is VideoItem;
        pnlVideo.Visible = video;
        if (video) pnlVideo.BringToFront();
        trkSeek.Enabled = true;
        for (int index = 0; index < lstPlaylist.Items.Count; index++)
        {
            if (ReferenceEquals(lstPlaylist.Items[index].Tag, item))
            {
                lstPlaylist.Items[index].Selected = true;
                break;
            }
        }
        UpdateFavoriteButton();
        UpdateElapsed();
    }

    private void UpdateElapsed()
    {
        if (CurrentIndex < 0 || CurrentIndex >= Playlist.Count)
        {
            lblElapsed.Text = "00:00:00 / 00:00:00";
            if (!_seeking) trkSeek.Value = 0;
            return;
        }
        TimeSpan duration = Playlist[CurrentIndex].Duration;
        lblElapsed.Text = FormatTime(ElapsedTime) + " / " + FormatTime(duration);
        if (!_seeking && duration.TotalMilliseconds > 0)
        {
            int progress = (int)Math.Clamp(
                ElapsedTime.TotalMilliseconds / duration.TotalMilliseconds * 1000.0, 0, 1000);
            trkSeek.Value = progress;
        }
    }

    private static string FormatTime(TimeSpan time) =>
        string.Format("{0:00}:{1:00}:{2:00}",
            (int)time.TotalHours, time.Minutes, time.Seconds);

    /// <inheritdoc/>
    protected override void SetStatus(string text)
    {
        if (lblStatus != null)
            lblStatus.Text = "Status: " + text;
    }

    /// <inheritdoc/>
    protected override void ApplyTheme()
    {
        BackColor = Theme.Canvas;
        ForeColor = Theme.Ink;
        pnlCassette.BackColor = Theme.CassetteBay;
        pnlDetails.BackColor = Theme.Paper;
        flowTransport.BackColor = Theme.Gold;
        flowSeek.BackColor = Theme.LightGold;
        lblCassetteTitle.ForeColor = Theme.Ink;
        Image? cassette = Theme.LoadImage("cassette_art.png");
        if (cassette != null)
            pnlCassette.BackgroundImage = cassette;
        pnlCassette.BackgroundImageLayout = ImageLayout.Stretch;
        Image? titlePaper = Theme.LoadImage("paper_texture.png");
        if (titlePaper != null)
        {
            lblCassetteTitle.BackgroundImage = titlePaper;
            lblCassetteTitle.BackgroundImageLayout = ImageLayout.Tile;
        }
        Image? texture = Theme.LoadImage("paper_texture.png");
        if (texture != null)
        {
            layoutRoot.BackgroundImage = texture;
            layoutRoot.BackgroundImageLayout = ImageLayout.Tile;
            flowTransport.BackgroundImage = Theme.LoadImage("paper_texture.png");
            flowTransport.BackgroundImageLayout = ImageLayout.Tile;
            flowSeek.BackgroundImage = Theme.LoadImage("paper_texture.png");
            flowSeek.BackgroundImageLayout = ImageLayout.Tile;
        }

        Button[] styledButtons = {
            btnPrevious, btnPlay, btnPause, btnStop, btnNext,
            btnAdd, btnFavorite, btnShuffle, btnRepeat
        };
        foreach (Button button in styledButtons)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(123, 81, 45);
            button.BackColor = Color.FromArgb(235, 194, 134);
            button.ForeColor = Theme.Ink;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }
        ApplyTransportIcons();
        Icon? appIcon = Theme.TryLoadIcon("logo.ico");
        if (appIcon != null)
            Icon = appIcon;
    }

    /// <summary>Displays the existing group-designed transport icons on Designer buttons.</summary>
    private void ApplyTransportIcons()
    {
        var transport = new (Button Button, string Name)[]
        {
            (btnAdd, "add"), (btnPrevious, "prev"), (btnPlay, "play"),
            (btnPause, "pause"), (btnStop, "stop"), (btnNext, "next")
        };
        foreach (var (button, name) in transport)
        {
            Image? icon = Theme.LoadIcon(name, LogicalToDeviceUnits(18));
            if (icon == null) continue;
            Image? previousImage = button.Image;
            button.Image = icon;
            button.ImageAlign = ContentAlignment.MiddleLeft;
            button.TextImageRelation = TextImageRelation.ImageBeforeText;
            previousImage?.Dispose();
        }
    }

    /// <inheritdoc/>
    protected override void OnDpiChanged(DpiChangedEventArgs e)
    {
        base.OnDpiChanged(e);
        ApplyTransportIcons();
    }

    /// <inheritdoc/>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _elapsedTimer.Stop();
        _elapsedTimer.Dispose();
        base.OnFormClosing(e);
    }
}
