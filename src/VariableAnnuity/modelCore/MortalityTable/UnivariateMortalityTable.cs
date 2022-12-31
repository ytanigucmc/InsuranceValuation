using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class ConstantMortalityTable: BaseMortalityTable
    {
        double FixedMortalityRate;
        public ConstantMortalityTable(double fixedRate)
        {
            FixedMortalityRate = fixedRate;
        }
        public override double GetMortalityRate(BasePolicyHolder holder)
        {
            return FixedMortalityRate;
        }
    }

    public class PiecewiseConstantAgeBasedMortalityTable : BaseMortalityTable
    {
        IInterpolation MortalityTableInterpolator;
        public PiecewiseConstantAgeBasedMortalityTable(Double[] ages, Double[] mortalityRates)
        {
            MortalityTableInterpolator = new StepInterpolation(ages, mortalityRates);
        }
        public override double GetMortalityRate(BasePolicyHolder holder)
        {
            return MortalityTableInterpolator.Interpolate(holder.GetAge());
        }
    }
}
