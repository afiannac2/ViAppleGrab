namespace ViAppleGrab.Windows_Forms
{
    partial class InitMenu
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
            this.btnEditUsers = new System.Windows.Forms.Button();
            this.btnViewResults = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPlay2 = new System.Windows.Forms.Button();
            this.btnPlay4 = new System.Windows.Forms.Button();
            this.lblUsers = new System.Windows.Forms.Label();
            this.lblRunStudy = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblCurrentUserLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurrentUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnRedoWarmup = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.utilitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createFirewallRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFirewallRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backupResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveResults = new System.Windows.Forms.Button();
            this.btnPlay3 = new System.Windows.Forms.Button();
            this.btnRedoWarmup2 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEditUsers
            // 
            this.btnEditUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditUsers.Location = new System.Drawing.Point(12, 64);
            this.btnEditUsers.Name = "btnEditUsers";
            this.btnEditUsers.Size = new System.Drawing.Size(260, 38);
            this.btnEditUsers.TabIndex = 0;
            this.btnEditUsers.Text = "1. Select and Edit Users";
            this.btnEditUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditUsers.UseVisualStyleBackColor = true;
            this.btnEditUsers.Click += new System.EventHandler(this.btnEditUsers_Click);
            // 
            // btnViewResults
            // 
            this.btnViewResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewResults.Location = new System.Drawing.Point(12, 399);
            this.btnViewResults.Name = "btnViewResults";
            this.btnViewResults.Size = new System.Drawing.Size(260, 38);
            this.btnViewResults.TabIndex = 1;
            this.btnViewResults.Text = "6. View Results Files";
            this.btnViewResults.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewResults.UseVisualStyleBackColor = true;
            this.btnViewResults.Click += new System.EventHandler(this.btnViewResults_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay.Location = new System.Drawing.Point(12, 161);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(260, 38);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "2. Run Stage #1: Warmup #1";
            this.btnPlay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(12, 588);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(260, 38);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "9. Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPlay2
            // 
            this.btnPlay2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay2.Location = new System.Drawing.Point(12, 205);
            this.btnPlay2.Name = "btnPlay2";
            this.btnPlay2.Size = new System.Drawing.Size(260, 38);
            this.btnPlay2.TabIndex = 4;
            this.btnPlay2.Text = "3. Run Stage #2: Singles";
            this.btnPlay2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPlay2.UseVisualStyleBackColor = true;
            this.btnPlay2.Click += new System.EventHandler(this.btnPlay2_Click);
            // 
            // btnPlay4
            // 
            this.btnPlay4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay4.Location = new System.Drawing.Point(12, 293);
            this.btnPlay4.Name = "btnPlay4";
            this.btnPlay4.Size = new System.Drawing.Size(260, 38);
            this.btnPlay4.TabIndex = 5;
            this.btnPlay4.Text = "5. Run Stage #4: Simultaneous";
            this.btnPlay4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPlay4.UseVisualStyleBackColor = true;
            this.btnPlay4.Click += new System.EventHandler(this.btnPlay4_Click);
            // 
            // lblUsers
            // 
            this.lblUsers.AutoSize = true;
            this.lblUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsers.Location = new System.Drawing.Point(78, 35);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(125, 20);
            this.lblUsers.TabIndex = 6;
            this.lblUsers.Text = "Manage Users";
            // 
            // lblRunStudy
            // 
            this.lblRunStudy.AutoSize = true;
            this.lblRunStudy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRunStudy.Location = new System.Drawing.Point(96, 132);
            this.lblRunStudy.Name = "lblRunStudy";
            this.lblRunStudy.Size = new System.Drawing.Size(93, 20);
            this.lblRunStudy.TabIndex = 7;
            this.lblRunStudy.Text = "Run Study";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(87, 370);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "View Results";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(70, 515);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Other Operations";
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(12, 544);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(260, 38);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "8. Reset for Next User";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCurrentUserLbl,
            this.lblCurrentUser});
            this.statusStrip1.Location = new System.Drawing.Point(0, 640);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(284, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblCurrentUserLbl
            // 
            this.lblCurrentUserLbl.Name = "lblCurrentUserLbl";
            this.lblCurrentUserLbl.Size = new System.Drawing.Size(158, 17);
            this.lblCurrentUserLbl.Text = "No user is currently selected!";
            // 
            // lblCurrentUser
            // 
            this.lblCurrentUser.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblCurrentUser.Name = "lblCurrentUser";
            this.lblCurrentUser.Size = new System.Drawing.Size(0, 17);
            // 
            // btnRedoWarmup
            // 
            this.btnRedoWarmup.Location = new System.Drawing.Point(188, 168);
            this.btnRedoWarmup.Name = "btnRedoWarmup";
            this.btnRedoWarmup.Size = new System.Drawing.Size(75, 23);
            this.btnRedoWarmup.TabIndex = 12;
            this.btnRedoWarmup.Text = "Redo";
            this.btnRedoWarmup.UseVisualStyleBackColor = true;
            this.btnRedoWarmup.Visible = false;
            this.btnRedoWarmup.Click += new System.EventHandler(this.btnRedoWarmup_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.utilitiesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // utilitiesToolStripMenuItem
            // 
            this.utilitiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createFirewallRuleToolStripMenuItem,
            this.removeFirewallRuleToolStripMenuItem,
            this.backupResultsToolStripMenuItem});
            this.utilitiesToolStripMenuItem.Name = "utilitiesToolStripMenuItem";
            this.utilitiesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.utilitiesToolStripMenuItem.Text = "Utilities";
            // 
            // createFirewallRuleToolStripMenuItem
            // 
            this.createFirewallRuleToolStripMenuItem.Name = "createFirewallRuleToolStripMenuItem";
            this.createFirewallRuleToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.createFirewallRuleToolStripMenuItem.Text = "Create Firewall Rule";
            this.createFirewallRuleToolStripMenuItem.Click += new System.EventHandler(this.createFirewallRuleToolStripMenuItem_Click);
            // 
            // removeFirewallRuleToolStripMenuItem
            // 
            this.removeFirewallRuleToolStripMenuItem.Name = "removeFirewallRuleToolStripMenuItem";
            this.removeFirewallRuleToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.removeFirewallRuleToolStripMenuItem.Text = "Remove Firewall Rule";
            this.removeFirewallRuleToolStripMenuItem.Click += new System.EventHandler(this.removeFirewallRuleToolStripMenuItem_Click);
            // 
            // backupResultsToolStripMenuItem
            // 
            this.backupResultsToolStripMenuItem.Name = "backupResultsToolStripMenuItem";
            this.backupResultsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.backupResultsToolStripMenuItem.Text = "Backup Results";
            this.backupResultsToolStripMenuItem.Click += new System.EventHandler(this.backupResultsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // btnSaveResults
            // 
            this.btnSaveResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveResults.Location = new System.Drawing.Point(12, 443);
            this.btnSaveResults.Name = "btnSaveResults";
            this.btnSaveResults.Size = new System.Drawing.Size(260, 38);
            this.btnSaveResults.TabIndex = 14;
            this.btnSaveResults.Text = "7. Save Results Files";
            this.btnSaveResults.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveResults.UseVisualStyleBackColor = true;
            this.btnSaveResults.Click += new System.EventHandler(this.btnSaveResults_Click);
            // 
            // btnPlay3
            // 
            this.btnPlay3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay3.Location = new System.Drawing.Point(12, 249);
            this.btnPlay3.Name = "btnPlay3";
            this.btnPlay3.Size = new System.Drawing.Size(260, 38);
            this.btnPlay3.TabIndex = 15;
            this.btnPlay3.Text = "4. Run Stage #3: Warmup #2";
            this.btnPlay3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPlay3.UseVisualStyleBackColor = true;
            this.btnPlay3.Click += new System.EventHandler(this.btnPlay3_Click);
            // 
            // btnRedoWarmup2
            // 
            this.btnRedoWarmup2.Location = new System.Drawing.Point(188, 256);
            this.btnRedoWarmup2.Name = "btnRedoWarmup2";
            this.btnRedoWarmup2.Size = new System.Drawing.Size(75, 23);
            this.btnRedoWarmup2.TabIndex = 16;
            this.btnRedoWarmup2.Text = "Redo";
            this.btnRedoWarmup2.UseVisualStyleBackColor = true;
            this.btnRedoWarmup2.Visible = false;
            this.btnRedoWarmup2.Click += new System.EventHandler(this.btnRedoWarmup2_Click);
            // 
            // InitMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 662);
            this.Controls.Add(this.btnRedoWarmup2);
            this.Controls.Add(this.btnPlay3);
            this.Controls.Add(this.btnSaveResults);
            this.Controls.Add(this.btnRedoWarmup);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblRunStudy);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.btnPlay4);
            this.Controls.Add(this.btnPlay2);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnViewResults);
            this.Controls.Add(this.btnEditUsers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "InitMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome to ViAppleGrab!";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEditUsers;
        private System.Windows.Forms.Button btnViewResults;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPlay2;
        private System.Windows.Forms.Button btnPlay4;
        private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.Label lblRunStudy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblCurrentUserLbl;
        private System.Windows.Forms.ToolStripStatusLabel lblCurrentUser;
        private System.Windows.Forms.Button btnRedoWarmup;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem utilitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFirewallRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFirewallRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backupResultsToolStripMenuItem;
        private System.Windows.Forms.Button btnSaveResults;
        private System.Windows.Forms.Button btnPlay3;
        private System.Windows.Forms.Button btnRedoWarmup2;
    }
}