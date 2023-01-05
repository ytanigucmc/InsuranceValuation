using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BaseWithdrawlStrategy: IWithdrawStrategy
    {
        public abstract double GetWithdrawlAmount();
    }
}
