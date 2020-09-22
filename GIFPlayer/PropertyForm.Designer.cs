namespace GIFPlayer
{
    partial class PropertyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyForm));
            this.filePathLabel = new System.Windows.Forms.Label();
            this.totalFramesLabel = new System.Windows.Forms.Label();
            this.delayLabel = new System.Windows.Forms.Label();
            this.sizeLabel = new System.Windows.Forms.Label();
            this.thumbnail = new System.Windows.Forms.PictureBox();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnail)).BeginInit();
            this.SuspendLayout();
            // 
            // filePathLabel
            // 
            this.filePathLabel.AutoSize = true;
            this.filePathLabel.Location = new System.Drawing.Point(124, 14);
            this.filePathLabel.Name = "filePathLabel";
            this.filePathLabel.Size = new System.Drawing.Size(56, 12);
            this.filePathLabel.TabIndex = 1;
            this.filePathLabel.Text = "File path :";
            // 
            // totalFramesLabel
            // 
            this.totalFramesLabel.AutoSize = true;
            this.totalFramesLabel.Location = new System.Drawing.Point(124, 39);
            this.totalFramesLabel.Name = "totalFramesLabel";
            this.totalFramesLabel.Size = new System.Drawing.Size(80, 12);
            this.totalFramesLabel.TabIndex = 2;
            this.totalFramesLabel.Text = "Total frames : ";
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(124, 64);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(44, 12);
            this.delayLabel.TabIndex = 3;
            this.delayLabel.Text = "Delay : ";
            // 
            // sizeLabel
            // 
            this.sizeLabel.AutoSize = true;
            this.sizeLabel.Location = new System.Drawing.Point(124, 89);
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(36, 12);
            this.sizeLabel.TabIndex = 4;
            this.sizeLabel.Text = "Size : ";
            // 
            // thumbnail
            // 
            this.thumbnail.Image = ((System.Drawing.Image)(resources.GetObject("thumbnail.Image")));
            this.thumbnail.Location = new System.Drawing.Point(12, 12);
            this.thumbnail.Name = "thumbnail";
            this.thumbnail.Size = new System.Drawing.Size(96, 92);
            this.thumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.thumbnail.TabIndex = 0;
            this.thumbnail.TabStop = false;
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.filePathTextBox.Location = new System.Drawing.Point(186, 12);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.ReadOnly = true;
            this.filePathTextBox.Size = new System.Drawing.Size(225, 19);
            this.filePathTextBox.TabIndex = 5;
            this.filePathTextBox.TabStop = false;
            // 
            // PropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 119);
            this.Controls.Add(this.filePathTextBox);
            this.Controls.Add(this.sizeLabel);
            this.Controls.Add(this.delayLabel);
            this.Controls.Add(this.totalFramesLabel);
            this.Controls.Add(this.filePathLabel);
            this.Controls.Add(this.thumbnail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PropertyForm";
            this.Text = "PropertyForm";
            ((System.ComponentModel.ISupportInitialize)(this.thumbnail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox thumbnail;
        private System.Windows.Forms.Label filePathLabel;
        private System.Windows.Forms.Label totalFramesLabel;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.Label sizeLabel;
        private System.Windows.Forms.TextBox filePathTextBox;
    }
}