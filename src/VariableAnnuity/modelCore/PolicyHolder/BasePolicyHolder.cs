using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BasePolicyHolder: IPolicyHolder
    {
        protected double Age;
        public BasePolicyHolder(double age)
        {
            Age = age;
        }

        public abstract double GetAge();
        public abstract void IncrementAge(double age);
    }
}
