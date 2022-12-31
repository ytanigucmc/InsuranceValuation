using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal interface IFeePaidHandlable
    {
        void OnFeePaid(double feePaid);
    }
}
