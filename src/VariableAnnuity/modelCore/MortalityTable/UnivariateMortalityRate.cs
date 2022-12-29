using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class ConstantUnivariateMortalityTable: BaseUnivariateMortalityTable
    {
        double FixedMortalityRate;
        public ConstantUnivariateMortalityTable(double fixedRate)
        {
            FixedMortalityRate = fixedRate;
        }
        public override double GetMortalityRate(double x)
        {
            return FixedMortalityRate;
        }
    }

    public class PiecewiseConstantUnivariateMortalityTable : BaseUnivariateMortalityTable
    {
        IInterpolation MortalityTableInterpolator;
        public PiecewiseConstantUnivariateMortalityTable(Double[] ages, Double[] mortalityRates)
        {
            MortalityTableInterpolator = new StepInterpolation(ages, mortalityRates);
        }
        public override double GetMortalityRate(double x)
        {
            return MortalityTableInterpolator.Interpolate(x);
        }
    }
}
