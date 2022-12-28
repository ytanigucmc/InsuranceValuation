using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal abstract class BaseRandomNumberGenerator : IRandomNumberGenerator
    {
        public abstract double GetRandom();
    }
}
