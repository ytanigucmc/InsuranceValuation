using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.configs;

namespace VariableAnnuity.cmds
{
    internal class VariableAnnuityMain
    {
        static void Main(string[] args)
        {
            Console.WriteLine("In main now");
            string config_file = args[0];
            VariableAnnuityConfig config = new VariableAnnuityConfig(config_file);

        }

    }
}
