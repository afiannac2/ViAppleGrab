using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ViToolkit.Logging
{
    public class Log
    {
        TextWriter writer;

        public Log()
        {
            open();
            writer.WriteLine("Log opened at " + DateTime.Now.ToString());
            writer.Flush();
        }

        ~Log()
        {
            writer.Flush();
            writer.Close();
        }

        private void open()
        {
            string filename = DateTime.Now.ToString();
            filename = filename.Replace("/", "");
            filename = filename.Replace(" ", "");
            filename = filename.Replace(":", "");
            filename = filename + ".log";
            writer = new StreamWriter(filename);
        }

        private void close()
        {
            writer.Close();
        }

        public void LogEvent(string EventName, string EventMessage)
        {
            string output = EventName.ToUpper() + " : [" + DateTime.Now.ToString() + "] : " + EventMessage;

            writer.WriteLine(output);
            writer.Flush();
        }

        public static List<string> GatherByEventName(string Path, string EventName)
        {
            List<string> results = new List<string>();
            StreamReader sr;

            //If the file exists, open it to read, if not, return an empty list
            if(File.Exists(Path))
                sr = new StreamReader(Path);
            else
                return results;

            string text = sr.ReadLine();

            while (text != null)
            {
                if (EventName.ToUpper() == text.Substring(0, text.IndexOf(" ") + 1))
                    results.Add(text); 
            }

            return results;
        }
    }
}
