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

            //Update the IP Address and store it, if need be
            VerifyIP vIP = new VerifyIP();
            vIP.ShowDialog();

            //Show the main user form
            InitMenu form = new InitMenu();
            form.ShowDialog();
        }
    }
#endif
}

