using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class VariableAnnuityCashflowRecorder
    {
        public List<(string, dynamic)> record; 
        public VariableAnnuityCashflowRecorder()
        {
            record = new List<(string, dynamic)> ();
        }

        public void AddFundsData(BaseVariableAnnuity annuity, string suffix="")
        {
            List<(string, double)> fundsData = annuity.Funds.GetPortfolioAndFundsNameAndAmounts();
            foreach (var x in fundsData)
            {
                record.Add(((string, dynamic))(x.Item1+suffix, x.Item2));
            }
        }

        public void AddElement<T>(string key, T value)
        {
            record.Add(((string, dynamic))(key, value));
        }

        public void AddLifePlusPhaseIndicators(MGWBRider rider)
        {
            record.Add(((string, int))("Eligible Step-Up", Convert.ToInt32(rider.StepUpEligibility)));
            record.Add(((string, int))("Growth Phase", Convert.ToInt32(rider.RiderPhase == MGWRRidePhase.GrowthPhase)));
            record.Add(((string, int))("Withdrawal Phase", Convert.ToInt32(rider.RiderPhase == MGWRRidePhase.WithdrawPhase)));
            record.Add(((string, int))("Automatic Periodic Benefit Status", Convert.ToInt32(rider.RiderPhase == MGWRRidePhase.AutomaticPeriodicBenefitStatus)));
            record.Add(((string, int))("Last Death", Convert.ToInt32(rider.RiderPhase == MGWRRidePhase.LastDeath)));


        }

        
    }
}
