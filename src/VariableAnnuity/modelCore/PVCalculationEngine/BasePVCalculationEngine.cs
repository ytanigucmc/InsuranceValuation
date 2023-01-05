using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BasePVCalculationEngine: IPVCalculationEngine
    {
        public BasePVCalculationEngine() { }

        public abstract DataTable FromDataTable(DataTable dt, string labelForTime, List<string> itmesForPV, List<string> newHeaders);
    }
}
