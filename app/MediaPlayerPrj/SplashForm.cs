using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace mediaPlayerPrj;

/// <summary>Three-second skippable startup screen with group information and portraits.</summary>
public partial class SplashForm : Form
{
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 3000 };
    private PictureBox[] memberPhotos = null!;
    private Label[] memberCaptions = null!;

    /// <summary>Builds the splash screen and loads optional original group assets.</summary>
    public SplashForm()
    {
        InitializeComponent();
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        // Match the aged-paper texture used on the main player.
        Image? splashPaper = Theme.LoadImage("paper_texture.png");
        if (splashPaper != null)
        {
            root.BackgroundImage = splashPaper;
            root.BackgroundImageLayout = ImageLayout.Tile;
        }
        memberPhotos = new [] { picMember1, picMember2, picMember3, picMember4, picMember5 };
        memberCaptions = new [] { lblMember1, lblMember2, lblMember3, lblMember4, lblMember5 };
        picLogo.Image = Theme.LoadImage("logo.png");
        for (int index = 0; index < TeamInfo.Members.Count; index++)
        {
            TeamMember member = TeamInfo.Members[index];
            Image? portrait = Theme.LoadImage(member.PhotoFile);
            memberPhotos[index].Image = portrait;
            memberCaptions[index].Text = member.Name + Environment.NewLine +
                member.Role + (portrait == null ? Environment.NewLine + "Photo unavailable" : "");
            memberPhotos[index].Click += (_, _) => Close();
            memberCaptions[index].Click += (_, _) => Close();
        }
        _timer.Tick += (_, _) => Close();
        Shown += (_, _) => _timer.Start();
        KeyDown += (_, _) => Close();
        Click += (_, _) => Close();
        lblLoading.Click += (_, _) => Close();
        lblAppName.Click += (_, _) => Close();
        lblGroup.Click += (_, _) => Close();
    }

    /// <inheritdoc/>
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _timer.Stop();
        _timer.Dispose();
        picLogo.Image?.Dispose();
        for (int index = 0; index < memberPhotos.Length; index++)
            memberPhotos[index].Image?.Dispose();
        base.OnFormClosed(e);
    }
}
