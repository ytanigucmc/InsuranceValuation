using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public interface IPVCalculationEngine
    {
        List<double> FromDataTable(DataTable dt, string labelForTime, List<string> itmesForPV);
    }
}
