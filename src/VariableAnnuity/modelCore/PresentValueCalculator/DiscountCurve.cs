using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.modelCore.PresentValueCalculator;

namespace VariableAnnuity
{
    public class FlatRateDiscountCurve: IDiscountCurve
    {
        public double Rate;
        public FlatRateDiscountCurve(double flatRate)
        {
            Rate = flatRate;
        }

        public double GetDF(double t)
        {
            return Math.Pow(1+ Rate, -t);
        }
    }
}
