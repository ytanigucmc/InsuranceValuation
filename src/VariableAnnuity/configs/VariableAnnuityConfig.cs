using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VariableAnnuity.configs
{
    internal class VariableAnnuityConfig
    {
        private Dictionary<string, JToken> sections;

        public VariableAnnuityConfig(string path)
        {
            JObject config = JObject.Parse(File.ReadAllText(path));
            var _sections = config.ToObject<Dictionary<string, JToken>>();
            if (_sections is null)
                throw new Exception("Cofnig file is in invalid form");
            else
                sections = _sections;
        }

        public string GetOutputPath()
        {
            return (string)sections["Paths"]["output_file"];
        }

        public double GetStepUp()
        {
            string step_up_str = (string)sections["ModelAssumptions"]["StepUp"];
            return double.Parse(step_up_str.Replace("%",""))/100;
        }

        
    }
}
