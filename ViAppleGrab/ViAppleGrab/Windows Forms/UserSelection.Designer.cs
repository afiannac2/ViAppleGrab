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
            this.lblDisplayResults = new System.Windows.Forms.Label();
            this.cbDisplayResults = new System.Windows.Forms.CheckBox();
            this.cbSelectStudy = new System.Windows.Forms.ComboBox();
            this.lblSelectStudy = new System.Windows.Forms.Label();
            this.lblDominantArm = new System.Windows.Forms.Label();
            this.cmbDominantArm = new System.Windows.Forms.ComboBox();
            this.lblStage = new System.Windows.Forms.Label();
            this.cmbStage = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartGame
            // 
            this.btnStartGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartGame.Location = new System.Drawing.Point(13, 443);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(292, 55);
            this.btnStartGame.TabIndex = 8;
            this.btnStartGame.Text = "Start Game";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
            // 
            // cmbUsers
            // 
            this.cmbUsers.FormattingEnabled = true;
            this.cmbUsers.Location = new System.Drawing.Point(13, 139);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(292, 21);
            this.cmbUsers.TabIndex = 9;
            this.cmbUsers.SelectedIndexChanged += new System.EventHandler(this.cmbUsers_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(87, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Configuration Settings";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbQuickCalibration
            // 
            this.cbQuickCalibration.AutoSize = true;
            this.cbQuickCalibration.Location = new System.Drawing.Point(290, 247);
            this.cbQuickCalibration.Name = "cbQuickCalibration";
            this.cbQuickCalibration.Size = new System.Drawing.Size(15, 14);
            this.cbQuickCalibration.TabIndex = 13;
            this.cbQuickCalibration.UseVisualStyleBackColor = true;
            this.cbQuickCalibration.CheckedChanged += new System.EventHandler(this.cbQuickCalibration_CheckedChanged);
            // 
            // lblGameType
            // 
            this.lblGameType.AutoSize = true;
            this.lblGameType.Location = new System.Drawing.Point(13, 325);
            this.lblGameType.Name = "lblGameType";
            this.lblGameType.Size = new System.Drawing.Size(65, 13);
            this.lblGameType.TabIndex = 14;
            this.lblGameType.Text = "Game Type:";
            // 
            // cmbGameType
            // 
            this.cmbGameType.Enabled = false;
            this.cmbGameType.FormattingEnabled = true;
            this.cmbGameType.Items.AddRange(new object[] {
            "Apples Only",
            "Apples and Rotten Apples"});
            this.cmbGameType.Location = new System.Drawing.Point(105, 322);
            this.cmbGameType.Name = "cmbGameType";
            this.cmbGameType.Size = new System.Drawing.Size(200, 21);
            this.cmbGameType.TabIndex = 15;
            this.cmbGameType.SelectedIndexChanged += new System.EventHandler(this.cmbGameType_SelectedIndexChanged);
            // 
            // lblControlType
            // 
            this.lblControlType.AutoSize = true;
            this.lblControlType.Location = new System.Drawing.Point(13, 352);
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
            this.cmbControlType.Location = new System.Drawing.Point(105, 349);
            this.cmbControlType.Name = "cmbControlType";
            this.cmbControlType.Size = new System.Drawing.Size(200, 21);
            this.cmbControlType.TabIndex = 17;
            this.cmbControlType.SelectedIndexChanged += new System.EventHandler(this.cmbControlType_SelectedIndexChanged);
            // 
            // lblQuickCalibration
            // 
            this.lblQuickCalibration.AutoSize = true;
            this.lblQuickCalibration.Location = new System.Drawing.Point(13, 247);
            this.lblQuickCalibration.Name = "lblQuickCalibration";
            this.lblQuickCalibration.Size = new System.Drawing.Size(90, 13);
            this.lblQuickCalibration.TabIndex = 18;
            this.lblQuickCalibration.Text = "Quick Calibration:";
            // 
            // lblYAxisController
            // 
            this.lblYAxisController.AutoSize = true;
            this.lblYAxisController.Location = new System.Drawing.Point(13, 379);
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
            this.cmbYAxisController.Location = new System.Drawing.Point(105, 376);
            this.cmbYAxisController.Name = "cmbYAxisController";
            this.cmbYAxisController.Size = new System.Drawing.Size(200, 21);
            this.cmbYAxisController.TabIndex = 20;
            this.cmbYAxisController.SelectedIndexChanged += new System.EventHandler(this.cmbYAxisController_SelectedIndexChanged);
            // 
            // cbShowCamera
            // 
            this.cbShowCamera.AutoSize = true;
            this.cbShowCamera.Enabled = false;
            this.cbShowCamera.Location = new System.Drawing.Point(290, 224);
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
            this.lblShowCamera.Location = new System.Drawing.Point(13, 224);
            this.lblShowCamera.Name = "lblShowCamera";
            this.lblShowCamera.Size = new System.Drawing.Size(139, 13);
            this.lblShowCamera.TabIndex = 22;
            this.lblShowCamera.Text = "Show Camera: (Debug only)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(109, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 16);
            this.label2.TabIndex = 23;
            this.label2.Text = "Select a User";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbTestGroup
            // 
            this.cmbTestGroup.FormattingEnabled = true;
            this.cmbTestGroup.Items.AddRange(new object[] {
            "Group A",
            "Group B"});
            this.cmbTestGroup.Location = new System.Drawing.Point(105, 295);
            this.cmbTestGroup.Name = "cmbTestGroup";
            this.cmbTestGroup.Size = new System.Drawing.Size(200, 21);
            this.cmbTestGroup.TabIndex = 25;
            // 
            // lblTestGroup
            // 
            this.lblTestGroup.AutoSize = true;
            this.lblTestGroup.Location = new System.Drawing.Point(13, 298);
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
            this.menuStrip1.Size = new System.Drawing.Size(317, 24);
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
            this.lblTypeOfPlay.Location = new System.Drawing.Point(10, 406);
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
            this.cmbTypeOfPlay.Location = new System.Drawing.Point(105, 403);
            this.cmbTypeOfPlay.Name = "cmbTypeOfPlay";
            this.cmbTypeOfPlay.Size = new System.Drawing.Size(200, 21);
            this.cmbTypeOfPlay.TabIndex = 28;
            this.cmbTypeOfPlay.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lblDisplayResults
            // 
            this.lblDisplayResults.AutoSize = true;
            this.lblDisplayResults.Location = new System.Drawing.Point(13, 271);
            this.lblDisplayResults.Name = "lblDisplayResults";
            this.lblDisplayResults.Size = new System.Drawing.Size(149, 13);
            this.lblDisplayResults.TabIndex = 29;
            this.lblDisplayResults.Text = "Display Results at Conclusion:";
            // 
            // cbDisplayResults
            // 
            this.cbDisplayResults.AutoSize = true;
            this.cbDisplayResults.Location = new System.Drawing.Point(290, 271);
            this.cbDisplayResults.Name = "cbDisplayResults";
            this.cbDisplayResults.Size = new System.Drawing.Size(15, 14);
            this.cbDisplayResults.TabIndex = 30;
            this.cbDisplayResults.UseVisualStyleBackColor = true;
            this.cbDisplayResults.CheckedChanged += new System.EventHandler(this.cbDisplayResults_CheckedChanged);
            // 
            // cbSelectStudy
            // 
            this.cbSelectStudy.FormattingEnabled = true;
            this.cbSelectStudy.Items.AddRange(new object[] {
            "Camp Abilities Study",
            "Original GI 2013 Study"});
            this.cbSelectStudy.Location = new System.Drawing.Point(13, 60);
            this.cbSelectStudy.Name = "cbSelectStudy";
            this.cbSelectStudy.Size = new System.Drawing.Size(292, 21);
            this.cbSelectStudy.TabIndex = 32;
            // 
            // lblSelectStudy
            // 
            this.lblSelectStudy.AutoSize = true;
            this.lblSelectStudy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectStudy.Location = new System.Drawing.Point(109, 31);
            this.lblSelectStudy.Name = "lblSelectStudy";
            this.lblSelectStudy.Size = new System.Drawing.Size(108, 16);
            this.lblSelectStudy.TabIndex = 33;
            this.lblSelectStudy.Text = "Select a Study";
            this.lblSelectStudy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDominantArm
            // 
            this.lblDominantArm.AutoSize = true;
            this.lblDominantArm.Location = new System.Drawing.Point(13, 325);
            this.lblDominantArm.Name = "lblDominantArm";
            this.lblDominantArm.Size = new System.Drawing.Size(76, 13);
            this.lblDominantArm.TabIndex = 34;
            this.lblDominantArm.Text = "Dominant Arm:";
            this.lblDominantArm.Visible = false;
            // 
            // cmbDominantArm
            // 
            this.cmbDominantArm.Enabled = false;
            this.cmbDominantArm.FormattingEnabled = true;
            this.cmbDominantArm.Items.AddRange(new object[] {
            "Left",
            "Right"});
            this.cmbDominantArm.Location = new System.Drawing.Point(105, 322);
            this.cmbDominantArm.Name = "cmbDominantArm";
            this.cmbDominantArm.Size = new System.Drawing.Size(200, 21);
            this.cmbDominantArm.TabIndex = 35;
            this.cmbDominantArm.Visible = false;
            // 
            // lblStage
            // 
            this.lblStage.AutoSize = true;
            this.lblStage.Location = new System.Drawing.Point(13, 352);
            this.lblStage.Name = "lblStage";
            this.lblStage.Size = new System.Drawing.Size(38, 13);
            this.lblStage.TabIndex = 36;
            this.lblStage.Text = "Stage:";
            this.lblStage.Visible = false;
            // 
            // cmbStage
            // 
            this.cmbStage.FormattingEnabled = true;
            this.cmbStage.Items.AddRange(new object[] {
            "1. Warmup (10 targets)",
            "2. Single (20 targets on dominant)",
            "3. Simultaneous (20 targets per arm)"});
            this.cmbStage.Location = new System.Drawing.Point(105, 349);
            this.cmbStage.Name = "cmbStage";
            this.cmbStage.Size = new System.Drawing.Size(200, 21);
            this.cmbStage.TabIndex = 37;
            this.cmbStage.Visible = false;
            this.cmbStage.SelectedIndexChanged += new System.EventHandler(this.cmbStage_SelectedIndexChanged);
            // 
            // UserSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 510);
            this.Controls.Add(this.cmbStage);
            this.Controls.Add(this.lblStage);
            this.Controls.Add(this.cmbDominantArm);
            this.Controls.Add(this.lblDominantArm);
            this.Controls.Add(this.lblSelectStudy);
            this.Controls.Add(this.cbSelectStudy);
            this.Controls.Add(this.cbDisplayResults);
            this.Controls.Add(this.lblDisplayResults);
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
        private System.Windows.Forms.Label lblDisplayResults;
        private System.Windows.Forms.CheckBox cbDisplayResults;
        private System.Windows.Forms.ComboBox cbSelectStudy;
        private System.Windows.Forms.Label lblSelectStudy;
        private System.Windows.Forms.Label lblDominantArm;
        private System.Windows.Forms.ComboBox cmbDominantArm;
        private System.Windows.Forms.Label lblStage;
        private System.Windows.Forms.ComboBox cmbStage;
    }
}