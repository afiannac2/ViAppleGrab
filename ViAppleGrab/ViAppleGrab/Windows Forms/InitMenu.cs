using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System;
using ViAppleGrab.Properties;
using ViToolkit.Logging;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace ViAppleGrab.Windows_Forms
{
    public partial class InitMenu : Form
    {
        bool Stage1Complete = false;
        bool Stage2Complete = false;
        bool Stage3Complete = false;

        public InitMenu()
        {
            InitializeComponent();

            btnPlay.Enabled = false;
            btnPlay2.Enabled = false;
            btnPlay3.Enabled = false;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            //Warn the user to setup the playstation Move.Me server before continuing
            string message = "Before this game attempts to connect to the "
            + "Playstation 3, please ensure that the computer is networked to the "
            + "playstation and the Move.Me server game is running on the "
            + "playstation with both controllers active. \n\n"
            + "Press the OK button if this is already complete or after you have "
            + "completed this; otherwise, press Cancel to close this game.";

            DialogResult res = MessageBox.Show(message, "Warning!", MessageBoxButtons.OKCancel);

            if (res == DialogResult.OK)
            {
                Settings.Default.STAGE = (int)StudyStages.Warmup;

                StartGameThread();

                btnPlay.Text = "Stage #1 Complete";
                btnPlay.Enabled = false;

                Stage1Complete = true;
            }
        }

        private void btnPlay2_Click(object sender, EventArgs e)
        {
            if (!Stage1Complete)
            {
                DialogResult res = MessageBox.Show("Are you sure you wish to run Stage 2 before Stage 1 has been completed?", "Warning!", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    Settings.Default.STAGE = (int)StudyStages.Single;

                    StartGameThread();

                    btnPlay2.Text = "Stage #2 Complete";
                    btnPlay2.Enabled = false;
                    Stage2Complete = true;
                }
            }
            else
            {
                Settings.Default.STAGE = (int)StudyStages.Single;

                StartGameThread();

                btnPlay2.Text = "Stage #2 Complete";
                btnPlay2.Enabled = false;
                Stage2Complete = true;
            }
        }

        private void btnPlay3_Click(object sender, EventArgs e)
        {
            if (!Stage2Complete)
            {
                DialogResult res = MessageBox.Show("Are you sure you wish to run Stage 2 before Stage 1 has been completed?", "Warning!", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    Settings.Default.STAGE = (int)StudyStages.Simultaneous;

                    StartGameThread();

                    btnPlay3.Text = "Stage #3 Complete";
                    btnPlay3.Enabled = false;
                    Stage3Complete = true;
                }
            }
            else
            {
                Settings.Default.STAGE = (int)StudyStages.Simultaneous;

                StartGameThread();

                btnPlay3.Text = "Stage #3 Complete";
                btnPlay3.Enabled = false;
                Stage3Complete = true;
            }
        }

        private void StartGameThread()
        {
            Thread t = new Thread(new ThreadStart(() =>
            {

                using (ViAppleGrabGame game = new ViAppleGrabGame())
                {
                    DateTime now = DateTime.Now;

                    try
                    {
                        //Create the results trace file
                        string fname = now.Month.ToString("D2") + now.Day.ToString("D2") + now.ToString("yy");

                        string[] files = Directory.GetFiles("Results", fname + "_*.*", SearchOption.AllDirectories);

                        fname += "_" + files.Length.ToString() + ".xml";

                        XMLTrace.CreateTraceFile(fname);

                        game.Run();
                    }
                    catch (Exception ex)
                    {
                        ((ViAppleGrabInput)game.Components[0]).Controllers.StopRumbles();

                        Console.WriteLine(ex.Message);

                        if (ex.InnerException != null)
                            Console.WriteLine("Inner Exception: " + ex.InnerException.Message);

                        MessageBox.Show("A fatal error has occurred. Please restart the program.");
                    }
                    finally
                    {
                        XMLTrace.Save();

                        if (Settings.Default.DISPLAY_RESULTS_AT_END)
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                            {
                                FileName = Directory.GetCurrentDirectory() + @"\Results",
                                UseShellExecute = true,
                                Verb = "open"
                            });
                    }
                }

            }));
            t.Start();
        }

        private void btnViewResults_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = Directory.GetCurrentDirectory() + @"\Results",
                UseShellExecute = true,
                Verb = "open"
            });
        }

        private void btnEditUsers_Click(object sender, EventArgs e)
        {
            EditUsers form = new EditUsers();
            form.ShowDialog();
            if (Settings.Default.CURRENT_USER_ID == -1)
            {
                lblCurrentUserLbl.Text = "No user is currently selected!";
                lblCurrentUser.Text = "";

                btnPlay.Enabled = false;
                btnPlay2.Enabled = false;
                btnPlay3.Enabled = false;
            }
            else
            {
                lblCurrentUserLbl.Text = "Current User:";

                XDocument users;
                string usersfile = "Users.xml";
                users = XDocument.Load(usersfile);
                var q = users.Descendants().Elements("User")
                    .Where(u => u.Attribute("ID").Value == Settings.Default.CURRENT_USER_ID.ToString())
                    .Select(user => new
                    {
                        Name = user.Element("FirstName").Value + " " + user.Element("LastName").Value
                    })
                    .FirstOrDefault();

                lblCurrentUser.Text = q.Name;

                btnPlay.Enabled = true;
                btnPlay2.Enabled = true;
                btnPlay3.Enabled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Stage1Complete = false;
            Stage2Complete = false;
            Stage3Complete = false;

            btnPlay.Enabled = true;
            btnPlay.Text = "2. Run Stage #1: Warmup";
            btnPlay2.Enabled = true;
            btnPlay2.Text = "3. Run Stage #2: Singles";
            btnPlay3.Enabled = true;
            btnPlay3.Text = "4. Run Stage #3: Simultaneous";

            lblCurrentUserLbl.Text = "No user is currently selected!";
            lblCurrentUser.Text = "";

            btnPlay.Enabled = false;
            btnPlay2.Enabled = false;
            btnPlay3.Enabled = false;

            Settings.Default.Reload();
        }
    }
}
