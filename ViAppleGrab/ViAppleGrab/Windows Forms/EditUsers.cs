using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ViAppleGrab.Windows_Forms
{
    public partial class EditUsers : Form
    {
        XDocument users;

        public EditUsers()
        {
            InitializeComponent();

            users = XDocument.Load("Users.xml");
        }
    }
}
