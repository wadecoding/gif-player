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

        PlayerStatus playerStatus = new PlayerStatus();
        ImageManager manager = new ImageManager(new ImageCacheLoader());
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
            ChangeStatus(PlayerStatus.Status.Playing);
        }
        private void Stop()
        {
            ChangeStatus(PlayerStatus.Status.Pausing);
        }

        private void ChangeStatus(PlayerStatus.Status status)
        {
            if (status == playerStatus.status) return;

            playerStatus.status = status;
            switch (status)
            {
                case PlayerStatus.Status.Playing:
                    ForwardActiveFrame();
                    if (playerStatus.isAutoplay && timer.Enabled == false)
                    {
                        timer.Start();
                    }
                    controlButton.Image = Resources.stop_smaller;
                    stopButton.Enabled = true;
                    playButton.Enabled = false;
                    break;
                case PlayerStatus.Status.Pausing:
                    if (timer.Enabled)
                    {
                        timer.Stop();
                    }
                    controlButton.Image = Resources.play_smaller;
                    stopButton.Enabled = false;
                    playButton.Enabled = true;
                    break;
                case PlayerStatus.Status.SeekMoving:
                    if (timer.Enabled)
                    {
                        timer.Stop();
                    }
                    break;
            }
        }

        private void DrawPresentFrame(int idx)
        {
            pictureBox.Image = manager.Get(idx);
        }

        private void ChangeFrameView(int presentFrame, int totalFrame)
        {
            int presentFrameNumber = presentFrame + 1;
            ChangeFrameCounter(presentFrameNumber, totalFrame);
            seekbar.Value = presentFrameNumber;
        }

        private void ChangeFrameCounter(int presentFrame, int totalFrame)
        {
            string format = "D" + totalFrame.ToString().Length;
            frameCounter.Text = $"Frames: {presentFrame.ToString(format)} / {totalFrame}";
        }

        public void ChangeActiveFrame(int frame)
        {
            DrawPresentFrame(frame);
            ChangeFrameView(manager.ActiveFrameIndex, manager.TotalFramesCount);
            if (manager.ActiveFrameIndex == 0)
            {
                backwardButton.Enabled = false;
            }
            else
            {
                backwardButton.Enabled = true;
            }
            if (manager.ActiveFrameIndex + 1 == manager.TotalFramesCount)
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
            if (manager.ActiveFrameIndex + increment + 1 <= manager.TotalFramesCount)
            {
                manager.ActiveFrameIndex += increment;
                ChangeActiveFrame(manager.ActiveFrameIndex);
            }
            else if (manager.ActiveFrameIndex + increment + 1 > manager.TotalFramesCount)
            {
                if (Settings.Default.loop)
                {
                    manager.ActiveFrameIndex = 0;
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
            if (manager.ActiveFrameIndex - decrement >= 0)
            {
                manager.ActiveFrameIndex -= decrement;
                ChangeActiveFrame(manager.ActiveFrameIndex);
            }
            else if (manager.ActiveFrameIndex - decrement < 0)
            {
                if (Settings.Default.loop)
                {
                    manager.ActiveFrameIndex = manager.TotalFramesCount - 1;
                    ChangeActiveFrame(manager.ActiveFrameIndex);
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (playerStatus.isAutoplay)
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
            switch (playerStatus.status)
            {
                case PlayerStatus.Status.Pausing:
                    Play();
                    break;
                case PlayerStatus.Status.Playing:
                    Stop();
                    break;
            }
        }

        private void forwardButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(PlayerStatus.Status.Pausing);
            ForwardActiveFrame();
        }

        private void backwardButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(PlayerStatus.Status.Pausing);
            BackwardActiveFrame();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Gif Image (.gif)|*.gif|JPEG Image (.jpg)|*.jpg|Png Image (.png)|*.png";
            saveFileDialog.FilterIndex = 3;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                Image img = manager.Get(manager.ActiveFrameIndex);
                img.Save(filePath, GetExtensionFromPath(filePath));
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
            if (playerStatus.status == PlayerStatus.Status.SeekMoving)
            {
                ChangeFrameCounter(seekbar.Value, manager.TotalFramesCount);
            }
        }

        PlayerStatus.Status oldStatus;
        private void seekbar_MouseDown(object sender, MouseEventArgs e)
        {
            oldStatus = playerStatus.status;
            ChangeStatus(PlayerStatus.Status.SeekMoving);
        }

        private void seekbar_MouseUp(object sender, MouseEventArgs e)
        {
            if (playerStatus.status == PlayerStatus.Status.SeekMoving)
            {
                ChangeActiveFrame(seekbar.Value - 1);
                ChangeStatus(oldStatus);
            }
        }

        private void propertyButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(PlayerStatus.Status.Pausing);
            PropertyForm propertyForm = new PropertyForm(manager, playerStatus);
            propertyForm.ShowDialog();
            ChangeStatus(PlayerStatus.Status.Playing);
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
                    if (playerStatus.status == PlayerStatus.Status.Playing)
                    {
                        stopButton_Click(null, null);
                    }
                    else if (playerStatus.status == PlayerStatus.Status.Pausing)
                    {
                        playButton_Click(null, null);
                    }
                    break;
                case (Keys.Control | Keys.Up):
                    playerStatus.AdditionalDelayCoefficient *= 0.5f;
                    break;
                case (Keys.Control | Keys.Down):
                    playerStatus.AdditionalDelayCoefficient *= 2.0f;
                    break;
            }
            timer.Interval = (int)Math.Round(manager.Delay * playerStatus.AdditionalDelayCoefficient);



            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop, false) as string[];
            LoadFile(files[0]);
        }

        private async void LoadFile(string path)
        {
            this.Text = "Loading - GIFPlayer";
            await Task.Run(() => manager.Set(path));
            timer.Interval = manager.Delay;
            seekbar.Maximum = manager.TotalFramesCount;
            seekbar.Value = 1;
            saveButton.Enabled = true;
            propertyButton.Enabled = true;
            seekbar.Enabled = true;
            controlButton.Enabled = true;
            this.Text = $"{ Path.GetFileName(manager.Path) } - GIFPlayer";
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
    public class ImageCache
    {
        Image[] frames;
        public Image Load(string path)
        {
            Stream stream = File.OpenRead(path);
            Image src = Image.FromStream(stream);
            FrameDimension dimension = new FrameDimension(src.FrameDimensionsList.First());
            int frameCount = src.GetFrameCount(dimension);
            frames = new Image[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                src.SelectActiveFrame(FrameDimension.Page, i);
                frames[i] = new Bitmap(src);
            }
            return src;
        }
        public Image Get(int index)
        {
            return frames[index];
        }
    }
}
