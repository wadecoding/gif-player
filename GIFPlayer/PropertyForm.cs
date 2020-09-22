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
        public PropertyForm(MainForm.GIFFileInfo info)
        {
            InitializeComponent();
            thumbnail.Enabled = false;
            thumbnail.Image = info.image;
            filePathTextBox.Text = $"{info.filePath}";
            totalFramesLabel.Text = $"Total Frames: {info.totalFrames.ToString()}";
            delayLabel.Text = $"Delay time: {info.intrinsicDelay.ToString()} X {info.delayCoe.ToString()} ms";
            sizeLabel.Text = $"Size: {info.image.Width} x {info.image.Height}";
        }
    }
}
