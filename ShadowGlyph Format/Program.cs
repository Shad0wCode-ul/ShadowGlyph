using System;
using System.IO;
using System.Windows.Forms;

namespace ShadowGlyph_Format
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            if (args.Length > 0 && File.Exists(args[0]) && Path.GetExtension(args[0]).ToLower() == ".sglf")
            {
                try
                {
                    string hex = File.ReadAllText(args[0]);
                    var form = new ImageViewerForm(ShadowGlyph_Format_Main.CreateImageFromColorString(hex));
                    Application.Run(form);
                }
                catch
                {
                    MessageBox.Show("Ungültige SGLF-Datei", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Application.Run(new ShadowGlyph_Format_Main());
            }
        }
    }
}