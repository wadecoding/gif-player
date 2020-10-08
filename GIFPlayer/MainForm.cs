using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using GIFPlayer.Properties;
using System.Reflection;

namespace GIFPlayer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public static Image FromFile(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            MemoryStream ms = new MemoryStream(bytes);
            Image img = Image.FromStream(ms);
            return img;
        }

        public struct GIFFileInfo
        {
            public string filePath { get; private set; }
            public Image image { get; private set; }
            public FrameDimension dimension { get; private set; }
            public int totalFrames { get; private set; }
            public int activeFrame { get; private set; }
            public float delayCoe { get; set; }
            public int intrinsicDelay { get; private set; }
            public void Initialize(string _filePath)
            {
                filePath = _filePath;
                if (image != null) { image.Dispose(); }
                image = FromFile(filePath);
                dimension = new FrameDimension(image.FrameDimensionsList.First());
                totalFrames = image.GetFrameCount(dimension);
                activeFrame = 1;
                PropertyItem item = gifFileInfo.image.GetPropertyItem(0x5100);
                intrinsicDelay = (item.Value[0] + item.Value[1] * 256) * 10;
                delayCoe = 1.0f;
            }
            public void SetActiveFrame(int frame)
            {
                image.SelectActiveFrame(dimension, frame);
                activeFrame = frame;
            }
        }
        struct GIFFileStatus
        {
            public enum Status
            {
                Playing, Pausing, SeekMoving
            }
            public Status status;
            public bool isAutoplay;
            public void ToggleAutoplay(bool? _autoplay)
            {
                if (_autoplay != null)
                {
                    isAutoplay = (bool)_autoplay;
                }
                else
                {
                    isAutoplay ^= true;
                }
            }
            public void Initialize(bool _isAutoplay)
            {
                isAutoplay = _isAutoplay;
                status = Status.Pausing;
            }
        }
        static GIFFileInfo gifFileInfo;
        static GIFFileStatus gifFileStatus;


        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "gif files (*.gif)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFile(openFileDialog.FileName);
            }
        }

        private void Play()
        {
            ChangeStatus(GIFFileStatus.Status.Playing);
        }
        private void Stop()
        {
            ChangeStatus(GIFFileStatus.Status.Pausing);
        }

        private void ChangeStatus(GIFFileStatus.Status status)
        {
            if (status == gifFileStatus.status) return;

            gifFileStatus.status = status;
            switch (status)
            {
                case GIFFileStatus.Status.Playing:
                    ForwardActiveFrame();
                    if (gifFileStatus.isAutoplay && timer.Enabled == false)
                    {
                        timer.Start();
                    }
                    controlButton.Image = Properties.Resources.stop_smaller;
                    stopButton.Enabled = true;
                    playButton.Enabled = false;
                    break;
                case GIFFileStatus.Status.Pausing:
                    if (timer.Enabled)
                    {
                        timer.Stop();
                    }
                    controlButton.Image = Properties.Resources.play_smaller;
                    stopButton.Enabled = false;
                    playButton.Enabled = true;
                    break;
                case GIFFileStatus.Status.SeekMoving:
                    if (timer.Enabled)
                    {
                        timer.Stop();
                    }
                    break;
            }
        }

        private void DrawPresentFrame()
        {
            pictureBox.Image = gifFileInfo.image;
        }

        private void ChangeFrameView(int presentFrame, int totalFrame)
        {
            presentFrame++;
            ChangeFrameCounter(presentFrame, totalFrame);
            seekbar.Value = presentFrame;
        }

        private void ChangeFrameCounter(int presentFrame, int totalFrame)
        {
            string format = "D" + totalFrame.ToString().Length;
            frameCounter.Text = $"Frames: {presentFrame.ToString(format)} / {totalFrame}";
        }

        public void ChangeActiveFrame(int frame)
        {
            gifFileInfo.SetActiveFrame(frame);
            DrawPresentFrame();
            ChangeFrameView(gifFileInfo.activeFrame, gifFileInfo.totalFrames);
            if (gifFileInfo.activeFrame == 0)
            {
                backwardButton.Enabled = false;
            }
            else
            {
                backwardButton.Enabled = true;
            }
            if (gifFileInfo.activeFrame + 1 == gifFileInfo.totalFrames)
            {
                forwardButton.Enabled = false;
            }
            else
            {
                forwardButton.Enabled = true;
            }
        }

        public void ForwardActiveFrame(int increment = 1)
        {
            if (gifFileInfo.activeFrame + increment + 1 <= gifFileInfo.totalFrames)
            {
                ChangeActiveFrame(gifFileInfo.activeFrame + increment);
            }
            else if (gifFileInfo.activeFrame + increment + 1 > gifFileInfo.totalFrames)
            {
                if (Properties.Settings.Default.loop)
                {
                    ChangeActiveFrame(0);
                }
                else
                {
                    controlButton.Image = Resources.play_smaller;
                    stopButton.Enabled = false;
                    playButton.Enabled = true;
                }
            }
        }
        public void BackwardActiveFrame(int decrement = 1)
        {
            if (gifFileInfo.activeFrame - decrement >= 0)
            {
                ChangeActiveFrame(gifFileInfo.activeFrame - decrement);
            }
            else if (gifFileInfo.activeFrame - decrement < 0)
            {
                if (Properties.Settings.Default.loop)
                {
                    ChangeActiveFrame(gifFileInfo.totalFrames - 1);
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (gifFileStatus.isAutoplay)
            {
                ForwardActiveFrame();
            }
            else
            {
                timer.Stop();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void controlButton_Click(object sender, EventArgs e)
        {
            switch (gifFileStatus.status)
            {
                case GIFFileStatus.Status.Pausing:
                    Play();
                    break;
                case GIFFileStatus.Status.Playing:
                    Stop();
                    break;
            }
        }

        private void forwardButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(GIFFileStatus.Status.Pausing);
            ForwardActiveFrame();
        }

        private void backwardButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(GIFFileStatus.Status.Pausing);
            BackwardActiveFrame();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Gif Image (.gif)|*.gif|JPEG Image (.jpg)|*.jpg|Png Image (.png)|*.png";
            saveFileDialog.FilterIndex = 3;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                gifFileInfo.image.Save(filePath, GetExtensionFromPath(filePath));
            }
        }

        private ImageFormat GetExtensionFromPath(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            switch (extension.ToLower())
            {
                case ".gif":
                    return ImageFormat.Gif;
                case ".jpg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                default:
                    return null;
            }
        }

        private void seekbar_ValueChanged(object sender, EventArgs e)
        {
            if (gifFileStatus.status == GIFFileStatus.Status.SeekMoving)
            {
                ChangeFrameCounter(seekbar.Value, gifFileInfo.totalFrames);
            }
        }

        GIFFileStatus.Status oldStatus;
        private void seekbar_MouseDown(object sender, MouseEventArgs e)
        {
            oldStatus = gifFileStatus.status;
            ChangeStatus(GIFFileStatus.Status.SeekMoving);
        }

        private void seekbar_MouseUp(object sender, MouseEventArgs e)
        {
            if (gifFileStatus.status == GIFFileStatus.Status.SeekMoving)
            {
                ChangeActiveFrame(seekbar.Value - 1);
                ChangeStatus(oldStatus);
            }
        }

        private void propertyButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(GIFFileStatus.Status.Pausing);
            PropertyForm propertyForm = new PropertyForm(gifFileInfo);
            propertyForm.ShowDialog();
            ChangeStatus(GIFFileStatus.Status.Playing);
        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            SettingForm settingForm = new SettingForm();
            Properties.Settings.Default.Reload();
            if (settingForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.Save();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case (Keys.Control | Keys.S):
                    if (saveButton.Enabled)
                    {
                        saveButton_Click(null, null);
                    }
                    break;
                case (Keys.Control | Keys.Right):
                    if (forwardButton.Enabled)
                    {
                        forwardButton_Click(null, null);
                    }
                    break;
                case (Keys.Control | Keys.Left):
                    if (backwardButton.Enabled)
                    {
                        backwardButton_Click(null, null);
                    }
                    break;
                case (Keys.Control | Keys.Space):
                    if (gifFileStatus.status == GIFFileStatus.Status.Playing)
                    {
                        stopButton_Click(null, null);
                    }
                    else if (gifFileStatus.status == GIFFileStatus.Status.Pausing)
                    {
                        playButton_Click(null, null);
                    }
                    break;
                case (Keys.Control | Keys.Up):
                    if (gifFileInfo.image != null)
                    {
                        gifFileInfo.delayCoe *= 0.5f;
                    }
                    break;
                case (Keys.Control | Keys.Down):
                    if (gifFileInfo.image != null)
                    {
                        gifFileInfo.delayCoe *= 2.0f;
                    }
                    break;
            }
            timer.Interval = (int)Math.Round(gifFileInfo.intrinsicDelay * gifFileInfo.delayCoe);



            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop, false) as string[];
            LoadFile(files[0]);
        }

        private void LoadFile(string filepath)
        {
            gifFileInfo.Initialize(filepath);
            gifFileStatus.Initialize(true);
            timer.Interval = gifFileInfo.intrinsicDelay;
            seekbar.Maximum = gifFileInfo.totalFrames;
            seekbar.Value = 1;
            saveButton.Enabled = true;
            propertyButton.Enabled = true;
            seekbar.Enabled = true;
            controlButton.Enabled = true;
            Play();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files.Count() > 1) return;
                if (Path.GetExtension(files[0]).ToLower() != ".gif") return;
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}
