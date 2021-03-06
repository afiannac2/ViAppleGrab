﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ViAppleGrab.Properties;
using System.Xml;
using ViToolkit.Logging;
using System.IO;
using System.Diagnostics;

namespace ViAppleGrab
{
    public partial class UserSelection : Form
    {
        public Camera CameraForm = null;
        private UserInfo UserInfoForm = null;
        XmlDocument userDoc = null;
        Dictionary<string, string> userDic = new Dictionary<string, string>();
        ViAppleGrabGame game = null;

        public UserSelection(ViAppleGrabGame g)
        {
            game = g;

            InitializeComponent();

            //Initialize other stuff
            this.FormClosing += new FormClosingEventHandler(UserInformation_FormClosing);

            if (Settings.Default.SHOW_CAMERA)
            {
                CameraForm = new Camera();
                CameraForm.Show();
                CameraForm.FormClosed += delegate { CameraForm = null; };
            }

            _refreshUsers();
        }

        public void _refreshUsers()
        {
            cmbUsers.Items.Clear();
            userDic.Clear();

            userDoc = new XmlDocument();
            userDoc.Load("Users.xml");
            XmlNodeList nodes = userDoc.SelectNodes("//User");

            foreach (XmlNode n in nodes)
            {
                userDic.Add(n.Attributes["ID"].Value,
                            n.SelectSingleNode("LastName").InnerText
                            + ", " + n.SelectSingleNode("FirstName").InnerText);
            }

            //Add the users to the drop down in the order of their IDs
            for (int i = 1; i < userDic.Count + 1; i++)
            {
                cmbUsers.Items.Add(userDic[i.ToString()]);
            }

            userDoc = null;

            cmbUsers.Update();
        }

