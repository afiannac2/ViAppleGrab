using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViToolkit.Logging;
using System.Xml;

namespace XML_Test_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            XMLTrace.CreateTraceFile("trace.xml");

            XmlNode child = XMLTrace.AppendElement("TestInstance", "");

            XmlNode subchild = XMLTrace.AppendSubchild(child, "Start", "");
            XMLTrace.AddAttributes(subchild, new Dictionary<string, string> { { "Time", DateTime.Now.ToString() } });

            subchild = XMLTrace.AppendSubchild(child, "Notes", "");
            XMLTrace.AddText(subchild, "This is some data...");

            subchild = XMLTrace.AppendSubchild(child, "End", "");
            XMLTrace.AddAttributes(subchild, new Dictionary<string, string> { { "Time", DateTime.Now.ToString() } });

            XMLTrace.Save();
        }
    }
}
