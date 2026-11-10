#nullable enable
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace mediaPlayerPrj;

public partial class Form1
{
    private IContainer? components;
    private TableLayoutPanel layoutRoot = null!;
    private SplitContainer splitMain = null!;
    private Panel pnlCassette = null!;
    private Panel pnlVideo = null!;
    private Panel pnlDetails = null!;
    private FlowLayoutPanel flowLibrary = null!;
    private FlowLayoutPanel flowTransport = null!;
    private FlowLayoutPanel flowSeek = null!;
    private ListView lstPlaylist = null!;
    private Label lblCassetteTitle = null!;
    private Label lblNowPlaying = null!;
    private Label lblElapsed = null!;
    private Label lblStatus = null!;
    private TextBox txtSearch = null!;
    private CheckBox chkFavoritesOnly = null!;
    private TrackBar trkSeek = null!;
    private TrackBar trkVolume = null!;
    private Label lblSeek = null!;
    private Label lblVolume = null!;
    private Button btnAdd = null!;
    private Button btnPlay = null!;
    private Button btnPause = null!;
    private Button btnStop = null!;
    private Button btnPrevious = null!;
    private Button btnNext = null!;
    private Button btnFavorite = null!;
    private Button btnShuffle = null!;
    private Button btnRepeat = null!;
    private Button btnSortTitle = null!;
    private Button btnSortArtist = null!;
    private MenuStrip menuMain = null!;
    private ToolStripMenuItem menuFile = null!;
    private ToolStripMenuItem menuPlayback = null!;
    private ToolStripMenuItem menuLibrary = null!;
    private ToolStripMenuItem menuHelp = null!;
    private ToolStripMenuItem miAdd = null!;
    private ToolStripMenuItem miSave = null!;
    private ToolStripMenuItem miReload = null!;
    private ToolStripMenuItem miPlay = null!;
    private ToolStripMenuItem miPause = null!;
    private ToolStripMenuItem miStop = null!;
    private ToolStripMenuItem miNext = null!;
    private ToolStripMenuItem miPrevious = null!;
    private ToolStripMenuItem miShuffle = null!;
    private ToolStripMenuItem miRepeat = null!;
    private ToolStripMenuItem miSortTitle = null!;
    private ToolStripMenuItem miSortArtist = null!;
    private ToolStripMenuItem miFavorites = null!;
    private ToolStripMenuItem miAbout = null!;

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();
        layoutRoot = new TableLayoutPanel();
        splitMain = new SplitContainer();
        pnlCassette = new Panel();
        pnlVideo = new Panel();
        pnlDetails = new Panel();
        flowLibrary = new FlowLayoutPanel();
        flowTransport = new FlowLayoutPanel();
        flowSeek = new FlowLayoutPanel();
        lstPlaylist = new ListView();
        lblCassetteTitle = new Label();
        lblNowPlaying = new Label();
        lblElapsed = new Label();
        lblStatus = new Label();
        txtSearch = new TextBox();
        chkFavoritesOnly = new CheckBox();
        trkSeek = new TrackBar();
        trkVolume = new TrackBar();
        btnAdd = new Button();
        btnPlay = new Button();
        btnPause = new Button();
        btnStop = new Button();
        btnPrevious = new Button();
        btnNext = new Button();
        btnFavorite = new Button();
        btnShuffle = new Button();
        btnRepeat = new Button();
        btnSortTitle = new Button();
        btnSortArtist = new Button();
        menuMain = new MenuStrip();
        menuFile = new ToolStripMenuItem();
        menuPlayback = new ToolStripMenuItem();
        menuLibrary = new ToolStripMenuItem();
        menuHelp = new ToolStripMenuItem();
        miAdd = new ToolStripMenuItem();
        miSave = new ToolStripMenuItem();
        miReload = new ToolStripMenuItem();
        miPlay = new ToolStripMenuItem();
        miPause = new ToolStripMenuItem();
        miStop = new ToolStripMenuItem();
        miNext = new ToolStripMenuItem();
        miPrevious = new ToolStripMenuItem();
        miShuffle = new ToolStripMenuItem();
        miRepeat = new ToolStripMenuItem();
        miSortTitle = new ToolStripMenuItem();
        miSortArtist = new ToolStripMenuItem();
        miFavorites = new ToolStripMenuItem();
        miAbout = new ToolStripMenuItem();

