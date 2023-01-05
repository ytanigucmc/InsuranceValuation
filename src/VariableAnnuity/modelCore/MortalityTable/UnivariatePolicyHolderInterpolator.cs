using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class ConstantPolicyHolderInterpolator: BasePolicyHolderInterpolator
    {
        double FixedResponse;
        public ConstantPolicyHolderInterpolator(double fixedRate)
        {
            FixedResponse = fixedRate;
        }
        public override double Interpolate(BasePolicyHolder holder)
        {
            return FixedResponse;
        }
    }

    public class StepPolicyHolderInterpolator : BasePolicyHolderInterpolator
    {
        IInterpolation Interpolator;
        public StepPolicyHolderInterpolator(Double[] ages, Double[] mortalityRates)
        {
            Interpolator = new StepInterpolation(ages, mortalityRates);
        }
        public override double Interpolate(BasePolicyHolder holder)
        {
            return Interpolator.Interpolate(holder.GetAge());
        }
    }
}
