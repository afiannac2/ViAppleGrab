using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using NetFwTypeLib;
using Ionic.Zip;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace FirewallConfig
{
    [RunInstaller(true)]
    public partial class ViAppleGrabInstaller : System.Configuration.Install.Installer
    {
        public ViAppleGrabInstaller()
        {
            InitializeComponent();
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            //Add the firewall rule to the set of firewall rules
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FWRule"));

            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallRule.Description = "Used to allow all communication between the PS3 and the computer.";
            firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            firewallRule.Enabled = true;
            firewallRule.InterfaceTypes = "All";
            firewallRule.Protocol = 17;
            firewallRule.Name = "ViAppleGrab";

            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            firewallPolicy.Rules.Add(firewallRule);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Uninstall(IDictionary savedState)
        {
            //Remove the firewall rule
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            firewallPolicy.Rules.Remove("ViAppleGrab");

            //Zip up the results files just to be safe
            if (Directory.EnumerateFiles(@"C:\Program Files (x86)\UNR\ViAppleGrab\Results").Any())
            {
                using (ZipFile zip = new ZipFile())
                {
                    string[] files = Directory.GetFiles(@"C:\Program Files (x86)\UNR\ViAppleGrab\Results");
                    zip.AddDirectory(@"C:\Program Files (x86)\UNR\ViAppleGrab\Results");
                    zip.Comment = "This archive contains all results files from ViAppleGrab which were created before the program was uninstalled";
                    zip.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\ViAppleGrab_results.zip");
                    MessageBox.Show("As a safety precaution, all results files were zipped into the file ViAppleGrab_results.zip on your desktop, before the program was uninstalled...");
                }
            }

            base.Uninstall(savedState);
        }
    }
}
