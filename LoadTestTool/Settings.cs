using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTestTool
{
    internal class Settings
    {
        public Settings() { }
        public Settings(string[] args) { LoadArguments(args); }
        public void LoadArguments(string[] args)
        {
            foreach (string arg in args)
            {
                var values = arg.Split('=');
                if (values.Length != 2) continue;
                switch (values[0])
                {
                    case "count":
                        bot_count = int.Parse(values[1]);
                        break;
                    case "scenario":
                        scenario_func_name = values[1];
                        break;
                    case "file":
                        scenario_func_name = "ScenarioFromFile";
                        scenario_file_name = values[1];
                        break;
                }
            }
        }
        public int bot_count = 1;
        public String scenario_func_name = "";
        public String scenario_file_name = "";
    }
}
