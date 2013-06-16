using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ViAppleGrab.Properties;

namespace ViAppleGrab.Windows_Forms
{
    public partial class VerifyIP : Form
    {
        public VerifyIP()
        {
            InitializeComponent();

            tbIP.Text = Settings.Default.IP_ADDRESS;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The IP Address can be found in the PS3 Main Menu under Settings > Network Settings > Settings and Connection Status List");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Settings.Default.IP_ADDRESS = tbIP.Text;
            this.Close();
        }
    }
}