        SuspendLayout();
        layoutRoot.SuspendLayout();
        ((ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((ISupportInitialize)trkSeek).BeginInit();
        ((ISupportInitialize)trkVolume).BeginInit();

        // Window and top-level layout are editable through the Visual Studio Designer.
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1180, 760);
        MinimumSize = new Size(960, 640);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Amazing Player";
        Font = new Font("Segoe UI", 10F);
        MainMenuStrip = menuMain;

        layoutRoot.Name = "layoutRoot";
        layoutRoot.Dock = DockStyle.Fill;
        layoutRoot.Padding = new Padding(14, 8, 14, 8);
        layoutRoot.ColumnCount = 1;
        layoutRoot.RowCount = 5;
        layoutRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        layoutRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
        layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
        layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

        // Designer-owned MenuStrip; no command logic is inside the designer file.
        menuMain.Name = "menuMain";
        menuMain.Dock = DockStyle.Fill;
        menuMain.BackColor = Color.FromArgb(245, 209, 154);
        menuFile.Name = "menuFile"; menuFile.Text = "&File";
        menuPlayback.Name = "menuPlayback"; menuPlayback.Text = "&Playback";
        menuLibrary.Name = "menuLibrary"; menuLibrary.Text = "&Library";
        menuHelp.Name = "menuHelp"; menuHelp.Text = "&Help";
        miAdd.Name = "miAdd"; miAdd.Text = "&Add Media..."; miAdd.ShortcutKeys = Keys.Control | Keys.O;
        miSave.Name = "miSave"; miSave.Text = "&Save Library"; miSave.ShortcutKeys = Keys.Control | Keys.S;
        miReload.Name = "miReload"; miReload.Text = "&Reload Library"; miReload.ShortcutKeys = Keys.Control | Keys.L;
        miPlay.Name = "miPlay"; miPlay.Text = "&Play"; miPlay.ShortcutKeys = Keys.F5;
        miPause.Name = "miPause"; miPause.Text = "P&ause"; miPause.ShortcutKeys = Keys.F6;
        miStop.Name = "miStop"; miStop.Text = "&Stop"; miStop.ShortcutKeys = Keys.F7;
        miNext.Name = "miNext"; miNext.Text = "&Next"; miNext.ShortcutKeys = Keys.Control | Keys.Right;
        miPrevious.Name = "miPrevious"; miPrevious.Text = "Pre&vious"; miPrevious.ShortcutKeys = Keys.Control | Keys.Left;
        miShuffle.Name = "miShuffle"; miShuffle.Text = "Shuffle: Off"; miShuffle.ShortcutKeys = Keys.Control | Keys.H;
        miRepeat.Name = "miRepeat"; miRepeat.Text = "Repeat: Off"; miRepeat.ShortcutKeys = Keys.Control | Keys.R;
        miSortTitle.Name = "miSortTitle"; miSortTitle.Text = "Sort by &Title"; miSortTitle.ShortcutKeys = Keys.Control | Keys.D1;
        miSortArtist.Name = "miSortArtist"; miSortArtist.Text = "Sort by &Artist"; miSortArtist.ShortcutKeys = Keys.Control | Keys.D2;
        miFavorites.Name = "miFavorites"; miFavorites.Text = "Show &Favorites"; miFavorites.ShortcutKeys = Keys.Control | Keys.F;
        miAbout.Name = "miAbout"; miAbout.Text = "&About"; miAbout.ShortcutKeys = Keys.F1;
        menuFile.DropDownItems.AddRange(new ToolStripItem[] { miAdd, miSave, miReload });
        menuPlayback.DropDownItems.AddRange(new ToolStripItem[] {
            miPlay, miPause, miStop, miPrevious, miNext, miShuffle, miRepeat
        });
        menuLibrary.DropDownItems.AddRange(new ToolStripItem[] { miSortTitle, miSortArtist, miFavorites });
        menuHelp.DropDownItems.Add(miAbout);
        menuMain.Items.AddRange(new ToolStripItem[] { menuFile, menuPlayback, menuLibrary, menuHelp });

        // Main: large cassette/video stage on left, playlist on right.
        splitMain.Name = "splitMain";
        splitMain.Dock = DockStyle.Fill;
        splitMain.BackColor = Color.FromArgb(124, 75, 38);
        splitMain.Orientation = Orientation.Vertical;
        splitMain.SplitterWidth = 7;
        splitMain.SplitterDistance = 728;
        splitMain.Panel1.BackColor = Color.FromArgb(238, 200, 147);
        splitMain.Panel2.BackColor = Color.FromArgb(238, 200, 147);

        pnlCassette.Name = "pnlCassette";
        pnlCassette.Dock = DockStyle.Fill;
        pnlCassette.BackColor = Color.FromArgb(69, 39, 27);
        pnlCassette.BackgroundImageLayout = ImageLayout.Stretch;
        lblCassetteTitle.Name = "lblCassetteTitle";
        lblCassetteTitle.Text = "Select a song";
        lblCassetteTitle.TextAlign = ContentAlignment.MiddleCenter;
        lblCassetteTitle.Location = new Point(192, 48);
        lblCassetteTitle.Size = new Size(360, 46);
        lblCassetteTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblCassetteTitle.Font = new Font("Segoe Print", 19F, FontStyle.Regular);
        lblCassetteTitle.BackColor = Color.FromArgb(239, 190, 120);
        lblCassetteTitle.ForeColor = Color.FromArgb(39, 48, 35);
        pnlVideo.Name = "pnlVideo";
        pnlVideo.Dock = DockStyle.Fill;
        pnlVideo.BackColor = Color.Black;
        pnlVideo.Visible = false;
        pnlCassette.Controls.Add(lblCassetteTitle);
        pnlCassette.Controls.Add(pnlVideo);

        pnlDetails.Name = "pnlDetails";
        pnlDetails.Dock = DockStyle.Bottom;
        pnlDetails.Height = 60;
        pnlDetails.BackColor = Color.FromArgb(243, 214, 170);
        lblNowPlaying.Name = "lblNowPlaying";
        lblNowPlaying.Text = "Now Playing: —";
        lblNowPlaying.Location = new Point(12, 13);
        lblNowPlaying.Size = new Size(490, 35);
        lblNowPlaying.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
        lblNowPlaying.AutoEllipsis = true;
        lblNowPlaying.ForeColor = Color.FromArgb(56, 35, 25);
        lblElapsed.Name = "lblElapsed";
        lblElapsed.Text = "00:00:00 / 00:00:00";
        lblElapsed.TextAlign = ContentAlignment.MiddleRight;
        lblElapsed.Location = new Point(508, 13);
        lblElapsed.Size = new Size(190, 35);
        lblElapsed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        pnlDetails.Controls.Add(lblNowPlaying);
        pnlDetails.Controls.Add(lblElapsed);
        splitMain.Panel1.Controls.Add(pnlCassette);
        splitMain.Panel1.Controls.Add(pnlDetails);

        flowLibrary.Name = "flowLibrary";
        flowLibrary.Dock = DockStyle.Top;
        flowLibrary.Height = 86;
        flowLibrary.AutoScroll = true;
        flowLibrary.WrapContents = true;
        flowLibrary.Padding = new Padding(5, 5, 5, 2);
        flowLibrary.BackColor = Color.FromArgb(241, 207, 159);
        txtSearch.Name = "txtSearch";
        txtSearch.Width = 160;
        txtSearch.PlaceholderText = "Search playlist...";
        btnSortTitle.Name = "btnSortTitle"; btnSortTitle.Text = "Title A–Z"; btnSortTitle.Size = new Size(102, 32);
        btnSortArtist.Name = "btnSortArtist"; btnSortArtist.Text = "Artist A–Z"; btnSortArtist.Size = new Size(102, 32);
        chkFavoritesOnly.Name = "chkFavoritesOnly"; chkFavoritesOnly.Text = "Favorites only";
        chkFavoritesOnly.AutoSize = true;
        flowLibrary.Controls.Add(txtSearch);
        flowLibrary.Controls.Add(btnSortTitle);
        flowLibrary.Controls.Add(btnSortArtist);
        flowLibrary.Controls.Add(chkFavoritesOnly);

        lstPlaylist.Name = "lstPlaylist";
        lstPlaylist.Dock = DockStyle.Fill;
        lstPlaylist.View = View.Details;
        lstPlaylist.FullRowSelect = true;
        lstPlaylist.GridLines = true;
        lstPlaylist.MultiSelect = false;
        lstPlaylist.HideSelection = false;
        lstPlaylist.BackColor = Color.FromArgb(252, 232, 195);
        lstPlaylist.Columns.Add("Title", 148);
        lstPlaylist.Columns.Add("Artist", 110);
        lstPlaylist.Columns.Add("Album / Resolution", 150);
        lstPlaylist.Columns.Add("Length", 78);
        lstPlaylist.Columns.Add("Type", 60);
        lstPlaylist.Columns.Add("File Path", 275);
        splitMain.Panel2.Controls.Add(lstPlaylist);
        splitMain.Panel2.Controls.Add(flowLibrary);

        // Playback buttons. Each control can be selected/moved in the Designer.
        flowTransport.Name = "flowTransport";
        flowTransport.Dock = DockStyle.Fill;
        flowTransport.AutoScroll = true;
        flowTransport.WrapContents = false;
        flowTransport.Padding = new Padding(6, 9, 0, 3);
        flowTransport.BackColor = Color.FromArgb(203, 142, 76);
        btnPrevious.Name = "btnPrevious"; btnPrevious.Text = "◀◀  Prev"; btnPrevious.Size = new Size(106, 54);
        btnPlay.Name = "btnPlay"; btnPlay.Text = "▶  Play"; btnPlay.Size = new Size(106, 54);
        btnPause.Name = "btnPause"; btnPause.Text = "Ⅱ  Pause"; btnPause.Size = new Size(106, 54);
        btnStop.Name = "btnStop"; btnStop.Text = "■  Stop"; btnStop.Size = new Size(106, 54);
        btnNext.Name = "btnNext"; btnNext.Text = "Next  ▶▶"; btnNext.Size = new Size(106, 54);
        btnAdd.Name = "btnAdd"; btnAdd.Text = "+  Add"; btnAdd.Size = new Size(106, 54);
        btnFavorite.Name = "btnFavorite"; btnFavorite.Text = "♡  Like"; btnFavorite.Size = new Size(106, 54);
        btnShuffle.Name = "btnShuffle"; btnShuffle.Text = "Shuffle: Off"; btnShuffle.Size = new Size(116, 54);
        btnRepeat.Name = "btnRepeat"; btnRepeat.Text = "Repeat: Off"; btnRepeat.Size = new Size(116, 54);
        flowTransport.Controls.Add(btnPrevious);
        flowTransport.Controls.Add(btnPlay);
        flowTransport.Controls.Add(btnPause);
        flowTransport.Controls.Add(btnStop);
        flowTransport.Controls.Add(btnNext);
        flowTransport.Controls.Add(btnAdd);
        flowTransport.Controls.Add(btnFavorite);
        flowTransport.Controls.Add(btnShuffle);
        flowTransport.Controls.Add(btnRepeat);

        flowSeek.Name = "flowSeek";
        flowSeek.Dock = DockStyle.Fill;
        flowSeek.BackColor = Color.FromArgb(232, 188, 125);
        flowSeek.WrapContents = false;
        flowSeek.AutoScroll = true;
        flowSeek.Padding = new Padding(10, 9, 0, 0);
        lblSeek = new Label();
        lblSeek.Name = "lblSeek"; lblSeek.Text = "Seek"; lblSeek.Width = 38; lblSeek.Height = 40;
        lblSeek.TextAlign = ContentAlignment.MiddleLeft;
        trkSeek.Name = "trkSeek"; trkSeek.Width = 650; trkSeek.Height = 45;
        trkSeek.Maximum = 1000; trkSeek.TickStyle = TickStyle.None; trkSeek.Enabled = false;
        lblVolume = new Label();
        lblVolume.Name = "lblVolume"; lblVolume.Text = "Volume"; lblVolume.Width = 66; lblVolume.Height = 40;
        lblVolume.TextAlign = ContentAlignment.MiddleLeft;
        trkVolume.Name = "trkVolume"; trkVolume.Width = 205; trkVolume.Height = 45;
        trkVolume.Minimum = 0; trkVolume.Maximum = 100; trkVolume.Value = 70;
        trkVolume.TickStyle = TickStyle.None;
        flowSeek.Controls.Add(lblSeek);
        flowSeek.Controls.Add(trkSeek);
        flowSeek.Controls.Add(lblVolume);
        flowSeek.Controls.Add(trkVolume);

        lblStatus.Name = "lblStatus";
        lblStatus.Text = "Status: Ready";
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.TextAlign = ContentAlignment.MiddleLeft;
        lblStatus.BackColor = Color.FromArgb(240, 208, 165);
        lblStatus.ForeColor = Color.FromArgb(65, 42, 29);

        layoutRoot.Controls.Add(menuMain, 0, 0);
        layoutRoot.Controls.Add(splitMain, 0, 1);
        layoutRoot.Controls.Add(flowTransport, 0, 2);
        layoutRoot.Controls.Add(flowSeek, 0, 3);
        layoutRoot.Controls.Add(lblStatus, 0, 4);
        Controls.Add(layoutRoot);

        ((ISupportInitialize)trkVolume).EndInit();
        ((ISupportInitialize)trkSeek).EndInit();
        splitMain.Panel2.ResumeLayout(false);
        splitMain.Panel1.ResumeLayout(false);
        ((ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        layoutRoot.ResumeLayout(false);
        layoutRoot.PerformLayout();
        ResumeLayout(false);
    }
}
