using System;
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

    private static void Require(bool condition, string name)
    {
        if (!condition)
            throw new InvalidOperationException(name + " was not found.");
    }
}
