using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity.modelCore.PresentValueCalculator
{
    internal interface IDiscountCurve
    {
        double GetDF(double t);
    }
}
