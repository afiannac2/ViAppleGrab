using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace ViToolkit.Logging
{
    public static class XMLTrace
    {
        private static string filename;

        private static XmlDocument traceDoc = new XmlDocument();

        public static void Load(string f)
        {          
            traceDoc.Load(f);
        }
        
        public static void CreateTraceFile(string f)
        {
            filename = @"Results\" + f;
            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");

            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                // Write XML data.
                writer.WriteStartElement("TraceData");
                writer.WriteStartAttribute("CreatedDate");
                writer.WriteValue(DateTime.Now);
                writer.WriteEndAttribute();
                writer.WriteEndElement();
                writer.Flush();
            }

            traceDoc.Load(filename);
        }

        public static XmlNode AppendElement(string name, string namespaceURI)
        {
            XmlNode child = traceDoc.CreateNode(XmlNodeType.Element, name, namespaceURI);

            traceDoc.DocumentElement.AppendChild(child);

            return child;
        }

        public static XmlNode AppendSubchild(XmlNode child, string name, string namespaceURI)
        {
            XmlNode subchild = traceDoc.CreateNode(XmlNodeType.Element, name, namespaceURI);
            child.AppendChild(subchild);

            return subchild;
        }

        public static void AddAttributes(XmlNode node, Dictionary<string, string> d)
        {
            XmlAttribute at;

            foreach (KeyValuePair<string, string> kvp in d)
            {
                at = traceDoc.CreateAttribute(kvp.Key);
                at.Value = kvp.Value;
                node.Attributes.Append(at);
            }
        }

        public static void AddText(XmlNode node, string text)
        {
            node.InnerText = text;
        }

        public static void Save()
        {
            XmlNodeList targetNodes = traceDoc.SelectNodes("//Targets");

            foreach (XmlNode node in targetNodes)
            {
                XmlNodeList innerNodes = node.SelectNodes("TargetData[@Controller='']");

                foreach (XmlNode n in innerNodes)
                {
                    node.RemoveChild(n);
                }
            }

            XmlWriter writer = new XmlTextWriter(filename, Encoding.UTF8);
            traceDoc.Save(writer);
            writer.Close();
        }

        public static void AppendForeignNodeToDocElem(XmlNode node)
        {
            if (node != null)
            {
                XmlNode n = traceDoc.ImportNode(node, true);
                traceDoc.DocumentElement.AppendChild(n);
            }
        }

        public static XmlNode FindTargetDataNode(int targetID)
        {
            XmlNode n = FindLastTargetNode();
            return n.SelectSingleNode("TargetData[@ID='" + targetID.ToString() + "']");
            //return traceDoc.SelectSingleNode("//TargetData[@ID='" + targetID.ToString() + "']");
        }

        public static XmlNode FindLastTargetNode()
        {
            XmlNodeList l = traceDoc.SelectNodes("//Targets");
            int i = l.Count;
            return l[i - 1];
        }

        public static XmlNode FindLastGameInfoNode()
        {
            XmlNodeList l = traceDoc.SelectNodes("//GameInfo");
            int i = l.Count;
            return l[i - 1];
        }

        public static XmlNode FindUserInfoNode(string UserInfoFile, int ID)
        {
            XmlDocument users = new XmlDocument();
            users.Load(UserInfoFile);

            XmlNode userNode = users.SelectSingleNode("//User[@ID='" + ID.ToString() + "']");

            users = null;

            return userNode;
        }
    }
}
