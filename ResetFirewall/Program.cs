using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetFwTypeLib;

namespace ResetFirewall
{
    class Program
    {
        static void Main(string[] args)
        {
            //Remove the firewall rule
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            firewallPolicy.Rules.Remove("ViAppleGrab");
        }
    }
}
