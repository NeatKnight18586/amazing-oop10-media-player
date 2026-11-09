using System;
using System.Drawing;
using System.Windows.Forms;

namespace mediaPlayerPrj;

/// <summary>
/// Single place for the player's colors, fonts and spacing so the UI can be
/// restyled without touching layout code. Contains no playback or file logic.
/// </summary>
public static class Theme
{
    /// <summary>Window background behind all panels.</summary>
    public static readonly Color Background = Color.FromArgb(242, 243, 247);

    /// <summary>Background of the playlist and other content surfaces.</summary>
    public static readonly Color Surface = Color.White;

    /// <summary>Primary text color (contrast ratio above 12:1 on Background).</summary>
    public static readonly Color Text = Color.FromArgb(25, 28, 34);

    /// <summary>Secondary text such as the status line.</summary>
    public static readonly Color MutedText = Color.FromArgb(78, 84, 96);

    /// <summary>Accent used for primary buttons.</summary>
    public static readonly Color Accent = Color.FromArgb(31, 111, 98);

    /// <summary>Text drawn on top of <see cref="Accent"/>.</summary>
    public static readonly Color AccentText = Color.White;

    /// <summary>Backdrop of the video host; the video itself is never covered.</summary>
    public static readonly Color VideoBackdrop = Color.Black;

    /// <summary>Base UI font used for all controls.</summary>
    public static readonly Font Body = new("Segoe UI", 9.5f, FontStyle.Regular);

    /// <summary>Emphasized font for the Now Playing label.</summary>
    public static readonly Font Emphasis = new("Segoe UI Semibold", 10f, FontStyle.Regular);

    /// <summary>Outer padding in pixels around the main layout.</summary>
    public const int Gutter = 8;

    /// <summary>Standard height in pixels for buttons in the control strip.</summary>
    public const int ControlHeight = 32;

    /// <summary>
    /// Applies theme colors and fonts to <paramref name="root"/> and its children.
    /// Children of <paramref name="skip"/> (the VLC video host) are left untouched.
    /// </summary>
    /// <param name="root">Form or container to style.</param>
    /// <param name="skip">Control whose subtree must not be restyled, or null.</param>
    public static void Apply(Control root, Control? skip = null)
    {
        ArgumentNullException.ThrowIfNull(root);
        root.BackColor = Background;
        root.ForeColor = Text;
        root.Font = Body;
        foreach (Control child in root.Controls) ApplyToChild(child, skip);
    }

    private static void ApplyToChild(Control control, Control? skip)
    {
        if (ReferenceEquals(control, skip))
        {
            control.BackColor = VideoBackdrop;
            return;
        }

        switch (control)
        {
            case Button button:
                button.FlatStyle = FlatStyle.System;
                button.Height = ControlHeight;
                break;
            case ListView list:
                list.BackColor = Surface;
                list.ForeColor = Text;
                break;
            case MenuStrip:
                break;
            case Label label:
                label.ForeColor = Text;
                break;
        }

        foreach (Control child in control.Controls) ApplyToChild(child, skip);
    }
}
