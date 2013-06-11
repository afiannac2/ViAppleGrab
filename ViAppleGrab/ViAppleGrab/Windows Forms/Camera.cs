using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ViAppleGrab
{
    public partial class Camera : Form
    {
        public Camera()
        {
            InitializeComponent();
        }

        public void SetImage(ref Image i)
        {
            pbCamera.Image = i;
        }
    }
}
