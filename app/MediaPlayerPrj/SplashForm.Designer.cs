using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace mediaPlayerPrj;

public partial class SplashForm
{
    private IContainer? components;
    private TableLayoutPanel root = null!;
    private TableLayoutPanel teamGrid = null!;
    private PictureBox picLogo = null!;
    private Label lblAppName = null!;
    private Label lblGroup = null!;
    private Label lblLoading = null!;
    private Label[] memberCaptions = null!;
    private PictureBox[] memberPhotos = null!;

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();
        root = new TableLayoutPanel();
        teamGrid = new TableLayoutPanel();
        picLogo = new PictureBox();
        lblAppName = new Label();
        lblGroup = new Label();
        lblLoading = new Label();
        memberCaptions = new Label[5];
        memberPhotos = new PictureBox[5];

        SuspendLayout();
        root.SuspendLayout();
        teamGrid.SuspendLayout();
        ((ISupportInitialize)picLogo).BeginInit();

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(980, 570);
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterScreen;
        KeyPreview = true;
        Text = "Amazing Player";
        BackColor = Color.FromArgb(236, 189, 116);
        Font = new Font("Segoe UI", 10F);

        root.Name = "root";
        root.Dock = DockStyle.Fill;
        root.Padding = new Padding(24, 16, 24, 16);
        root.ColumnCount = 1;
        root.RowCount = 5;
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 162F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));

        picLogo.Name = "picLogo";
        picLogo.Dock = DockStyle.Fill;
        picLogo.SizeMode = PictureBoxSizeMode.Zoom;
        picLogo.BackColor = Color.Transparent;

        lblAppName.Name = "lblAppName";
        lblAppName.Text = "Amazing Player";
        lblAppName.Dock = DockStyle.Fill;
        lblAppName.TextAlign = ContentAlignment.MiddleCenter;
        lblAppName.Font = new Font("Segoe Print", 32F, FontStyle.Bold);
        lblAppName.ForeColor = Color.FromArgb(43, 49, 34);

        lblGroup.Name = "lblGroup";
        lblGroup.Text = "BSCS2A  ·  Group 1  ·  Amazing";
        lblGroup.Dock = DockStyle.Fill;
        lblGroup.TextAlign = ContentAlignment.MiddleCenter;
        lblGroup.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblGroup.ForeColor = Color.FromArgb(87, 45, 30);

        teamGrid.Name = "teamGrid";
        teamGrid.Dock = DockStyle.Fill;
        teamGrid.ColumnCount = 5;
        teamGrid.RowCount = 1;
        teamGrid.Padding = new Padding(8, 4, 8, 4);
        for (int index = 0; index < 5; index++)
        {
            teamGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            var memberPanel = new TableLayoutPanel
            {
                Name = "memberPanel" + (index + 1),
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                RowCount = 2,
                ColumnCount = 1,
                BackColor = Color.FromArgb(248, 217, 170)
            };
            memberPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            memberPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            memberPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            memberPhotos[index] = new PictureBox
            {
                Name = "picMember" + (index + 1),
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Margin = new Padding(5),
                BackColor = Color.FromArgb(221, 180, 125)
            };
            memberCaptions[index] = new Label
            {
                Name = "lblMember" + (index + 1),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopCenter,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(55, 38, 29)
            };
            memberPanel.Controls.Add(memberPhotos[index], 0, 0);
            memberPanel.Controls.Add(memberCaptions[index], 0, 1);
            teamGrid.Controls.Add(memberPanel, index, 0);
        }

        lblLoading.Name = "lblLoading";
        lblLoading.Dock = DockStyle.Fill;
        lblLoading.TextAlign = ContentAlignment.MiddleCenter;
        lblLoading.Text = "Loading...  (click or press a key to continue)";
        lblLoading.Font = new Font("Segoe UI", 12F);
        lblLoading.ForeColor = Color.FromArgb(91, 48, 30);

        root.Controls.Add(picLogo, 0, 0);
        root.Controls.Add(lblAppName, 0, 1);
        root.Controls.Add(lblGroup, 0, 2);
        root.Controls.Add(teamGrid, 0, 3);
        root.Controls.Add(lblLoading, 0, 4);
        Controls.Add(root);

        ((ISupportInitialize)picLogo).EndInit();
        teamGrid.ResumeLayout(false);
        root.ResumeLayout(false);
        ResumeLayout(false);
    }
}
