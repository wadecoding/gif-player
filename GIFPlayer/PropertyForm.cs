using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIFPlayer
{
    public partial class PropertyForm : Form
    {
        public PropertyForm(ImageManager manager, PlayerStatus status)
        {
            InitializeComponent();
            Image image = manager.Get(0);
            thumbnail.Enabled = false;
            thumbnail.Image = image;
            filePathTextBox.Text = $"{manager.Path}";
            totalFramesLabel.Text = $"Total Frames: {manager.TotalFramesCount}";
            delayLabel.Text = $"Delay time: {manager.Delay} X {status.AdditionalDelayCoefficient} ms";
            sizeLabel.Text = $"Size: {image.Width} x {image.Height}";
        }
    }
}
