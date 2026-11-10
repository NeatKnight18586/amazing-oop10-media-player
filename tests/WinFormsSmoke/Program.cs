using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using mediaPlayerPrj;

namespace WinFormsSmoke;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        try
        {
            ApplicationConfiguration.Initialize();

            using (var splash = new SplashForm())
            {
                Require(splash.Controls.Find("lblAppName", true).Length == 1, "Splash title");
                Require(splash.Controls.Find("lblGroup", true).Length == 1, "Splash group");
                Require(TeamInfo.Members.Count == 5, "Five team members");
                for (int index = 1; index <= 5; index++)
                    Require(splash.Controls.Find("picMember" + index, true).Length == 1,
                        "Member photo card " + index);
                CaptureIfPossible(splash, "splash.png");
            }

            using (var form = new Form1())
            {
                Require(form.Controls.Find("menuMain", true).Length == 1, "Menu strip");
                Require(form.Controls.Find("lstPlaylist", true).Length == 1, "Playlist");
                Require(form.Controls.Find("pnlVideo", true).Length == 1, "Video host");
                Require(form.Controls.Find("btnPlay", true).Length == 1, "Play button");
                Require(form.Controls.Find("btnFavorite", true).Length == 1, "Favorite button");
                Require(form.Controls.Find("btnShuffle", true).Length == 1, "Shuffle button");
                Require(form.Controls.Find("btnRepeat", true).Length == 1, "Repeat button");
                Require(form.Controls.Find("trkSeek", true).Length == 1, "Seek bar");
                Require(form.Controls.Find("trkVolume", true).Length == 1, "Volume control");
                CaptureIfPossible(form, "main-window.png");
                // Validate that controls remain on-screen at the minimum window size.
                form.ClientSize = new Size(960, 640);
                form.Show();
                Application.DoEvents();
                var transport = form.Controls.Find("flowTransport", true).Single();
                var cassette = form.Controls.Find("pnlCassette", true).Single();
                var title = form.Controls.Find("lblCassetteTitle", true).Single();
                Require(title.Left >= 0 && title.Right <= cassette.ClientSize.Width,
                    "Cassette title should stay inside the player panel");
                foreach (string button in new [] {
                    "btnPrevious", "btnPlay", "btnPause", "btnStop", "btnNext",
                    "btnAdd", "btnFavorite", "btnShuffle", "btnRepeat" })
                {
                    var control = form.Controls.Find(button, true).Single();
                    Require(control.Left >= 0 && control.Right <= transport.ClientSize.Width,
                        button + " should not extend outside the transport row");
                }
                form.Hide();
            }

            Console.WriteLine("PASS: Main and splash forms constructed; required named WinForms controls present.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("FAIL: Windows Forms startup check.");
            Console.Error.WriteLine(ex);
            return 1;
        }
    }

    private static void CaptureIfPossible(Form form, string filename)
    {
        try
        {
            form.Show();
            Application.DoEvents();
            using var bitmap = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
            string output = Path.Combine(Environment.CurrentDirectory, "ui-captures");
            Directory.CreateDirectory(output);
            bitmap.Save(Path.Combine(output, filename), ImageFormat.Png);
            form.Hide();
            Console.WriteLine("Captured " + filename);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Layout screenshot unavailable in this Windows session: " + ex.Message);
        }
    }

    private static void Require(bool condition, string name)
    {
        if (!condition)
            throw new InvalidOperationException(name + " was not found.");
    }
}
