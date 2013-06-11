namespace ViAppleGrab
{
    partial class UserSelection
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
            this.btnStartGame = new System.Windows.Forms.Button();
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.btnNewUser = new System.Windows.Forms.Button();
            this.btnEditUser = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbQuickCalibration = new System.Windows.Forms.CheckBox();
            this.lblGameType = new System.Windows.Forms.Label();
            this.cmbGameType = new System.Windows.Forms.ComboBox();
            this.lblControlType = new System.Windows.Forms.Label();
            this.cmbControlType = new System.Windows.Forms.ComboBox();
            this.lblQuickCalibration = new System.Windows.Forms.Label();
            this.lblYAxisController = new System.Windows.Forms.Label();
            this.cmbYAxisController = new System.Windows.Forms.ComboBox();
            this.cbShowCamera = new System.Windows.Forms.CheckBox();
            this.lblShowCamera = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTestGroup = new System.Windows.Forms.ComboBox();
            this.lblTestGroup = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.purgeResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.purgeDebugLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTypeOfPlay = new System.Windows.Forms.Label();
            this.cmbTypeOfPlay = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartGame
            // 
            this.btnStartGame.Location = new System.Drawing.Point(13, 408);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(220, 23);
            this.btnStartGame.TabIndex = 8;
            this.btnStartGame.Text = "Start Game";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
            // 
            // cmbUsers
            // 
            this.cmbUsers.FormattingEnabled = true;
            this.cmbUsers.Location = new System.Drawing.Point(13, 62);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(220, 21);
            this.cmbUsers.TabIndex = 9;
            this.cmbUsers.SelectedIndexChanged += new System.EventHandler(this.cmbUsers_SelectedIndexChanged);
            // 
            // btnNewUser
            // 
            this.btnNewUser.Location = new System.Drawing.Point(13, 90);
            this.btnNewUser.Name = "btnNewUser";
            this.btnNewUser.Size = new System.Drawing.Size(107, 23);
            this.btnNewUser.TabIndex = 10;
            this.btnNewUser.Text = "New User";
            this.btnNewUser.UseVisualStyleBackColor = true;
            this.btnNewUser.Click += new System.EventHandler(this.btnNewUser_Click);
            // 
            // btnEditUser
            // 
            this.btnEditUser.Enabled = false;
            this.btnEditUser.Location = new System.Drawing.Point(126, 90);
            this.btnEditUser.Name = "btnEditUser";
            this.btnEditUser.Size = new System.Drawing.Size(107, 23);
            this.btnEditUser.TabIndex = 11;
            this.btnEditUser.Text = "Edit User";
            this.btnEditUser.UseVisualStyleBackColor = true;
            this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(43, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Configuration Settings";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbQuickCalibration
            // 
            this.cbQuickCalibration.AutoSize = true;
            this.cbQuickCalibration.Checked = true;
            this.cbQuickCalibration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbQuickCalibration.Location = new System.Drawing.Point(109, 202);
            this.cbQuickCalibration.Name = "cbQuickCalibration";
            this.cbQuickCalibration.Size = new System.Drawing.Size(15, 14);
            this.cbQuickCalibration.TabIndex = 13;
            this.cbQuickCalibration.UseVisualStyleBackColor = true;
            this.cbQuickCalibration.CheckedChanged += new System.EventHandler(this.cbQuickCalibration_CheckedChanged);
            // 
            // lblGameType
            // 
            this.lblGameType.AutoSize = true;
            this.lblGameType.Location = new System.Drawing.Point(13, 253);
            this.lblGameType.Name = "lblGameType";
            this.lblGameType.Size = new System.Drawing.Size(65, 13);
            this.lblGameType.TabIndex = 14;
            this.lblGameType.Text = "Game Type:";
            // 
            // cmbGameType
            // 
            this.cmbGameType.FormattingEnabled = true;
            this.cmbGameType.Items.AddRange(new object[] {
            "Apples Only",
            "Apples and Rotten Apples"});
            this.cmbGameType.Location = new System.Drawing.Point(89, 250);
            this.cmbGameType.Name = "cmbGameType";
            this.cmbGameType.Size = new System.Drawing.Size(144, 21);
            this.cmbGameType.TabIndex = 15;
            this.cmbGameType.SelectedIndexChanged += new System.EventHandler(this.cmbGameType_SelectedIndexChanged);
            // 
            // lblControlType
            // 
            this.lblControlType.AutoSize = true;
            this.lblControlType.Location = new System.Drawing.Point(13, 280);
            this.lblControlType.Name = "lblControlType";
            this.lblControlType.Size = new System.Drawing.Size(70, 13);
            this.lblControlType.TabIndex = 16;
            this.lblControlType.Text = "Control Type:";
            // 
            // cmbControlType
            // 
            this.cmbControlType.FormattingEnabled = true;
            this.cmbControlType.Items.AddRange(new object[] {
            "Alternating",
            "Together"});
            this.cmbControlType.Location = new System.Drawing.Point(89, 277);
            this.cmbControlType.Name = "cmbControlType";
            this.cmbControlType.Size = new System.Drawing.Size(144, 21);
            this.cmbControlType.TabIndex = 17;
            this.cmbControlType.SelectedIndexChanged += new System.EventHandler(this.cmbControlType_SelectedIndexChanged);
            // 
            // lblQuickCalibration
            // 
            this.lblQuickCalibration.AutoSize = true;
            this.lblQuickCalibration.Location = new System.Drawing.Point(13, 202);
            this.lblQuickCalibration.Name = "lblQuickCalibration";
            this.lblQuickCalibration.Size = new System.Drawing.Size(90, 13);
            this.lblQuickCalibration.TabIndex = 18;
            this.lblQuickCalibration.Text = "Quick Calibration:";
            // 
            // lblYAxisController
            // 
            this.lblYAxisController.AutoSize = true;
            this.lblYAxisController.Location = new System.Drawing.Point(13, 307);
            this.lblYAxisController.Name = "lblYAxisController";
            this.lblYAxisController.Size = new System.Drawing.Size(38, 13);
            this.lblYAxisController.TabIndex = 19;
            this.lblYAxisController.Text = "Y-axis:";
            // 
            // cmbYAxisController
            // 
            this.cmbYAxisController.FormattingEnabled = true;
            this.cmbYAxisController.Items.AddRange(new object[] {
            "Right Controller",
            "Left Controller"});
            this.cmbYAxisController.Location = new System.Drawing.Point(89, 304);
            this.cmbYAxisController.Name = "cmbYAxisController";
            this.cmbYAxisController.Size = new System.Drawing.Size(144, 21);
            this.cmbYAxisController.TabIndex = 20;
            this.cmbYAxisController.SelectedIndexChanged += new System.EventHandler(this.cmbYAxisController_SelectedIndexChanged);
            // 
            // cbShowCamera
            // 
            this.cbShowCamera.AutoSize = true;
            this.cbShowCamera.Enabled = false;
            this.cbShowCamera.Location = new System.Drawing.Point(109, 179);
            this.cbShowCamera.Name = "cbShowCamera";
            this.cbShowCamera.Size = new System.Drawing.Size(15, 14);
            this.cbShowCamera.TabIndex = 21;
            this.cbShowCamera.UseVisualStyleBackColor = true;
            this.cbShowCamera.CheckedChanged += new System.EventHandler(this.cbShowCamera_CheckedChanged);
            // 
            // lblShowCamera
            // 
            this.lblShowCamera.AutoSize = true;
            this.lblShowCamera.Enabled = false;
            this.lblShowCamera.Location = new System.Drawing.Point(13, 179);
            this.lblShowCamera.Name = "lblShowCamera";
            this.lblShowCamera.Size = new System.Drawing.Size(76, 13);
            this.lblShowCamera.TabIndex = 22;
            this.lblShowCamera.Text = "Show Camera:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(65, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 16);
            this.label2.TabIndex = 23;
            this.label2.Text = "Select A User";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbTestGroup
            // 
            this.cmbTestGroup.FormattingEnabled = true;
            this.cmbTestGroup.Items.AddRange(new object[] {
            "Group A",
            "Group B"});
            this.cmbTestGroup.Location = new System.Drawing.Point(89, 223);
            this.cmbTestGroup.Name = "cmbTestGroup";
            this.cmbTestGroup.Size = new System.Drawing.Size(144, 21);
            this.cmbTestGroup.TabIndex = 25;
            // 
            // lblTestGroup
            // 
            this.lblTestGroup.AutoSize = true;
            this.lblTestGroup.Location = new System.Drawing.Point(13, 226);
            this.lblTestGroup.Name = "lblTestGroup";
            this.lblTestGroup.Size = new System.Drawing.Size(63, 13);
            this.lblTestGroup.TabIndex = 24;
            this.lblTestGroup.Text = "Test Group:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(245, 24);
            this.menuStrip1.TabIndex = 26;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.purgeResultsToolStripMenuItem,
            this.purgeDebugLogsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // purgeResultsToolStripMenuItem
            // 
            this.purgeResultsToolStripMenuItem.Name = "purgeResultsToolStripMenuItem";
            this.purgeResultsToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.purgeResultsToolStripMenuItem.Text = "Purge Results";
            this.purgeResultsToolStripMenuItem.Click += new System.EventHandler(this.purgeResultsToolStripMenuItem_Click);
            // 
            // purgeDebugLogsToolStripMenuItem
            // 
            this.purgeDebugLogsToolStripMenuItem.Name = "purgeDebugLogsToolStripMenuItem";
            this.purgeDebugLogsToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.purgeDebugLogsToolStripMenuItem.Text = "Purge Debug Logs";
            this.purgeDebugLogsToolStripMenuItem.Click += new System.EventHandler(this.purgeDebugLogsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // lblTypeOfPlay
            // 
            this.lblTypeOfPlay.AutoSize = true;
            this.lblTypeOfPlay.Location = new System.Drawing.Point(10, 364);
            this.lblTypeOfPlay.Name = "lblTypeOfPlay";
            this.lblTypeOfPlay.Size = new System.Drawing.Size(65, 13);
            this.lblTypeOfPlay.TabIndex = 27;
            this.lblTypeOfPlay.Text = "Type of play";
            // 
            // cmbTypeOfPlay
            // 
            this.cmbTypeOfPlay.FormattingEnabled = true;
            this.cmbTypeOfPlay.Items.AddRange(new object[] {
            "Warmup",
            "40 Target User Study"});
            this.cmbTypeOfPlay.Location = new System.Drawing.Point(89, 361);
            this.cmbTypeOfPlay.Name = "cmbTypeOfPlay";
            this.cmbTypeOfPlay.Size = new System.Drawing.Size(144, 21);
            this.cmbTypeOfPlay.TabIndex = 28;
            this.cmbTypeOfPlay.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // UserSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 443);
            this.Controls.Add(this.cmbTypeOfPlay);
            this.Controls.Add(this.lblTypeOfPlay);
            this.Controls.Add(this.cmbTestGroup);
            this.Controls.Add(this.lblTestGroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblShowCamera);
            this.Controls.Add(this.cbShowCamera);
            this.Controls.Add(this.cmbYAxisController);
            this.Controls.Add(this.lblYAxisController);
            this.Controls.Add(this.lblQuickCalibration);
            this.Controls.Add(this.cmbControlType);
            this.Controls.Add(this.lblControlType);
            this.Controls.Add(this.cmbGameType);
            this.Controls.Add(this.lblGameType);
            this.Controls.Add(this.cbQuickCalibration);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEditUser);
            this.Controls.Add(this.btnNewUser);
            this.Controls.Add(this.cmbUsers);
            this.Controls.Add(this.btnStartGame);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "UserSelection";
            this.Text = "User Information";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.ComboBox cmbUsers;
        private System.Windows.Forms.Button btnNewUser;
        private System.Windows.Forms.Button btnEditUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbQuickCalibration;
        private System.Windows.Forms.Label lblGameType;
        private System.Windows.Forms.ComboBox cmbGameType;
        private System.Windows.Forms.Label lblControlType;
        private System.Windows.Forms.ComboBox cmbControlType;
        private System.Windows.Forms.Label lblQuickCalibration;
        private System.Windows.Forms.Label lblYAxisController;
        private System.Windows.Forms.ComboBox cmbYAxisController;
        private System.Windows.Forms.CheckBox cbShowCamera;
        private System.Windows.Forms.Label lblShowCamera;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbTestGroup;
        private System.Windows.Forms.Label lblTestGroup;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem purgeResultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem purgeDebugLogsToolStripMenuItem;
        private System.Windows.Forms.Label lblTypeOfPlay;
        private System.Windows.Forms.ComboBox cmbTypeOfPlay;
    }
}