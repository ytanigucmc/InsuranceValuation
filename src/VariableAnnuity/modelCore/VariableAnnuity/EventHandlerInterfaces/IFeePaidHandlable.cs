using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity

namespace VariableAnnuity
{
    internal interface IFeePaidHandlable
    {
        void OnFeePaid(object source, DollarAmountEventArgs args);
    }
}
