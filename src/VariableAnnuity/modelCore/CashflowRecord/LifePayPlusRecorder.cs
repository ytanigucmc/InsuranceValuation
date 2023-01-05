using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class LifePayPlusRecorder: RecordHolder
    {
        public LifePayPlusRecorder():base() { } 

        public void AddFundsData(BaseVariableAnnuity annuity, string suffix = "")
        {
            List<(string, double)> fundsData = annuity.Funds.GetPortfolioAndFundsNameAndAmounts();
            foreach (var x in fundsData)
            {
                AddElement(x.Item1 + " " + suffix, x.Item2);
            }
        }

        public void AddLifePlusPhaseIndicators(LifePayPlusMGWBRider rider)
        {
            AddElement("Eligible Step-Up", Convert.ToInt32(rider.StepUpEligibility));
            AddElement("Growth Phase", Convert.ToInt32(rider.RiderPhase == MGWRRidePhase.GrowthPhase));
            AddElement("Withdrawal Phase", Convert.ToInt32(rider.RiderPhase == MGWRRidePhase.WithdrawPhase));
            AddElement("Automatic Periodic Benefit Status", Convert.ToInt32(rider.RiderPhase == MGWRRidePhase.AutomaticPeriodicBenefitStatus));
            AddElement("Last Death", Convert.ToInt32(rider.RiderPhase == MGWRRidePhase.LastDeath));

        }

        public void AddFundsReturn(BaseVariableAnnuity annuity,  List<double> fundReturns, string suffix = "")
        {
            List<string> fundNames = annuity.Funds.GetFundsNames();
            foreach (var x in fundNames.Zip(fundReturns, Tuple.Create))
            {
                AddElement(x.Item1 + " " + suffix, x.Item2);
            }
        }

        public DataTable ToDataTale()
        {
            DataTable dt = new DataTable();
            foreach (var item in AllRecords[0])
            {
                dt.Columns.Add(item.Item1, typeof(object));
            }
            foreach (var record in AllRecords)
            {
                var row = dt.NewRow();
                foreach (var item in record)
                {
                    row[item.Item1] = (item.Item2);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }

    public class VariableAnnuityCashflowRecorder2
    {
        public DataTable Records;

        public DataRow Current; 
        public VariableAnnuityCashflowRecorder2(DataTable dt)
        {
            Records = dt;
            Current = dt.NewRow();
        }

        public DataTable GetRecords()
        {
            return Records;
        }

        public DataRow GetLatestRecord() 
        { 
            return Current;
        }

        public void StartNewRecord()
        {
            Current = Records.NewRow();
        }

        public void AddCurrentRecord()
        {
            Records.Rows.Add(Current);
        }

        public void Add(string key, object value)
        {
            Current[key] = value;
        }

        public void AddBoolAsOneZeoro(string key, bool value)
        {
            Current[key] = value ? 1 : 0;
        }


        public void AddFundsData(BaseVariableAnnuity annuity, string suffix = "")
        {
            List<(string, double)> fundsData = annuity.Funds.GetPortfolioAndFundsNameAndAmounts();
            foreach (var x in fundsData)
            {
                Current[x.Item1 + " " + suffix] = x.Item2;
            }
        }

        public void AddLifePlusPhaseIndicators(LifePayPlusMGWBRider rider)
        {
            AddBoolAsOneZeoro("Eligible Step-Up", rider.StepUpEligibility);
            AddBoolAsOneZeoro("Growth Phase", rider.RiderPhase == MGWRRidePhase.GrowthPhase);
            AddBoolAsOneZeoro("Withdrawal Phase", rider.RiderPhase == MGWRRidePhase.WithdrawPhase);
            AddBoolAsOneZeoro("Automatic Periodic Benefit Status", rider.RiderPhase == MGWRRidePhase.AutomaticPeriodicBenefitStatus);
            AddBoolAsOneZeoro("Last Death", rider.RiderPhase == MGWRRidePhase.LastDeath);
        }

        public void AddFundsReturn(BaseVariableAnnuity annuity, List<double> fundReturns, string suffix = "")
        {
            List<string> fundNames = annuity.Funds.GetFundsNames();
            foreach (var x in fundNames.Zip(fundReturns, Tuple.Create))
            {
                Add(x.Item1 + " " + suffix, x.Item2);
            }
        }




    }
}
