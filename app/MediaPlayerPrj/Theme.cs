using System;
using System.Drawing;
using System.IO;
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

    /// <summary>Border of buttons and other outlined controls.</summary>
    public static readonly Color Border = Color.FromArgb(176, 184, 196);

    /// <summary>Button background while the mouse is over it.</summary>
    public static readonly Color Hover = Color.FromArgb(228, 240, 237);

    /// <summary>Color for actionable error messages (contrast about 7:1 on Background).</summary>
    public static readonly Color Error = Color.FromArgb(160, 28, 28);

    /// <summary>Fixed-width font for the elapsed/total clock so digits do not jitter.</summary>
    public static readonly Font Clock = new("Consolas", 12f, FontStyle.Regular);

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

    /// <summary>
    /// Flat button style with a visible border and hover color. Unlike
    /// <see cref="FlatStyle.System"/>, a flat button can display an icon.
    /// </summary>
    /// <param name="button">Button to style.</param>
    public static void StyleButton(Button button)
    {
        ArgumentNullException.ThrowIfNull(button);
        button.FlatStyle = FlatStyle.Flat;
        button.UseVisualStyleBackColor = false;
        button.BackColor = Surface;
        button.ForeColor = Text;
        button.FlatAppearance.BorderColor = Border;
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.MouseOverBackColor = Hover;
        button.FlatAppearance.MouseDownBackColor = Border;
        button.Cursor = Cursors.Hand;
    }

    /// <summary>
    /// Loads <c>Assets/icons/&lt;name&gt;.png</c> from the application folder, scaled to a
    /// square of <paramref name="pixels"/>. Returns null (never throws) when the
    /// file is missing or unreadable so callers can fall back to a text-only button.
    /// </summary>
    /// <param name="name">Icon file name without extension, e.g. "play".</param>
    /// <param name="pixels">Target width and height in device pixels.</param>
    public static Image? LoadIcon(string name, int pixels)
    {
        if (string.IsNullOrWhiteSpace(name) || pixels <= 0) return null;

        try
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets", "icons", name + ".png");
            if (!File.Exists(path)) return null;

            using var source = Image.FromFile(path);
            var scaled = new Bitmap(pixels, pixels);
            using var g = Graphics.FromImage(scaled);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.DrawImage(source, 0, 0, pixels, pixels);
            return scaled;
        }
        catch (Exception ex) when (ex is IOException or OutOfMemoryException
            or ArgumentException or UnauthorizedAccessException)
        {
            return null;
        }
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
                StyleButton(button);
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

    // Cassette palette and project-asset loading used by the Designer-backed layout.

    /// <summary>Main window background.</summary>
    public static Color Canvas => Color.FromArgb(225, 174, 103);

    /// <summary>Warm paper for metadata and playlist panels.</summary>
    public static Color Paper => Color.FromArgb(243, 214, 170);

    /// <summary>Control-row color.</summary>
    public static Color Gold => Color.FromArgb(203, 142, 76);

    /// <summary>Seek and volume row color.</summary>
    public static Color LightGold => Color.FromArgb(232, 188, 125);

    /// <summary>Ink color used for readable text.</summary>
    public static Color Ink => Color.FromArgb(54, 40, 30);

    /// <summary>Cassette bay fallback color when artwork is unavailable.</summary>
    public static Color CassetteBay => Color.FromArgb(69, 39, 27);

    /// <summary>Looks for an original image inside the project's copied Assets folder.</summary>
    public static Image? LoadImage(string name)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Assets", name);
        if (!File.Exists(path)) return null;
        try
        {
            // Read all bytes first so the asset is not locked while the player runs.
            using var stream = new MemoryStream(File.ReadAllBytes(path));
            using Image original = Image.FromStream(stream);
            return new Bitmap(original);
        }
        catch (Exception ex) when (ex is IOException || ex is ArgumentException)
        {
            return null;
        }
    }

    /// <summary>Loads the group icon when the ICO asset is available.</summary>
    public static Icon? TryLoadIcon(string name)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Assets", name);
        if (!File.Exists(path)) return null;
        try { return new Icon(path); }
        catch (Exception ex) when (ex is IOException || ex is ArgumentException)
        {
            return null;
        }
    }

}
