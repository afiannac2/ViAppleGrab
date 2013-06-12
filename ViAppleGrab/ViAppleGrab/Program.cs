using System;
using ViAppleGrab.Properties;
using ViToolkit.Logging;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using ViAppleGrab.Windows_Forms;

namespace ViAppleGrab
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //If they have not been created yet, create the mandatory directories
            if(!Directory.Exists("DebugLogs"))
                Directory.CreateDirectory("DebugLogs");

            if(!Directory.Exists("Results"))
                Directory.CreateDirectory("Results");
            
            //Create the debug log file
            string filename = System.DateTime.Now.ToFileTimeUtc().ToString("X");
            TextWriter writer = new StreamWriter(@"DebugLogs\" + filename + ".log");
            Debug.Listeners.Add(new TextWriterTraceListener(writer));
            Debug.AutoFlush = true;
            Debug.WriteLine("[Start Time] - " + System.DateTime.Now.ToString());

            InitMenu form = new InitMenu();
            form.ShowDialog();

            //Warn the user to setup the playstation Move.Me server before continuing
            //string message = "Please ensure that the computer is networked to the "
            //+ "playstation and the Move.Me server game is running on the "
            //+ "playstation with both controllers active \n\n"
            //+ "Press the OK button when this is complete; otherwise, press "
            //+ "Cancel to close this game.";

            //DialogResult res = MessageBox.Show(message, "Warning!", MessageBoxButtons.OKCancel);

            //if (res == DialogResult.OK)
            //{
            //    using (ViAppleGrabGame game = new ViAppleGrabGame())
            //    {
            //        DateTime now = DateTime.Now;

            //        try
            //        {
            //            //Create the results trace file
            //            string fname = now.Month.ToString("D2") + now.Day.ToString("D2") + now.ToString("yy");

            //            string[] files = Directory.GetFiles("Results", fname + "_*.*", SearchOption.AllDirectories);

            //            fname += "_" + files.Length.ToString() + ".xml";

            //            XMLTrace.CreateTraceFile(fname);

            //            game.Run();
            //        }
            //        catch (Exception e)
            //        {
            //            ((ViAppleGrabInput)game.Components[0]).Controllers.StopRumbles();

            //            Console.WriteLine(e.Message);

            //            if (e.InnerException != null)
            //                Console.WriteLine("Inner Exception: " + e.InnerException.Message);

            //            throw new Exception("Something is screwed up...!");
            //        }
            //        finally
            //        {
            //            XMLTrace.Save();

            //            if(Settings.Default.DISPLAY_RESULTS_AT_END)
            //                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            //                {
            //                    FileName = Directory.GetCurrentDirectory() + @"\Results",
            //                    UseShellExecute = true,
            //                    Verb = "open"
            //                });
            //        }
            //    }
            //}
        }
    }
#endif
}

