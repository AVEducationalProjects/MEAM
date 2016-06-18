using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MEAM.Model;

namespace MEAMDataGen
{
    class Program
    {
        public Dictionary<string, Action> Actions = new Dictionary<string, Action>
        {
            {"gen-objs", MEAMObjectsGen.GenerateObjects},
            {"gen-schd", MEAMPlanGen.GenerateEmptyYearPlan}
        };

        static void Main(string[] args)
        {
            var program = new Program();

            while (true)
            {
                var cmd = Console.ReadLine();
                if (program.Actions.ContainsKey(cmd))
                    program.Actions[cmd]();
            }
        }
    }
}
