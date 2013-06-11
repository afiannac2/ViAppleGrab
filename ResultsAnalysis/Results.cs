using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace ResultsAnalysis
{
    class Results
    {
        public FileInfo FileInformation { get; private set; }

        public User User { get; private set; }

        public Game[] Games { get; private set; }

        public int GameCount
        {
            get
            {
                return Games.Length;
            }
        }

        private XmlDocument xml = new XmlDocument();

        public Results(FileInfo f)
        {
            FileInformation = f;

            xml.Load(f.FullName);

            User = new User(xml.DocumentElement.SelectSingleNode("User"));

            XmlNodeList games = xml.DocumentElement.SelectNodes("GameInfo");
            XmlNodeList targets = xml.DocumentElement.SelectNodes("Targets");

            Games = new Game[games.Count];

            for (int i = 0; i < games.Count; i++)
            {
                Games[i] = new Game(games[i], targets[i]);
            }
        }
    }
}
