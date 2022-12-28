using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal class PolicyHolder: BasePolicyHolder
    {
        public PolicyHolder(double age):base(age)
        {
        }

        public override double GetAge()
        {
            return Age;
        }

        public override void IncrementAge(double increment)
        {
            Age += increment;
            if (Age < 0)
            {
                Age = 0;
            }
        }
    }
}
