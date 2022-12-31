using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal interface IWithdrawMadeHandlable
    {
        void OnWithdrawMade(object source, DollarAmountEventArgs args);
    }
}
