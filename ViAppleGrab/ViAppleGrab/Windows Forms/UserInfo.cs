using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ViToolkit.Logging;

namespace ViAppleGrab
{
    public enum UserInfoMode
    {
        Edit = 0,
        New = 1
    }

    public enum Months
    {
        Jan = 0,
        Feb = 1,
        Mar = 2,
        Apr = 3,
        May = 4,
        Jun = 5,
        Jul = 6,
        Aug = 7,
        Sep = 8,
        Oct = 9,
        Nov = 10,
        Dec = 11
    }

    public partial class UserInfo : Form
    {
        XmlDocument userDoc = new XmlDocument();
        private bool IsLeapYear = false;
        private int _userID;
        private UserInfoMode mode;
        private UserSelection parent;
        
        public UserInfo(UserInfoMode m, int ID, Form p)
        {
            InitializeComponent();

            parent = (UserSelection)p;
            mode = m;

            if (mode == UserInfoMode.Edit)
            {
                this.Text = "User Information - Edit Mode";
                btnSubmit.Text = "Submit Changes";
            }
            else //Edit the user info
            {
                btnSubmit.Text = "Submit New User";
            }            

            int year = DateTime.Now.Year;
            int centuryAgo = year - 60;

            for (int i = year; i > centuryAgo; i--)
            {
                cmbYear.Items.Add(i.ToString());
            }

            _userID = ID;

            userDoc.Load("Users.xml");

            if (mode == UserInfoMode.Edit)
            {
                XmlNode userNode = userDoc.SelectSingleNode("//User[@ID='" + ID.ToString() + "']");

                tbFirstName.Text = userNode.SelectSingleNode("FirstName").InnerText;
                tbLastName.Text = userNode.SelectSingleNode("LastName").InnerText;

                string temp = userNode.SelectSingleNode("DateOfBirth").InnerText;
                DateTime dob = Convert.ToDateTime(temp);
                cmbYear.SelectedIndex = DateTime.Now.Year - dob.Year;
                cmbMonth.SelectedIndex = dob.Month - 1;
                cmbDay.SelectedIndex = dob.Day - 1;

                switch(userNode.SelectSingleNode("Disability").InnerText)
                {
                    case "None":
                        cmbDisability.SelectedIndex = 0;
                        break;

                    case "Legally Blind":
                        cmbDisability.SelectedIndex = 1;
                        break;

                    case "Completely Blind":
                        cmbDisability.SelectedIndex = 2;
                        break;
                }

                switch(userNode.SelectSingleNode("TestGroup").InnerText)
                {
                    case "A":
                        cmbTestGroup.SelectedIndex = 0;
                        break;

                    case "B":
                        cmbTestGroup.SelectedIndex = 1;
                        break;
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbFirstName.Text != ""
                && tbLastName.Text != ""
                && cmbDay.SelectedIndex != (-1)
                && cmbMonth.SelectedIndex != (-1)
                && cmbYear.SelectedIndex != (-1)
                && cmbDisability.SelectedIndex != (-1))
            {
                if (mode == UserInfoMode.New)
                {
                    XmlNode child = userDoc.CreateNode(XmlNodeType.Element, "User", "");

                    XmlAttribute at = userDoc.CreateAttribute("ID");
                    at.Value = _userID.ToString();
                    child.Attributes.Append(at);

                    at = userDoc.CreateAttribute("DateCreated");
                    at.Value = DateTime.Now.ToLongDateString();
                    child.Attributes.Append(at);

                    XmlNode subChild = userDoc.CreateNode(XmlNodeType.Element, "FirstName", "");
                    subChild.InnerText = tbFirstName.Text;
                    child.AppendChild(subChild);

                    subChild = userDoc.CreateNode(XmlNodeType.Element, "LastName", "");
                    subChild.InnerText = tbLastName.Text;
                    child.AppendChild(subChild);

                    subChild = userDoc.CreateNode(XmlNodeType.Element, "DateOfBirth", "");
                    subChild.InnerText = (new DateTime(Convert.ToInt32(cmbYear.SelectedItem.ToString()),
                                                cmbMonth.SelectedIndex + 1,
                                                cmbDay.SelectedIndex + 1)).ToLongDateString();
                    child.AppendChild(subChild);

                    subChild = userDoc.CreateNode(XmlNodeType.Element, "Disability", "");
                    subChild.InnerText = cmbDisability.SelectedItem.ToString();
                    child.AppendChild(subChild);

                    subChild = userDoc.CreateNode(XmlNodeType.Element, "TestGroup", "");

                    string group;
                    if (cmbTestGroup.SelectedItem.ToString() == "Group A")
                        group = "A";
                    else
                        group = "B";

                    subChild.InnerText = group;
                    child.AppendChild(subChild);

                    userDoc.DocumentElement.AppendChild(child);
                }
                else
                {
                    XmlNode userNode = userDoc.SelectSingleNode("//User[@ID='" + _userID.ToString() + "']");

                    userNode.SelectSingleNode("FirstName").InnerText = tbFirstName.Text;
                    userNode.SelectSingleNode("LastName").InnerText = tbLastName.Text;

                    userNode.SelectSingleNode("DateOfBirth").InnerText = (new DateTime(Convert.ToInt32(cmbYear.SelectedItem.ToString()),
                                                cmbMonth.SelectedIndex + 1,
                                                cmbDay.SelectedIndex + 1)).ToLongDateString();

                    userNode.SelectSingleNode("Disability").InnerText = cmbDisability.SelectedItem.ToString();

                    string group;
                    if (cmbTestGroup.SelectedItem.ToString() == "Group A")
                        group = "A";
                    else
                        group = "B";

                    userNode.SelectSingleNode("TestGroup").InnerText = group;
                }

                XmlWriter writer = new XmlTextWriter("Users.xml", Encoding.UTF8);
                userDoc.Save(writer);
                writer.Close();

                parent._refreshUsers();
                parent.SetSelectedUser(_userID);

                this.Close();
            }
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            //_setDays(IsLeapYear);

            //if (cmbYear.SelectedIndex != (-1) && cmbDay.SelectedIndex != (-1))
            //{
            //    _updateAge();
            //}
            //else
            //    lblAge.Text = "";
        }

        private void _setDays(bool leapYear)
        {
            //int days = 31;

            ////switch ((Months)cmbMonth.SelectedIndex)
            ////{
            ////    case Months.Feb:
            ////        days = (leapYear) ? 29 : 28;
            ////        break;

            ////    case Months.Sep:
            ////    case Months.Jun:
            ////    case Months.Apr:
            ////    case Months.Nov:
            ////        days = 30;
            ////        break;

            ////    default:
            ////        days = 31;
            ////        break;
            ////}

            //cmbDay.Items.Clear();
            //for (int i = 0; i < days; i++)
            //{
            //    cmbDay.Items.Add((i + 1).ToString());
            //}
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int year = Convert.ToInt32(cmbYear.SelectedItem.ToString());
            //if ((year % 4 == 0 && year % 100 == 0 && year % 400 == 0) || (year % 4 == 0 && year % 100 != 0))
            //{
            //    IsLeapYear = true;
            //    _setDays(IsLeapYear);
            //}
            //else
            //{
            //    IsLeapYear = false;
            //    _setDays(IsLeapYear);
            //}

            //if (cmbMonth.SelectedIndex != (-1) && cmbDay.SelectedIndex != (-1))
            //{
            //    _updateAge();
            //}
            //else
            //    lblAge.Text = "";
        }

        private void _updateAge()
        {
            DateTime dob = new DateTime(Convert.ToInt32(cmbYear.SelectedItem.ToString()),
                                            cmbMonth.SelectedIndex + 1,
                                            cmbDay.SelectedIndex + 1);

            lblAge.Text = Math.Round(((DateTime.Now - dob).TotalDays / 365), 1).ToString() + " years old";
        }

        private void cmbDay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
