using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BaseMortalityTable: IMortalityTable
    {
        public abstract double GetMortalityRate(BasePolicyHolder holder);
    }
}
