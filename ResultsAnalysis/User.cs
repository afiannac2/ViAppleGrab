using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ResultsAnalysis
{
    class User
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Name
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
        public DateTime DOB { get; private set; }
        public int Age { get; private set; }
        public DateTime DateCreated { get; private set; }
        public string TestGroup { get; private set; }
        public string Disability { get; private set; }

        public User (XmlNode userNode)
        {
            FirstName = userNode.SelectSingleNode("FirstName").InnerText;
            LastName = userNode.SelectSingleNode("LastName").InnerText;
            DOB = DateTime.Parse(userNode.SelectSingleNode("DateOfBirth").InnerText);
            Age = (DateTime.Now.Date - DOB.Date).Days / 365;
            DateCreated = DateTime.Parse(userNode.Attributes["DateCreated"].Value);
            TestGroup = userNode.SelectSingleNode("TestGroup").InnerText;
            Disability = userNode.SelectSingleNode("Disability").InnerText;
        }

        public override string ToString()
        {
            return LastName + ", " + FirstName;
        }
    }
}
