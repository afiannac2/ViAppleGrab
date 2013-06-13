using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace ViAppleGrab.Windows_Forms
{
    public partial class EditUsers : Form
    {
        XDocument users;
        const string usersfile = "Users.xml";
        DateTime DOB;
        int CurrentID;
        bool ExitPrompt;

        public EditUsers()
        {
            InitializeComponent();

            if (!File.Exists(usersfile))
            {
                users = new XDocument(new XElement("Users"));
                users.Save(usersfile);
            }

            RefreshXDoc();
            CurrentID = -1;
            ExitPrompt = true;
        }

        private void RefreshXDoc()
        {
            users = XDocument.Load(usersfile);
            RefreshUserForm();
        }

        private void RefreshUserForm()
        {
            var q = users.Descendants().Elements("User").Select(user => new
                {
                    ID = user.Attribute("ID").Value,
                    Name = user.Element("LastName").Value + ", " + user.Element("FirstName").Value,
                    FirstName = user.Element("FirstName").Value,
                    LastName = user.Element("LastName").Value,
                    Gender = user.Element("Gender").Value,
                    Date = user.Element("DateOfBirth").Value,
                    Disability = user.Element("Disability").Value,
                    TestGroup = user.Element("TestGroup").Value,
                    Study = user.Element("Study").Value
                });

            lbUsers.DataSource = q.ToList();
            lbUsers.DisplayMember = "Name";
            lbUsers.ValueMember = "ID";
        }

        private void SaveXDoc()
        {
            users.Save(usersfile);
            RefreshXDoc();
            ClearUserForm();
        }

        private void ClearUserForm()
        {
            //cmbStudy.Text = "";
            cmbStudy.SelectedIndex = -1;
            cmbGroup.Enabled = false;
            cmbGroup.Items.Clear();
            cmbGroup.Text = "First Select a Study...";
            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbBirthDate.Text = "mm/dd/yy";
            tbBirthDate.ForeColor = SystemColors.InactiveCaptionText;
            tbDisability.Text = "Describe the level of the user's disability...";
            tbDisability.ForeColor = SystemColors.InactiveCaptionText;
            rbtnFemale.Checked = false;
            rbtnMale.Checked = false;
            CurrentID = -1;
            lblStatus.Text = "No user is currently selected... Click a name in the list above to make a selection.";
            lblStatusName.Text = "";
        }

        private void clearAllUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Issue a warning before clearing out all of the users
            DialogResult result = MessageBox.Show("This will permanently delete all recorded users! This is not reversible! Are you sure you want to continue?", "Warning!", MessageBoxButtons.OKCancel);

            //Clear out all of the users
            if (result == DialogResult.OK)
            {
                var userSet = users.Descendants().Elements("User");

                if (userSet == null) return;

                foreach (var user in userSet.ToList())
                {
                    user.Remove();
                }

                SaveXDoc();

                MessageBox.Show("All user records deleted!");
            }
        }

        private void returnToMainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveXDoc();

            ExitPrompt = false;
            this.Close();
        }

        private void tbBirthDate_Enter(object sender, EventArgs e)
        {
            if (tbBirthDate.Text == "mm/dd/yy")
            {
                tbBirthDate.ForeColor = SystemColors.WindowText;
                tbBirthDate.Text = "";
            }
        }

        private void tbBirthDate_Leave(object sender, EventArgs e)
        {
            if (tbBirthDate.Text == "")
            {
                tbBirthDate.Text = "mm/dd/yy";
                tbBirthDate.ForeColor = SystemColors.InactiveCaptionText;
            }
            else
            {
                bool success = DateTime.TryParse(tbBirthDate.Text, out DOB);

                if (!success)
                {
                    tbBirthDate.ForeColor = System.Drawing.Color.Red;
                    lblBirthDate.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    tbBirthDate.ForeColor = SystemColors.WindowText;
                    lblBirthDate.ForeColor = SystemColors.WindowText;
                }
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            SaveXDoc();

            ExitPrompt = false;
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearUserForm();
        }

        private void cmbStudy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStudy.SelectedIndex != -1)
            {
                cmbGroup.Enabled = true;
                cmbGroup.Items.Clear();

                if (cmbStudy.SelectedItem.ToString() == "Camp Abilities Study")
                {
                    cmbGroup.Items.Add("Group A: Everyone");
                    cmbGroup.Enabled = false;
                }
                else
                {
                    cmbGroup.Enabled = true;
                    cmbGroup.Items.Add("Group A: Split");
                    cmbGroup.Items.Add("Group B: Conjunctional");
                }

                cmbGroup.SelectedIndex = 0;

                tbDisability.ForeColor = SystemColors.WindowText;
                tbBirthDate.ForeColor = SystemColors.WindowText;
            }
        }

        private void tbDisability_Enter(object sender, EventArgs e)
        {
            if (tbDisability.Text == "Describe the level of the user's disability...")
            {
                tbDisability.Text = "";
                tbDisability.ForeColor = SystemColors.WindowText;
            }
        }

        private void tbDisability_Leave(object sender, EventArgs e)
        {
            if (tbDisability.Text == "")
            {
                tbDisability.Text = "Describe the level of the user's disability...";
                tbDisability.ForeColor = SystemColors.InactiveCaptionText;
            }
        }

        private void lbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            var user = users.Descendants().Elements("User")
                .Where(u => u.Attribute("ID").Value == lbUsers.SelectedValue.ToString())
                .Select(u => new
                {
                    ID = u.Attribute("ID").Value,
                    FirstName = u.Element("FirstName").Value,
                    LastName = u.Element("LastName").Value,
                    Gender = u.Element("Gender").Value,
                    Date = u.Element("DateOfBirth").Value,
                    Disability = u.Element("Disability").Value,
                    TestGroup = u.Element("TestGroup").Value,
                    Study = u.Element("Study").Value
                })
                .FirstOrDefault();

            if (user == null) return;

            if (user.Study == "Camp Abilities Study")
            {
                cmbStudy.SelectedIndex = 0;
            }
            else
            {
                cmbStudy.SelectedIndex = 1;

                if (user.TestGroup == "A")
                    cmbGroup.SelectedIndex = 0;
                else
                    cmbGroup.SelectedIndex = 1;
            }

            tbFirstName.Text = user.FirstName;
            tbLastName.Text = user.LastName;
            tbBirthDate.Text = user.Date;
            tbDisability.Text = user.Disability;

            if (user.Gender == "M")
                rbtnMale.Checked = true;
            else
                rbtnFemale.Checked = true;

            CurrentID = int.Parse(user.ID);

            lblStatus.Text = "Current Selection: ";
            lblStatusName.Text = user.FirstName + " " + user.LastName;
        }

        private void btnAddUpdate_Click(object sender, EventArgs e)
        {
            if (CurrentID == -1) //Add New
            {
                var q = users.Descendants().FirstOrDefault();

                if (q == null)
                {
                    MessageBox.Show("There is an issue... Call Alex. EditUsers.cs: Line 244");
                    return;
                }

                XElement user = new XElement("User");
                user.Add(new XAttribute("ID", lbUsers.Items.Count + 1));
                user.Add(new XAttribute("DateCreated", DateTime.Now.ToLongDateString()));
                user.Add(new XElement("FirstName", tbFirstName.Text));
                user.Add(new XElement("LastName", tbLastName.Text));
                user.Add(new XElement("DateOfBirth", DOB.ToLongDateString()));
                user.Add(new XElement("Gender", (rbtnMale.Checked ? "M" : "F")));
                user.Add(new XElement("Disability", tbDisability.Text));
                user.Add(new XElement("Study", cmbStudy.SelectedItem.ToString()));

                if (cmbGroup.SelectedItem.ToString() == "Group A: Everyone"
                    || cmbGroup.SelectedItem.ToString() == "Group A: Split")
                    user.Add(new XElement("TestGroup", "A"));
                else
                    user.Add(new XElement("TestGroup", "B"));

                q.Add(user);

                SaveXDoc();

                MessageBox.Show("New User Added");
            }
            else //Update
            {
                var user = users.Descendants().Elements("User")
                    .Where(u => u.Attribute("ID").Value == CurrentID.ToString())
                    .FirstOrDefault();

                if (user == null)
                {
                    MessageBox.Show("There is an issue... Call Alex. EditUsers.cs: Line 244");
                    return;
                }

                user.Element("FirstName").Value = tbFirstName.Text;
                user.Element("LastName").Value = tbLastName.Text;
                user.Element("DateOfBirth").Value = DOB.ToLongDateString();
                user.Element("Gender").Value = (rbtnMale.Checked ? "M" : "F");
                user.Element("Disability").Value = tbDisability.Text;
                user.Element("Study").Value = cmbStudy.SelectedItem.ToString();

                if (cmbGroup.SelectedItem.ToString() == "Group A: Everyone"
                    || cmbGroup.SelectedItem.ToString() == "Group A: Split")
                    user.Element("TestGroup").Value =  "A";
                else
                    user.Element("TestGroup").Value = "B";

                SaveXDoc();

                MessageBox.Show("Updated User Record Successfully");
            }
        }

        private void EditUsers_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ExitPrompt)
            {
                DialogResult result = 
                    MessageBox.Show("Do you wish to save your changes before exiting this form?", 
                        "Warning! Unsaved Changes will be lost!", 
                        MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                    SaveXDoc();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CurrentID == -1)
            {
                MessageBox.Show("Please select a user before attempting to delete!");
                return;
            }

            DialogResult result = 
                MessageBox.Show("Are you sure you wish to delete all records for this user?", 
                    "Warning!", 
                    MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                var user = users.Descendants().Elements("User")
                    .Where(u => u.Attribute("ID").Value == CurrentID.ToString())
                    .FirstOrDefault();

                if (user == null)
                {
                    MessageBox.Show("There is an issue... Call Alex. EditUsers.cs: Line 244");
                    return;
                }

                user.Remove();
                SaveXDoc();
                MessageBox.Show("User record removed");
            }
        }

    }
}
