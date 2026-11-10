using System;
using System.Drawing;
using System.Windows.Forms;

namespace mediaPlayerPrj;

/// <summary>Three-second skippable startup screen with group information and portraits.</summary>
public partial class SplashForm : Form
{
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 3000 };

    /// <summary>Builds the splash screen and loads optional original group assets.</summary>
    public SplashForm()
    {
        InitializeComponent();
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
