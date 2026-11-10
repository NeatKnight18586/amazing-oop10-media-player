#nullable enable
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
    private TableLayoutPanel memberPanel1 = null!;
    private PictureBox picMember1 = null!;
    private Label lblMember1 = null!;
    private TableLayoutPanel memberPanel2 = null!;
    private PictureBox picMember2 = null!;
    private Label lblMember2 = null!;
    private TableLayoutPanel memberPanel3 = null!;
    private PictureBox picMember3 = null!;
    private Label lblMember3 = null!;
    private TableLayoutPanel memberPanel4 = null!;
    private PictureBox picMember4 = null!;
    private Label lblMember4 = null!;
    private TableLayoutPanel memberPanel5 = null!;
    private PictureBox picMember5 = null!;
    private Label lblMember5 = null!;


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
        memberPanel1 = new TableLayoutPanel();
        picMember1 = new PictureBox();
        lblMember1 = new Label();
        memberPanel2 = new TableLayoutPanel();
        picMember2 = new PictureBox();
        lblMember2 = new Label();
        memberPanel3 = new TableLayoutPanel();
        picMember3 = new PictureBox();
        lblMember3 = new Label();
        memberPanel4 = new TableLayoutPanel();
        picMember4 = new PictureBox();
        lblMember4 = new Label();
        memberPanel5 = new TableLayoutPanel();
        picMember5 = new PictureBox();
        lblMember5 = new Label();


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
        teamGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        memberPanel1.Name = "memberPanel1";
        memberPanel1.Dock = DockStyle.Fill;
        memberPanel1.Margin = new Padding(5);
        memberPanel1.RowCount = 2;
        memberPanel1.ColumnCount = 1;
        memberPanel1.BackColor = Color.FromArgb(248, 217, 170);
        memberPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        memberPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        memberPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
        picMember1.Name = "picMember1";
        picMember1.Dock = DockStyle.Fill;
        picMember1.SizeMode = PictureBoxSizeMode.Zoom;
        picMember1.Margin = new Padding(5);
        picMember1.BackColor = Color.FromArgb(221, 180, 125);
        lblMember1.Name = "lblMember1";
        lblMember1.Dock = DockStyle.Fill;
        lblMember1.TextAlign = ContentAlignment.TopCenter;
        lblMember1.Font = new Font("Segoe UI", 9F);
        lblMember1.ForeColor = Color.FromArgb(55, 38, 29);
        memberPanel1.Controls.Add(picMember1, 0, 0);
        memberPanel1.Controls.Add(lblMember1, 0, 1);
        teamGrid.Controls.Add(memberPanel1, 0, 0);

        teamGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        memberPanel2.Name = "memberPanel2";
        memberPanel2.Dock = DockStyle.Fill;
        memberPanel2.Margin = new Padding(5);
        memberPanel2.RowCount = 2;
        memberPanel2.ColumnCount = 1;
        memberPanel2.BackColor = Color.FromArgb(248, 217, 170);
        memberPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        memberPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        memberPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
        picMember2.Name = "picMember2";
        picMember2.Dock = DockStyle.Fill;
        picMember2.SizeMode = PictureBoxSizeMode.Zoom;
        picMember2.Margin = new Padding(5);
        picMember2.BackColor = Color.FromArgb(221, 180, 125);
        lblMember2.Name = "lblMember2";
        lblMember2.Dock = DockStyle.Fill;
        lblMember2.TextAlign = ContentAlignment.TopCenter;
        lblMember2.Font = new Font("Segoe UI", 9F);
        lblMember2.ForeColor = Color.FromArgb(55, 38, 29);
        memberPanel2.Controls.Add(picMember2, 0, 0);
        memberPanel2.Controls.Add(lblMember2, 0, 1);
        teamGrid.Controls.Add(memberPanel2, 1, 0);

        teamGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        memberPanel3.Name = "memberPanel3";
        memberPanel3.Dock = DockStyle.Fill;
        memberPanel3.Margin = new Padding(5);
        memberPanel3.RowCount = 2;
        memberPanel3.ColumnCount = 1;
        memberPanel3.BackColor = Color.FromArgb(248, 217, 170);
        memberPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        memberPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        memberPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
        picMember3.Name = "picMember3";
        picMember3.Dock = DockStyle.Fill;
        picMember3.SizeMode = PictureBoxSizeMode.Zoom;
        picMember3.Margin = new Padding(5);
        picMember3.BackColor = Color.FromArgb(221, 180, 125);
        lblMember3.Name = "lblMember3";
        lblMember3.Dock = DockStyle.Fill;
        lblMember3.TextAlign = ContentAlignment.TopCenter;
        lblMember3.Font = new Font("Segoe UI", 9F);
        lblMember3.ForeColor = Color.FromArgb(55, 38, 29);
        memberPanel3.Controls.Add(picMember3, 0, 0);
        memberPanel3.Controls.Add(lblMember3, 0, 1);
        teamGrid.Controls.Add(memberPanel3, 2, 0);

        teamGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        memberPanel4.Name = "memberPanel4";
        memberPanel4.Dock = DockStyle.Fill;
        memberPanel4.Margin = new Padding(5);
        memberPanel4.RowCount = 2;
        memberPanel4.ColumnCount = 1;
        memberPanel4.BackColor = Color.FromArgb(248, 217, 170);
        memberPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        memberPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        memberPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
        picMember4.Name = "picMember4";
        picMember4.Dock = DockStyle.Fill;
        picMember4.SizeMode = PictureBoxSizeMode.Zoom;
        picMember4.Margin = new Padding(5);
        picMember4.BackColor = Color.FromArgb(221, 180, 125);
        lblMember4.Name = "lblMember4";
        lblMember4.Dock = DockStyle.Fill;
        lblMember4.TextAlign = ContentAlignment.TopCenter;
        lblMember4.Font = new Font("Segoe UI", 9F);
        lblMember4.ForeColor = Color.FromArgb(55, 38, 29);
        memberPanel4.Controls.Add(picMember4, 0, 0);
        memberPanel4.Controls.Add(lblMember4, 0, 1);
        teamGrid.Controls.Add(memberPanel4, 3, 0);

        teamGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        memberPanel5.Name = "memberPanel5";
        memberPanel5.Dock = DockStyle.Fill;
        memberPanel5.Margin = new Padding(5);
        memberPanel5.RowCount = 2;
        memberPanel5.ColumnCount = 1;
        memberPanel5.BackColor = Color.FromArgb(248, 217, 170);
        memberPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        memberPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        memberPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
        picMember5.Name = "picMember5";
        picMember5.Dock = DockStyle.Fill;
        picMember5.SizeMode = PictureBoxSizeMode.Zoom;
        picMember5.Margin = new Padding(5);
        picMember5.BackColor = Color.FromArgb(221, 180, 125);
        lblMember5.Name = "lblMember5";
        lblMember5.Dock = DockStyle.Fill;
        lblMember5.TextAlign = ContentAlignment.TopCenter;
        lblMember5.Font = new Font("Segoe UI", 9F);
        lblMember5.ForeColor = Color.FromArgb(55, 38, 29);
        memberPanel5.Controls.Add(picMember5, 0, 0);
        memberPanel5.Controls.Add(lblMember5, 0, 1);
        teamGrid.Controls.Add(memberPanel5, 4, 0);
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
