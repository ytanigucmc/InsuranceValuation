using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;

namespace VariableAnnuity
{
    internal class RandomNormalGenerator: BaseRandomNumberGenerator

    {
        public override double GetRandom()
        {
            return Normal.Sample(0, 1);
        }
    }
}
