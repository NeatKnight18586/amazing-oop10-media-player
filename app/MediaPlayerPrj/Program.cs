using System;
using System.Windows.Forms;

namespace mediaPlayerPrj;

/// <summary>Application entry point for the reconstructed instructor sample.</summary>
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        try
        {
            Application.Run(new Form1());
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Amazing Player could not start. Check VLC dependencies.\n\n" + ex.Message,
                "Amazing Player",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
