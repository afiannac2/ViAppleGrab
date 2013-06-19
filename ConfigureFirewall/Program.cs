using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetFwTypeLib;

namespace ConfigureFirewall
{
    class Program
    {
        static void Main(string[] args)
        {
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
    }
}
