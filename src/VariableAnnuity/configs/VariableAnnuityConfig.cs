using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

        public string GetOutputDir()
        {
            return (string)sections["Paths"]["output_dir"];
        }

        public string GetOutputSuffix()
        {
            return (string)sections["Paths"]["output_suffix"];
        }

        public bool IsFixRandom()
        {
            return (bool)sections["Paths"]["FixRandom"];
        }

        public string GetPathFundReturns()
        {
            return (string)sections["Paths"]["PathFundReturns"];
        }

        public List<double> ParseRandomReturns()
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines((string)sections["Paths"]["PathFundReturns"]);
                return lines.Select(x => double.Parse(x)).ToList();
            }
            catch (Exception e)
            {
                return ReturnsSaved.Returns;
            }

        }


        public double GetStepUp()
        {
            string str_to_parse = (string)sections["ModelAssumptions"]["Step-Up"];
            return ParseNumericString(str_to_parse);
        }

        public int GetStepUpPeriod()
        {
            return (int)sections["ModelAssumptions"]["Step-Up Period (Contract Years)"];
        }

        public double GetRiderChargeRate()
        {
            string str_to_parse = (string)sections["ModelAssumptions"]["Rider Charge"];
            return ParseNumericString(str_to_parse);
        }

        public int GetInitialPremium()
        {
            return (int)sections["ModelAssumptions"]["Initial Premium"];
        }

        public int GetStartingAge()
        {
            return (int)sections["ModelAssumptions"]["Starting Age"];
        }
        public DateTime GetContractDate()
        {
            return (DateTime)sections["ModelAssumptions"]["Contract Date"];
        }

        public int GetFirstWithdrawAge()
        {
            return (int)sections["ModelAssumptions"]["First Withdrawl Age"];
        }


        public int GetAnnuityCommencementAge()
        {
            return (int)sections["ModelAssumptions"]["Annuity CommencementDate/Age"];
        }

        public int GetLastDeathAge()
        {
            return (int)sections["ModelAssumptions"]["Last Death Age"];
        }

        public double GetMortalityRate()
        {
            string str_to_parse = (string)sections["ModelAssumptions"]["Mortality"];
            return ParseNumericString(str_to_parse);
        }

        public double GetWithdrawlRate()
        {
            string str_to_parse = (string)sections["ModelAssumptions"]["Withdrawl Rate"];
            return ParseNumericString(str_to_parse);
        }

        public double GetTargetFixedFundAllocation()
        {
            string str_to_parse = (string)sections["ModelAssumptions"]["Fixed Allocation Funds Automatic Rebalancing Target"];
            return ParseNumericString(str_to_parse);
        }

        public double GetMEFee()
        {
            string str_to_parse = (string)sections["ModelAssumptions"]["M&E"];
            return ParseNumericString(str_to_parse);
        }

        public double GetFundFees()
        {
            string str_to_parse = (string)sections["ModelAssumptions"]["Fund Fees"];
            return ParseNumericString(str_to_parse);
        }

        public double GetRiskFreeRate()
        {
            string str_to_parse = (string)sections["ModelAssumptions"]["Risk Free Rate"];
            return ParseNumericString(str_to_parse);
        }

        public double GetVolatility()
        {
            string str_to_parse = (string)sections["ModelAssumptions"]["Volatility"];
            return ParseNumericString(str_to_parse);
        }

        public (List<double>, List<double>) GetMaxAnnualWithdraw()
        {
            JToken withdrawls = sections["ModelAssumptions"]["Maximum Annual Withdrawl"];
            Dictionary<double, string>  withdrawlMax = withdrawls.ToObject<Dictionary<double, string>>();
            var ages = new List<double>();
            var rates = new List<double>();
            foreach (var item in withdrawlMax)
            {
                ages.Add(item.Key);
                rates.Add(ParseNumericString(item.Value));
            }
            return (ages, rates);
        }

        



        private double ParseNumericString(string value)
        {
            if (value.Contains("%"))
            {
                return double.Parse(value.TrimEnd(new[] { '%' })) / 100;
            }
            else
            {
                return double.Parse(value);
            }
        }

        

        
    }
}
