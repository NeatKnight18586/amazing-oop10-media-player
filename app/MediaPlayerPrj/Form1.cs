using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mediaPlayerPrj;

/// <summary>
/// Temporary code-built WinForms UI because the instructor's Form1.Designer.cs
/// was not provided. Shanen will replace the visuals with the original skin.
/// </summary>
public class Form1 : BasePlayerForm
{
    private readonly ListView _playlistView = new();
    private readonly Label _nowPlaying = new();
    private readonly Label _elapsed = new();
    private readonly Label _status = new();
    private readonly Panel _videoHost = new();
    private readonly SplitContainer _split = new();
    private readonly System.Windows.Forms.Timer _clock = new() { Interval = 500 };
    private readonly HashSet<string> _extensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".mp3", ".mp4"
    };
    private bool _importBusy;

    /// <summary>Builds the temporary playlist/controls surface with no Designer dependency.</summary>
    public Form1()
    {
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Text = "Amazing Player — Bootstrap (not final)";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1100, 700);
        MinimumSize = new Size(820, 540);

        BuildInterface();
        AttachVideo(_videoHost);
        ApplyTheme();
        _clock.Tick += (_, _) => UpdateElapsed();
        _clock.Start();
    }

    private void BuildInterface()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(Theme.Gutter)
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Controls.Add(layout);

        var menu = BuildMenu();
        layout.Controls.Add(menu, 0, 0);
        MainMenuStrip = menu;

        _split.Dock = DockStyle.Fill;
        _split.Orientation = Orientation.Vertical;
        var split = _split;
        layout.Controls.Add(split, 0, 1);

        _playlistView.Dock = DockStyle.Fill;
        _playlistView.View = View.Details;
        _playlistView.FullRowSelect = true;
        _playlistView.MultiSelect = false;
        _playlistView.GridLines = false;
        _playlistView.HideSelection = false;
        _playlistView.ShowItemToolTips = true;
        _playlistView.Columns.Add("Title", 170);
        _playlistView.Columns.Add("Artist", 120);
        _playlistView.Columns.Add("Album / Resolution", 145);
        _playlistView.Columns.Add("Length", 70, HorizontalAlignment.Right);
        _playlistView.Columns.Add("Type", 55);
        _playlistView.Columns.Add("Path", 250);
        _playlistView.DoubleClick += (_, _) => PlaySelected();
        _playlistView.SizeChanged += (_, _) => FitPlaylistColumns();
        split.Panel1.Controls.Add(_playlistView);

        _videoHost.Dock = DockStyle.Fill;
        _videoHost.BackColor = Theme.VideoBackdrop;
        split.Panel2.Controls.Add(_videoHost);

        var controls = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = true
        };
        layout.Controls.Add(controls, 0, 2);

        controls.Controls.Add(MakeButton("Add MP3/MP4", async () => await AddFromDialogAsync()));
        controls.Controls.Add(MakeButton("Play", PlaySelected));
        controls.Controls.Add(MakeButton("Pause", Pause));
        controls.Controls.Add(MakeButton("Stop", Stop));
        controls.Controls.Add(MakeButton("Prev", Previous));
        controls.Controls.Add(MakeButton("Next", Next));

        var volume = new TrackBar
        {
            Minimum = 0, Maximum = 100, Value = 70,
            TickFrequency = 20, Width = 125, AutoSize = false, Height = 36,
            Margin = new Padding(Theme.Gutter, 0, Theme.Gutter, 0)
        };
        volume.Scroll += (_, _) => SetVolume(volume.Value);
        controls.Controls.Add(volume);
        SetVolume(volume.Value);

        _nowPlaying.AutoSize = false;
        _nowPlaying.Width = 250;
        _nowPlaying.Height = Theme.ControlHeight;
        _nowPlaying.TextAlign = ContentAlignment.MiddleLeft;
        _nowPlaying.AutoEllipsis = true;
        _nowPlaying.Text = "Now playing: —";
        controls.Controls.Add(_nowPlaying);

        _elapsed.AutoSize = false;
        _elapsed.Width = 150;
        _elapsed.Height = Theme.ControlHeight;
        _elapsed.TextAlign = ContentAlignment.MiddleLeft;
        _elapsed.Text = "00:00:00 / 00:00:00";
        controls.Controls.Add(_elapsed);

        _status.Dock = DockStyle.Fill;
        _status.AutoEllipsis = true;
        _status.TextAlign = ContentAlignment.MiddleLeft;
        _status.Padding = new Padding(0, 4, 0, 4);
        _status.Text = "Bootstrap UI: library persistence, modes, favorites and skin pending.";
        layout.Controls.Add(_status, 0, 3);
    }

    private MenuStrip BuildMenu()
    {
        var bar = new MenuStrip { Dock = DockStyle.Fill };
        var file = new ToolStripMenuItem("&File");
        file.DropDownItems.Add(MenuAction("&Add Files...", Keys.Control | Keys.O,
            async () => await AddFromDialogAsync()));
        file.DropDownItems.Add(MenuAction("E&xit", Keys.None, () => Close()));

        var playback = new ToolStripMenuItem("&Playback");
        playback.DropDownItems.Add(MenuAction("&Play", Keys.F5, PlaySelected));
        playback.DropDownItems.Add(MenuAction("P&ause", Keys.F6, Pause));
        playback.DropDownItems.Add(MenuAction("&Stop", Keys.F7, Stop));
        playback.DropDownItems.Add(MenuAction("&Next", Keys.Control | Keys.Right, Next));
        playback.DropDownItems.Add(MenuAction("Pre&vious", Keys.Control | Keys.Left, Previous));

        var library = new ToolStripMenuItem("&Library");
        library.DropDownItems.Add(new ToolStripMenuItem("Sorting arrives in P2") { Enabled = false });

        var help = new ToolStripMenuItem("&Help");
        help.DropDownItems.Add(MenuAction("&About", Keys.F1, () =>
            MessageBox.Show("Amazing Player — BSCS2A / Group 1 Amazing\nBootstrap build", "About")));

        bar.Items.AddRange(new ToolStripItem[] { file, playback, library, help });
        return bar;
    }

    private static ToolStripMenuItem MenuAction(string title, Keys shortcut, Action action)
    {
        var item = new ToolStripMenuItem(title) { ShortcutKeys = shortcut };
        item.Click += (_, _) => action();
        return item;
    }

    private static Button MakeButton(string title, Action action)
    {
        var button = new Button
        {
            Text = title,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MinimumSize = new Size(0, Theme.ControlHeight)
        };
        button.Click += (_, _) => action();
        return button;
    }

    private async Task AddFromDialogAsync()
    {
        if (_importBusy) return;

        using var dialog = new OpenFileDialog
        {
            Filter = "Media files (*.mp3;*.mp4)|*.mp3;*.mp4|All files (*.*)|*.*",
            Multiselect = true
        };
        if (dialog.ShowDialog() != DialogResult.OK) return;

        _importBusy = true;
        try
        {
            var paths = new HashSet<string>(Playlist.Select(x => x.FilePath),
                StringComparer.OrdinalIgnoreCase);
            var additions = new List<MediaItem>();
            int failures = 0;
            foreach (string path in dialog.FileNames)
            {
                if (!_extensions.Contains(Path.GetExtension(path)) || !paths.Add(path))
                    continue;

                try
                {
                    additions.Add(await ReadMediaItemAsync(path));
                }
                catch (Exception ex)
                {
                    failures++;
                    SetStatus("Skipped unreadable file: " + Path.GetFileName(path) + " — " + ex.Message);
                }
            }

            Playlist.AddRange(additions);
            if (CurrentIndex < 0 && Playlist.Count > 0) CurrentIndex = 0;
            RebuildList();
            SetStatus($"Added {additions.Count} file(s); {failures} unreadable. Saving is P2.");
        }
        finally
        {
            _importBusy = false;
        }
    }

    private void RebuildList()
    {
        _playlistView.BeginUpdate();
        try
        {
            _playlistView.Items.Clear();
            for (int i = 0; i < Playlist.Count; i++)
            {
                var item = Playlist[i];
                var row = new ListViewItem(item.Title) { Tag = item };
                row.SubItems.Add(item.Artist);
                row.SubItems.Add(item.Extra);
                row.SubItems.Add(item.DurationText);
                row.SubItems.Add(item.MediaType);
                row.SubItems.Add(item.FilePath);
                _playlistView.Items.Add(row);
            }
            SelectCurrentRow();
        }
        finally
        {
            _playlistView.EndUpdate();
        }
    }

    /// <summary>
    /// Keeps Length/Type/Path at fixed widths and shares the rest of the row
    /// between Title, Artist and Album/Resolution so long titles stay readable.
    /// </summary>
    private void FitPlaylistColumns()
    {
        if (_playlistView.Columns.Count != 6) return;

        int length = LogicalToDeviceUnits(70);
        int type = LogicalToDeviceUnits(55);
        int path = LogicalToDeviceUnits(250);
        int scrollbar = SystemInformation.VerticalScrollBarWidth;
        int titleMin = LogicalToDeviceUnits(130);
        int artistMin = LogicalToDeviceUnits(80);
        int albumMin = LogicalToDeviceUnits(80);

        // Title/Artist/Album/Length always get the space first; Type and Path
        // are secondary and are reached by horizontal scrolling in narrow panes.
        int wide = _playlistView.ClientSize.Width - length - type - path - scrollbar;
        int narrow = _playlistView.ClientSize.Width - length - scrollbar;
        int flexible = Math.Max(Math.Max(wide, narrow), titleMin + artistMin + albumMin);

        _playlistView.Columns[0].Width = Math.Max(titleMin, flexible * 45 / 100);
        _playlistView.Columns[1].Width = Math.Max(artistMin, flexible * 30 / 100);
        _playlistView.Columns[2].Width = Math.Max(albumMin, flexible * 25 / 100);
        _playlistView.Columns[3].Width = length;
        _playlistView.Columns[4].Width = type;
        _playlistView.Columns[5].Width = path;
    }

    private void SelectCurrentRow()
    {
        if (CurrentIndex >= 0 && CurrentIndex < _playlistView.Items.Count)
        {
            _playlistView.SelectedItems.Clear();
            _playlistView.Items[CurrentIndex].Selected = true;
        }
    }

    private void PlaySelected()
    {
        if (_playlistView.SelectedIndices.Count > 0)
            CurrentIndex = _playlistView.SelectedIndices[0];
        Play();
    }

    private void UpdateElapsed()
    {
        if (CurrentIndex < 0 || CurrentIndex >= Playlist.Count)
        {
            _elapsed.Text = "00:00:00 / 00:00:00";
            return;
        }

        _elapsed.Text = Format(ElapsedTime) + " / " + Playlist[CurrentIndex].DurationText;
    }

    private static string Format(TimeSpan time) =>
        string.Format("{0:00}:{1:00}:{2:00}", (int)time.TotalHours, time.Minutes, time.Seconds);

    /// <inheritdoc/>
    protected override void OnNowPlayingChanged(MediaItem item)
    {
        _nowPlaying.Text = "Now playing: " + item.Title + " — " + item.Artist;
        SelectCurrentRow();
        UpdateElapsed();
    }

    /// <inheritdoc/>
    protected override void SetStatus(string text) => _status.Text = text;

    /// <inheritdoc/>
    protected override void ApplyTheme()
    {
        Theme.Apply(this, _videoHost);
        _nowPlaying.Font = Theme.Emphasis;
        _status.ForeColor = Theme.MutedText;
    }

    /// <summary>
    /// Gives the playlist half the width once the real window size is known,
    /// and keeps both panes from being dragged below a usable size so the video
    /// host can never be squeezed away.
    /// </summary>
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        int playlistMin = LogicalToDeviceUnits(260);
        int videoMin = LogicalToDeviceUnits(260);
        int available = _split.Width - _split.SplitterWidth;
        if (available >= playlistMin + videoMin)
        {
            _split.Panel1MinSize = playlistMin;
            _split.Panel2MinSize = videoMin;
        }
        _split.SplitterDistance = Math.Clamp((int)(_split.Width * 0.5),
            _split.Panel1MinSize, Math.Max(_split.Panel1MinSize, available - _split.Panel2MinSize));
        FitPlaylistColumns();
    }

    /// <inheritdoc/>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _clock.Stop();
        base.OnFormClosing(e);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing) _clock.Dispose();
        base.Dispose(disposing);
    }
}
