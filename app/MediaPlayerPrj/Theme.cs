using System;
using System.Drawing;
using System.IO;

namespace mediaPlayerPrj;

/// <summary>Original cassette palette and image asset lookup for the player UI.</summary>
public static class Theme
{
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
