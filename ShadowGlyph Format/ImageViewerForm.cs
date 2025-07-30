using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShadowGlyph_Format
{
    public class ImageViewerForm : Form
    {
        public ImageViewerForm(Image image)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            ShowIcon = false;
            ShowInTaskbar = true;

            Width = image.Width;
            Height = image.Height;

            PictureBox pb = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = image,
                SizeMode = PictureBoxSizeMode.Normal, // Kein Zoom = pixelgenau
            };

            Controls.Add(pb);
        }
    }
}
