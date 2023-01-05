using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
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

        public override DataTable FromDataTable(DataTable dt, string labelForTime, List<string> headersForPV, List<string> newHeaders)
        {

            DataTable dtOut = new DataView(dt).ToTable(false, headersForPV.ToArray());


            dtOut.Columns.Add("DF").SetOrdinal(0);
            dtOut.Columns.Add("Year").SetOrdinal(0);

            List<int> years = dt.AsEnumerable().Select(item => item.Field<int>(labelForTime)).ToList();
            List<double> DFs = (from year in years select DiscountCurve.GetDF(year)).ToList();
            for (int i =0; i < years.Count; i++)
            {
                dtOut.Rows[i]["DF"] = DFs[i];
                dtOut.Rows[i]["Year"] = years[i];
            }

            var row = dtOut.NewRow();
            List<double> CFs;
            List<double> PVs = new List<double>();
            foreach (string header in headersForPV)
            {
                CFs = dt.AsEnumerable().Select(item => item.Field<double>(header)).ToList();
                row[header] = CFs.Zip(DFs, (d1, d2) => d1 * d2).Sum();
            }
            dtOut.Rows.InsertAt(row, 0);
            foreach (var header in headersForPV.Zip(newHeaders, Tuple.Create))
            {
                dtOut.Columns[header.Item1].ColumnName = header.Item2;
            }

            return dtOut;




        }
    }
}
