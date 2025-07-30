using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ShadowGlyph_Format
{
    public partial class ShadowGlyph_Format_Main : Form
    {
        public ShadowGlyph_Format_Main()
        {
            InitializeComponent();
            InitPictureBox();
        }

        public ShadowGlyph_Format_Main(string fileToLoad) : this()
        {
            LoadAndDisplaySGLF(fileToLoad);
        }

        private void InitPictureBox()
        {
            picture.SizeMode = PictureBoxSizeMode.Zoom;
            picture.BackColor = Color.Transparent;
            picture.BorderStyle = BorderStyle.FixedSingle;
            picture.Width = 256;  // Optional größere Anzeige
            picture.Height = 256;
            picture.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            picture.Image = null;
        }

        public static Bitmap CreateImageFromColorString(string input)
        {
            const int width = 256;
            const int height = 256;
            int expectedLength = width * height * 6;

            if (input.Length < expectedLength)
                return null;

            Bitmap bmp = new Bitmap(width, height);
            int index = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (index + 6 > input.Length)
                        break;

                    try
                    {
                        int r = Convert.ToInt32(input.Substring(index, 2), 16);
                        int g = Convert.ToInt32(input.Substring(index + 2, 2), 16);
                        int b = Convert.ToInt32(input.Substring(index + 4, 2), 16);
                        bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                        index += 6;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return bmp;
        }

        private string ConvertToHex(Bitmap bmp)
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    sb.Append(c.R.ToString("X2"));
                    sb.Append(c.G.ToString("X2"));
                    sb.Append(c.B.ToString("X2"));
                }
            }

            return sb.ToString(); // 98304 Zeichen
        }

        private void LoadAndDisplaySGLF(string filePath)
        {
            try
            {
                string input = File.ReadAllText(filePath);
                Image image = CreateImageFromColorString(input);
                if (image == null)
                    throw new Exception("Datei ist beschädigt oder hat ungültiges Format.");

                picture.Image = image;
                this.Text = $"SGLF Viewer – {Path.GetFileName(filePath)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Datei:\n" + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // PNG → SGLF
        private void button2_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PNG-Bilder|*.png";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Bitmap original = new Bitmap(openFileDialog.FileName);
                        Bitmap resized = new Bitmap(original, new Size(256, 256));
                        string hexData = ConvertToHex(resized);
                        string outputPath = Path.ChangeExtension(openFileDialog.FileName, ".sglf");

                        File.WriteAllText(outputPath, hexData);
                        MessageBox.Show($"Gespeichert als: {outputPath}", "Export erfolgreich");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler beim Konvertieren:\n" + ex.Message);
                    }
                }
            }
        }

        // SGLF öffnen
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "ShadowGlyph Files (*.sglf)|*.sglf";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadAndDisplaySGLF(ofd.FileName);
                }
            }
        }

        // Zufallsbild erzeugen und anzeigen
        private void buttonGenerateRandom_Click(object sender, EventArgs e)
        {
            const int width = 128;
            const int height = 128;
            Random rnd = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < width * height; i++)
            {
                int r = rnd.Next(0, 256);
                int g = rnd.Next(0, 256);
                int b = rnd.Next(0, 256);
                sb.Append(r.ToString("X2"));
                sb.Append(g.ToString("X2"));
                sb.Append(b.ToString("X2"));
            }

            string hexOutput = sb.ToString();
            File.WriteAllText("random_128x128.sglf", hexOutput);
            MessageBox.Show("Zufallsbild gespeichert als random_128x128.sglf");

            Image image = CreateImageFromColorString(hexOutput);
            picture.Image = image;
        }
    }
}
