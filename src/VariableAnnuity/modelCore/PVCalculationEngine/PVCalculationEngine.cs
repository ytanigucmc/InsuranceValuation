using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.modelCore.PresentValueCalculator;

namespace VariableAnnuity
{
    public class PVCalculationEngine: BasePVCalculationEngine
    {
        public IDiscountCurve DiscountCurve;
        public PVCalculationEngine(IDiscountCurve dCUrve):base()
        {
            DiscountCurve = dCUrve;
        }

        public override List<double> FromDataTable(DataTable dt, string labelForTime, List<string> headersForPV)
        {
            RecordHolder recorder = new RecordHolder();
            List<int> years = dt.AsEnumerable().Select(item => item.Field<int>(labelForTime)).ToList();
            List<double> DFs = (from year in years select DiscountCurve.GetDF(year)).ToList();
            List<double> CFs;
            List<double> PVs = new List<double>();
            foreach (string header in headersForPV)
            {
                CFs = dt.AsEnumerable().Select(item => item.Field<double>(header)).ToList();
                PVs.Add(CFs.Zip(DFs, (d1, d2) => d1 * d2).Sum());
            }
            return PVs;
        }
    }
}
