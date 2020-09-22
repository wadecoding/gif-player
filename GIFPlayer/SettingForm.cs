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
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
            loop_checkBox.Checked = Properties.Settings.Default.loop;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.loop = loop_checkBox.Checked;
        }
    }
}
