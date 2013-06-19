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
using ViToolkit.FileUtilities;
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

        string Stage1File = "";
        string Stage2File = "";
        string Stage3File = "";

        bool ShowResultsWarning = true;

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

                Stage1File = Directory.GetCurrentDirectory() + @"\Results\" + StartGameThread();

                btnPlay.Text = "Stage #1 Complete";
                btnPlay.Enabled = false;

                Stage1Complete = true;

                btnRedoWarmup.Visible = true;
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

                    Stage2File = Directory.GetCurrentDirectory() + @"\Results\" + StartGameThread();

                    btnPlay2.Text = "Stage #2 Complete";
                    btnPlay2.Enabled = false;
                    Stage2Complete = true;
                }
            }
            else
            {
                Settings.Default.STAGE = (int)StudyStages.Single;

                Stage2File = Directory.GetCurrentDirectory() + @"\Results\" + StartGameThread();

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

                    Stage3File = Directory.GetCurrentDirectory() + @"\Results\" + StartGameThread();

                    btnPlay3.Text = "Stage #3 Complete";
                    btnPlay3.Enabled = false;
                }
            }
            else
            {
                Settings.Default.STAGE = (int)StudyStages.Simultaneous;

                Stage3File = Directory.GetCurrentDirectory() + @"\Results\" + StartGameThread();

                btnPlay3.Text = "Stage #3 Complete";
                btnPlay3.Enabled = false;
            }
        }

        private string StartGameThread()
        {
            //Generate the output filename
            DateTime now = DateTime.Now;

            string fname = now.Month.ToString("D2") + now.Day.ToString("D2") + now.ToString("yy");

            string[] files = Directory.GetFiles("Results", fname + "_*.*", SearchOption.AllDirectories);

            fname += "_" + files.Length.ToString() + ".xml";

            //Start the game thread
            Thread t = new Thread(new ParameterizedThreadStart((object file) =>
            {
                using (ViAppleGrabGame game = new ViAppleGrabGame())
                {
                    try
                    {
                        XMLTrace.CreateTraceFile((string)file);

                        game.Run();

                        //Save any results data
                        XMLTrace.Save();

                        if (Settings.Default.DISPLAY_RESULTS_AT_END)
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                            {
                                FileName = Directory.GetCurrentDirectory() + @"\Results",
                                UseShellExecute = true,
                                Verb = "open"
                            });
                    }
                    catch (Exception ex)
                    {
                        //Turn off the controllers
                        ((ViAppleGrabInput)game.Components[0]).Controllers.StopRumbles();

                        //Save any results data
                        XMLTrace.Save();

                        if (Settings.Default.DISPLAY_RESULTS_AT_END)
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                            {
                                FileName = Directory.GetCurrentDirectory() + @"\Results",
                                UseShellExecute = true,
                                Verb = "open"
                            });

                        //Output the error information
                        Console.WriteLine(ex.Message);

                        if (ex.InnerException != null)
                            Console.WriteLine("Inner Exception: " + ex.InnerException.Message);

                        MessageBox.Show("A fatal error has occurred. Please restart the program.");
                    }
                }

            }));
            t.Start(fname);

            return fname;
        }

        private void btnViewResults_Click(object sender, EventArgs e)
        {
            if (ShowResultsWarning)
            {
                DialogResult res = MessageBox.Show(
                    "File Explorer will now open up and the results files for this "
                    + "user will automatically be selected. Please copy these files "
                    + "to a backup location for safe keeping! \n\n"
                    + "Would you like to view this warning message again in the future?",
                    "Save These Results Files!",
                    MessageBoxButtons.YesNoCancel);

                if (res == System.Windows.Forms.DialogResult.No)
                    ShowResultsWarning = false;
                else if (res == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }

            IEnumerable<string> paths = new List<string>();

            if (Stage1File != "")
                ((List<string>)paths).Add(Stage1File);

            if (Stage2File != "")
                ((List<string>)paths).Add(Stage2File);

            if (Stage3File != "")
                ((List<string>)paths).Add(Stage3File);

            //Either show the selected files, or show the results folder without any selections
            if (((List<string>)paths).Count > 0)
                ShowSelectedInExplorer.FilesOrFolders(paths);
            else
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
            DialogResult res = MessageBox.Show(
                "Before exiting the system, please make sure you have copied any "
                + "results files for the previous user to a backup location! \n\n"
                + "Are you sure you wish to close the program? \n\n"
                + "Press YES to close the program \n"
                + "Press NO to go back and save the results files",
                "Have you saved the results yet?",
                MessageBoxButtons.YesNo);

            if(res == System.Windows.Forms.DialogResult.Yes)
                this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show(
                "Before resetting the system, please make sure you have copied any "
                + "results files for the previous user to a backup location! \n\n"
                + "Are you sure you wish to reset the system? \n\n"
                + "Press YES to reset the system \n"
                + "Press NO to go back and save the results files",
                "Warning!", 
                MessageBoxButtons.YesNo);

            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                Stage1Complete = false;
                Stage2Complete = false;

                btnPlay.Enabled = true;
                btnPlay.Text = "2. Run Stage #1: Warmup";
                Stage1File = "";

                btnPlay2.Enabled = true;
                btnPlay2.Text = "3. Run Stage #2: Singles";
                Stage2File = "";

                btnPlay3.Enabled = true;
                btnPlay3.Text = "4. Run Stage #3: Simultaneous";
                Stage3File = "";

                lblCurrentUserLbl.Text = "No user is currently selected!";
                lblCurrentUser.Text = "";

                btnPlay.Enabled = false;
                btnPlay2.Enabled = false;
                btnPlay3.Enabled = false;

                Settings.Default.Reload();
            }
        }

        private void btnRedoWarmup_Click(object sender, EventArgs e)
        {
            btnPlay.Text = "2. Run Stage #1: Warmup";
            btnPlay.Enabled = true;

            Stage1Complete = false;

            btnRedoWarmup.Visible = false;
        }
    }
}
