using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BasePolicyHolderInterpolator: IPolicyHolderInterpolator
    {
        public abstract double Interpolate(BasePolicyHolder holder);
    }
}
