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

namespace ViAppleGrab.Windows_Forms
{
    public partial class InitMenu : Form
    {
        public InitMenu()
        {
            InitializeComponent();
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
                Thread t = new Thread(new ThreadStart(() => {            
                    
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

                            throw new Exception("Something is screwed up...!");
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

                btnPlay.Text = "Close and Reopen the Program to Play Again";
                btnPlay.Enabled = false;
            }
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
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
