using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BasePolicyHolder: IPolicyHolder
    {
        protected int Age;
        public BasePolicyHolder(int age)
        {
            Age = age;
        }

        public abstract int GetAge();
        public abstract void IncrementAge(int increment);
    }
}
