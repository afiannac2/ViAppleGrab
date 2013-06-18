namespace ViAppleGrab.Windows_Forms
{
    partial class EditUsers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbUsers = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.utilitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllUsersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.returnToMainMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.pnlGender = new System.Windows.Forms.Panel();
            this.rbtnFemale = new System.Windows.Forms.RadioButton();
            this.rbtnMale = new System.Windows.Forms.RadioButton();
            this.tbBirthDate = new System.Windows.Forms.TextBox();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.lblDisability = new System.Windows.Forms.Label();
            this.tbDisability = new System.Windows.Forms.TextBox();
            this.cmbStudy = new System.Windows.Forms.ComboBox();
            this.lblStudy = new System.Windows.Forms.Label();
            this.lblGroup = new System.Windows.Forms.Label();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.btnAddUpdate = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblStudyHeader = new System.Windows.Forms.Label();
            this.lblUserHeader = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusName = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblArmLength = new System.Windows.Forms.Label();
            this.lblHeight = new System.Windows.Forms.Label();
            this.lblArm = new System.Windows.Forms.Label();
            this.tbArmLength = new System.Windows.Forms.TextBox();
            this.tbHeight = new System.Windows.Forms.TextBox();
            this.rbtnRight = new System.Windows.Forms.RadioButton();
            this.rbtnLeft = new System.Windows.Forms.RadioButton();
            this.pnlArm = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.pnlGender.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnlArm.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbUsers
            // 
            this.lbUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUsers.FormattingEnabled = true;
            this.lbUsers.ItemHeight = 16;
            this.lbUsers.Location = new System.Drawing.Point(12, 32);
            this.lbUsers.Name = "lbUsers";
            this.lbUsers.Size = new System.Drawing.Size(260, 580);
            this.lbUsers.TabIndex = 0;
            this.lbUsers.SelectedIndexChanged += new System.EventHandler(this.lbUsers_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(651, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.utilitiesToolStripMenuItem,
            this.returnToMainMenuToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // utilitiesToolStripMenuItem
            // 
            this.utilitiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearAllUsersToolStripMenuItem});
            this.utilitiesToolStripMenuItem.Name = "utilitiesToolStripMenuItem";
            this.utilitiesToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.utilitiesToolStripMenuItem.Text = "Utilities";
            // 
            // clearAllUsersToolStripMenuItem
            // 
            this.clearAllUsersToolStripMenuItem.Name = "clearAllUsersToolStripMenuItem";
            this.clearAllUsersToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.clearAllUsersToolStripMenuItem.Text = "Clear All Users";
            this.clearAllUsersToolStripMenuItem.Click += new System.EventHandler(this.clearAllUsersToolStripMenuItem_Click);
            // 
            // returnToMainMenuToolStripMenuItem
            // 
            this.returnToMainMenuToolStripMenuItem.Name = "returnToMainMenuToolStripMenuItem";
            this.returnToMainMenuToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.returnToMainMenuToolStripMenuItem.Text = "Return to Main Menu";
            this.returnToMainMenuToolStripMenuItem.Click += new System.EventHandler(this.returnToMainMenuToolStripMenuItem_Click);
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFirstName.Location = new System.Drawing.Point(278, 226);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(76, 16);
            this.lblFirstName.TabIndex = 2;
            this.lblFirstName.Text = "First Name:";
            // 
            // tbFirstName
            // 
            this.tbFirstName.Location = new System.Drawing.Point(410, 225);
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(229, 20);
            this.tbFirstName.TabIndex = 3;
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastName.Location = new System.Drawing.Point(278, 260);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(76, 16);
            this.lblLastName.TabIndex = 4;
            this.lblLastName.Text = "Last Name:";
            // 
            // tbLastName
            // 
            this.tbLastName.Location = new System.Drawing.Point(410, 259);
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(229, 20);
            this.tbLastName.TabIndex = 4;
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGender.Location = new System.Drawing.Point(278, 295);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(56, 16);
            this.lblGender.TabIndex = 6;
            this.lblGender.Text = "Gender:";
            // 
            // pnlGender
            // 
            this.pnlGender.Controls.Add(this.rbtnFemale);
            this.pnlGender.Controls.Add(this.rbtnMale);
            this.pnlGender.Location = new System.Drawing.Point(410, 285);
            this.pnlGender.Name = "pnlGender";
            this.pnlGender.Size = new System.Drawing.Size(229, 34);
            this.pnlGender.TabIndex = 5;
            // 
            // rbtnFemale
            // 
            this.rbtnFemale.AutoSize = true;
            this.rbtnFemale.Location = new System.Drawing.Point(57, 10);
            this.rbtnFemale.Name = "rbtnFemale";
            this.rbtnFemale.Size = new System.Drawing.Size(59, 17);
            this.rbtnFemale.TabIndex = 1;
            this.rbtnFemale.TabStop = true;
            this.rbtnFemale.Text = "Female";
            this.rbtnFemale.UseVisualStyleBackColor = true;
            // 
            // rbtnMale
            // 
            this.rbtnMale.AutoSize = true;
            this.rbtnMale.Location = new System.Drawing.Point(3, 10);
            this.rbtnMale.Name = "rbtnMale";
            this.rbtnMale.Size = new System.Drawing.Size(48, 17);
            this.rbtnMale.TabIndex = 0;
            this.rbtnMale.TabStop = true;
            this.rbtnMale.Text = "Male";
            this.rbtnMale.UseVisualStyleBackColor = true;
            // 
            // tbBirthDate
            // 
            this.tbBirthDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBirthDate.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.tbBirthDate.Location = new System.Drawing.Point(410, 329);
            this.tbBirthDate.Name = "tbBirthDate";
            this.tbBirthDate.Size = new System.Drawing.Size(229, 20);
            this.tbBirthDate.TabIndex = 6;
            this.tbBirthDate.Text = "mm/dd/yy";
            this.tbBirthDate.Enter += new System.EventHandler(this.tbBirthDate_Enter);
            this.tbBirthDate.Leave += new System.EventHandler(this.tbBirthDate_Leave);
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.AutoSize = true;
            this.lblBirthDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBirthDate.Location = new System.Drawing.Point(278, 330);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(83, 16);
            this.lblBirthDate.TabIndex = 9;
            this.lblBirthDate.Text = "Date of Birth:";
            // 
            // lblDisability
            // 
            this.lblDisability.AutoSize = true;
            this.lblDisability.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisability.Location = new System.Drawing.Point(279, 478);
            this.lblDisability.Name = "lblDisability";
            this.lblDisability.Size = new System.Drawing.Size(77, 16);
            this.lblDisability.TabIndex = 10;
            this.lblDisability.Text = "Disabilities:";
            // 
            // tbDisability
            // 
            this.tbDisability.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.tbDisability.Location = new System.Drawing.Point(411, 477);
            this.tbDisability.Multiline = true;
            this.tbDisability.Name = "tbDisability";
            this.tbDisability.Size = new System.Drawing.Size(229, 100);
            this.tbDisability.TabIndex = 10;
            this.tbDisability.Text = "Describe the level of the user\'s disabilities, both visual and physical...";
            this.tbDisability.Enter += new System.EventHandler(this.tbDisability_Enter);
            this.tbDisability.Leave += new System.EventHandler(this.tbDisability_Leave);
            // 
            // cmbStudy
            // 
            this.cmbStudy.FormattingEnabled = true;
            this.cmbStudy.Items.AddRange(new object[] {
            "Camp Abilities Study",
            "Original GI 2013 Study"});
            this.cmbStudy.Location = new System.Drawing.Point(410, 103);
            this.cmbStudy.Name = "cmbStudy";
            this.cmbStudy.Size = new System.Drawing.Size(229, 21);
            this.cmbStudy.TabIndex = 1;
            this.cmbStudy.SelectedIndexChanged += new System.EventHandler(this.cmbStudy_SelectedIndexChanged);
            // 
            // lblStudy
            // 
            this.lblStudy.AutoSize = true;
            this.lblStudy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStudy.Location = new System.Drawing.Point(278, 104);
            this.lblStudy.Name = "lblStudy";
            this.lblStudy.Size = new System.Drawing.Size(97, 16);
            this.lblStudy.TabIndex = 13;
            this.lblStudy.Text = "Select a Study:";
            // 
            // lblGroup
            // 
            this.lblGroup.AutoSize = true;
            this.lblGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGroup.Location = new System.Drawing.Point(278, 133);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(100, 16);
            this.lblGroup.TabIndex = 14;
            this.lblGroup.Text = "Select a Group:";
            // 
            // cmbGroup
            // 
            this.cmbGroup.Enabled = false;
            this.cmbGroup.FormattingEnabled = true;
            this.cmbGroup.Location = new System.Drawing.Point(410, 132);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Size = new System.Drawing.Size(229, 21);
            this.cmbGroup.TabIndex = 2;
            this.cmbGroup.Text = "First Select a Study...";
            // 
            // btnAddUpdate
            // 
            this.btnAddUpdate.Location = new System.Drawing.Point(281, 589);
            this.btnAddUpdate.Name = "btnAddUpdate";
            this.btnAddUpdate.Size = new System.Drawing.Size(180, 23);
            this.btnAddUpdate.TabIndex = 11;
            this.btnAddUpdate.Text = "Add/Update";
            this.btnAddUpdate.UseVisualStyleBackColor = true;
            this.btnAddUpdate.Click += new System.EventHandler(this.btnAddUpdate_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(281, 32);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(180, 23);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "New User (Clear Form)";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(467, 589);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(172, 23);
            this.btnDone.TabIndex = 13;
            this.btnDone.Text = "Save Selection and Return";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(467, 32);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(172, 23);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete User Record";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblStudyHeader
            // 
            this.lblStudyHeader.AutoSize = true;
            this.lblStudyHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStudyHeader.Location = new System.Drawing.Point(278, 68);
            this.lblStudyHeader.Name = "lblStudyHeader";
            this.lblStudyHeader.Size = new System.Drawing.Size(152, 20);
            this.lblStudyHeader.TabIndex = 20;
            this.lblStudyHeader.Text = "Study Information";
            // 
            // lblUserHeader
            // 
            this.lblUserHeader.AutoSize = true;
            this.lblUserHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserHeader.Location = new System.Drawing.Point(278, 187);
            this.lblUserHeader.Name = "lblUserHeader";
            this.lblUserHeader.Size = new System.Drawing.Size(176, 20);
            this.lblUserHeader.TabIndex = 21;
            this.lblUserHeader.Text = "Personal Information";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblStatusName});
            this.statusStrip1.Location = new System.Drawing.Point(0, 619);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(651, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 22;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(429, 17);
            this.lblStatus.Text = "No user is currently selected... Click a name in the list above to make a selecti" +
    "on.";
            // 
            // lblStatusName
            // 
            this.lblStatusName.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblStatusName.Name = "lblStatusName";
            this.lblStatusName.Size = new System.Drawing.Size(0, 17);
            // 
            // lblArmLength
            // 
            this.lblArmLength.AutoSize = true;
            this.lblArmLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArmLength.Location = new System.Drawing.Point(279, 367);
            this.lblArmLength.Name = "lblArmLength";
            this.lblArmLength.Size = new System.Drawing.Size(131, 16);
            this.lblArmLength.TabIndex = 23;
            this.lblArmLength.Text = "Arm Length (inches) :";
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeight.Location = new System.Drawing.Point(279, 405);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(103, 16);
            this.lblHeight.TabIndex = 24;
            this.lblHeight.Text = "Height (inches) :";
            // 
            // lblArm
            // 
            this.lblArm.AutoSize = true;
            this.lblArm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArm.Location = new System.Drawing.Point(279, 441);
            this.lblArm.Name = "lblArm";
            this.lblArm.Size = new System.Drawing.Size(95, 16);
            this.lblArm.TabIndex = 25;
            this.lblArm.Text = "Dominant Arm:";
            // 
            // tbArmLength
            // 
            this.tbArmLength.Location = new System.Drawing.Point(410, 368);
            this.tbArmLength.Name = "tbArmLength";
            this.tbArmLength.Size = new System.Drawing.Size(229, 20);
            this.tbArmLength.TabIndex = 7;
            // 
            // tbHeight
            // 
            this.tbHeight.Location = new System.Drawing.Point(410, 405);
            this.tbHeight.Name = "tbHeight";
            this.tbHeight.Size = new System.Drawing.Size(229, 20);
            this.tbHeight.TabIndex = 8;
            // 
            // rbtnRight
            // 
            this.rbtnRight.AutoSize = true;
            this.rbtnRight.Location = new System.Drawing.Point(6, 6);
            this.rbtnRight.Name = "rbtnRight";
            this.rbtnRight.Size = new System.Drawing.Size(50, 17);
            this.rbtnRight.TabIndex = 0;
            this.rbtnRight.TabStop = true;
            this.rbtnRight.Text = "Right";
            this.rbtnRight.UseVisualStyleBackColor = true;
            // 
            // rbtnLeft
            // 
            this.rbtnLeft.AutoSize = true;
            this.rbtnLeft.Location = new System.Drawing.Point(62, 5);
            this.rbtnLeft.Name = "rbtnLeft";
            this.rbtnLeft.Size = new System.Drawing.Size(43, 17);
            this.rbtnLeft.TabIndex = 1;
            this.rbtnLeft.TabStop = true;
            this.rbtnLeft.Text = "Left";
            this.rbtnLeft.UseVisualStyleBackColor = true;
            // 
            // pnlArm
            // 
            this.pnlArm.Controls.Add(this.rbtnLeft);
            this.pnlArm.Controls.Add(this.rbtnRight);
            this.pnlArm.Location = new System.Drawing.Point(407, 435);
            this.pnlArm.Name = "pnlArm";
            this.pnlArm.Size = new System.Drawing.Size(118, 31);
            this.pnlArm.TabIndex = 9;
            // 
            // EditUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 641);
            this.Controls.Add(this.pnlArm);
            this.Controls.Add(this.tbHeight);
            this.Controls.Add(this.tbArmLength);
            this.Controls.Add(this.lblArm);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.lblArmLength);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblUserHeader);
            this.Controls.Add(this.lblStudyHeader);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnAddUpdate);
            this.Controls.Add(this.cmbGroup);
            this.Controls.Add(this.lblGroup);
            this.Controls.Add(this.lblStudy);
            this.Controls.Add(this.cmbStudy);
            this.Controls.Add(this.tbDisability);
            this.Controls.Add(this.lblDisability);
            this.Controls.Add(this.lblBirthDate);
            this.Controls.Add(this.tbBirthDate);
            this.Controls.Add(this.pnlGender);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.tbLastName);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.tbFirstName);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.lbUsers);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EditUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EditUsers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditUsers_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlGender.ResumeLayout(false);
            this.pnlGender.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlArm.ResumeLayout(false);
            this.pnlArm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbUsers;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem utilitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllUsersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem returnToMainMenuToolStripMenuItem;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox tbFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox tbLastName;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Panel pnlGender;
        private System.Windows.Forms.RadioButton rbtnFemale;
        private System.Windows.Forms.RadioButton rbtnMale;
        private System.Windows.Forms.TextBox tbBirthDate;
        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.Label lblDisability;
        private System.Windows.Forms.TextBox tbDisability;
        private System.Windows.Forms.ComboBox cmbStudy;
        private System.Windows.Forms.Label lblStudy;
        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.ComboBox cmbGroup;
        private System.Windows.Forms.Button btnAddUpdate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblStudyHeader;
        private System.Windows.Forms.Label lblUserHeader;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusName;
        private System.Windows.Forms.Label lblArmLength;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label lblArm;
        private System.Windows.Forms.TextBox tbArmLength;
        private System.Windows.Forms.TextBox tbHeight;
        private System.Windows.Forms.RadioButton rbtnRight;
        private System.Windows.Forms.RadioButton rbtnLeft;
        private System.Windows.Forms.Panel pnlArm;
    }
}