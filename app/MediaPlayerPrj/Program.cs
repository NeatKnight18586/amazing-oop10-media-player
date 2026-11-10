using System;
using System.Windows.Forms;

namespace mediaPlayerPrj;

/// <summary>Application entry point.</summary>
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        try
        {
            using (var splash = new SplashForm())
                splash.ShowDialog();
            Application.Run(new Form1());
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Amazing Player could not start.\n\n" + ex.Message,
                "Amazing Player",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