        void UserInformation_FormClosing(object sender, FormClosingEventArgs e)
        {
            ViAppleGrabInput.GameHasFocus = true;

            if (CameraForm != null)
                CameraForm.Close();
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            if (CameraForm != null)
            {
                btnEditUser.Enabled = false;
                btnNewUser.Enabled = false;
                btnStartGame.Enabled = false;
                cmbUsers.Enabled = false;

                CameraForm.Focus();

                ((ViAppleGrabInput)game.Components[0]).Controllers.ReinitializeControllers();
                ((ViAppleGrabLogic)game.Components[1]).UserID = cmbUsers.SelectedIndex + 1;
                ((ViAppleGrabLogic)game.Components[1]).NewGame();
            }
            else
            {
                ((ViAppleGrabInput)game.Components[0]).Controllers.ReinitializeControllers();
                ((ViAppleGrabLogic)game.Components[1]).UserID = cmbUsers.SelectedIndex + 1;
                ((ViAppleGrabLogic)game.Components[1]).NewGame();

                this.Enabled = false;
            }

            ViAppleGrabInput.GameHasFocus = true;
        }

        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUsers.SelectedIndex != (-1))
            {
                btnEditUser.Enabled = true;

                XmlNode node = XMLTrace.FindUserInfoNode("Users.xml", cmbUsers.SelectedIndex + 1);

                switch(node.SelectSingleNode("TestGroup").InnerText)
                {
                    case "A":
                        cmbTestGroup.SelectedIndex = 0;
                        cmbGameType.SelectedIndex = 0;
                        cmbControlType.SelectedIndex = 0;
                        break;

                    case "B":
                        cmbTestGroup.SelectedIndex = 1;
                        cmbGameType.SelectedIndex = 0;
                        cmbControlType.SelectedIndex = 1;
                        break;
                }
            }
        }

        private void btnNewUser_Click(object sender, EventArgs e)
        {
            UserInfoForm = new UserInfo(UserInfoMode.New, cmbUsers.Items.Count + 1, this);

            UserInfoForm.FormClosed += delegate
            {
                UserInfoForm = null;
            };
            
            UserInfoForm.Show();
            UserInfoForm.Focus();
        }

        private void cbQuickCalibration_CheckedChanged(object sender, EventArgs e)
        {
            if (cbQuickCalibration.Checked)
                Settings.Default.QUICK_CALIBRATION = true;
            else
                Settings.Default.QUICK_CALIBRATION = false;
        }

        private void cbShowCamera_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowCamera.Checked)
            {
                CameraForm = new Camera();
                ((ViAppleGrabInput)game.Components[0]).TurnOnCamera();
                CameraForm.Show();
                CameraForm.FormClosed += delegate { CameraForm = null; };
            }
            else
            {
                ((ViAppleGrabInput)game.Components[0]).TurnOffCamera();

                if (CameraForm != null)
                {
                    CameraForm.Close();
                }
            }
        }

        private void cmbGameType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGameType.SelectedItem.ToString() == "Apples Only")
            {
                Settings.Default.GAME_TYPE = 0;
            }
            else
            {
                Settings.Default.GAME_TYPE = 1;
            }
        }

        private void cmbControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbControlType.SelectedItem.ToString() == "Alternating")
            {
                Settings.Default.CONTROL_TYPE = (int)ControlType.Alternating;
                cmbYAxisController.Visible = false;
                lblYAxisController.Visible = false;
                Settings.Default.RIGHT_GIVES_Y_FEEDBACK = true;
            }
            else
            {
                Settings.Default.CONTROL_TYPE = (int)ControlType.Together;
                cmbYAxisController.Visible = true;
                lblYAxisController.Visible = true;
                cmbYAxisController.SelectedIndex = 0;
            }
        }

        private void cmbYAxisController_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbYAxisController.SelectedItem.ToString() == "Right Controller")
            {
                Settings.Default.RIGHT_GIVES_Y_FEEDBACK = true;
            }
            else
            {
                Settings.Default.RIGHT_GIVES_Y_FEEDBACK = false;
            }
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            UserInfoForm = new UserInfo(UserInfoMode.Edit, cmbUsers.SelectedIndex + 1, this);
            
            UserInfoForm.FormClosed += delegate 
            { 
                UserInfoForm = null;
            };
            
            UserInfoForm.Show();
            UserInfoForm.Focus();
        }

        public void SetSelectedUser(int ID)
        {
            cmbUsers.SelectedIndex = ID - 1;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViAppleGrabInput.GameHasFocus = true;
            ((ViAppleGrabLogic)game.Components[1]).ShutDown();
            this.Close();
        }

        private void purgeResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string m = "Are you sure you wish to purge all results files? This is not a reversible operation!";
            string c = "WARNING!";

            DialogResult r = MessageBox.Show(m, c, MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                string[] files = Directory.GetFiles("Results");

                foreach (string file in files)
                {
                    File.Delete(file);
                }

                MessageBox.Show("All results files have been deleted!");
            }
            else
            {
                MessageBox.Show("No files have been deleted...");
            }
        }

        private void purgeDebugLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string m = "Are you sure you wish to purge all debug log files? This is not a reversible operation!";
            string c = "WARNING!";

            DialogResult r = MessageBox.Show(m, c, MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                string[] files = Directory.GetFiles("DebugLogs");

                foreach (string file in files)
                {
                    //Don't delete the current debug listener - trying to delete
                    //  it will throw an IOException
                    try
                    {
                        File.Delete(file);
                    }
                    catch (IOException) { }
                }

                MessageBox.Show("All debug log files have been deleted!");
            }
            else
            {
                MessageBox.Show("No files have been deleted...");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTypeOfPlay.SelectedItem.ToString() == "Warmup")
            {
                Settings.Default.TOGETHER_FILE = "Warmup_Together.txt";
                Settings.Default.ALTERNATING_FILE = "Warmup_Alternating.txt";
                Settings.Default.MAX_LEVELS = 1;
            }
            else
            {
                Settings.Default.TOGETHER_FILE = "Targets_Together.txt";
                Settings.Default.ALTERNATING_FILE = "Targets_Alternating.txt";
                Settings.Default.MAX_LEVELS = 4;
            }
        }
    }
}
