using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class PolicyHolder: BasePolicyHolder
    {
        public PolicyHolder(int age):base(age)
        {
        }

        public override int GetAge()
        {
            return Age;
        }

        public override void IncrementAge(int increment)
        {
            Age += increment;
            if (Age < 0)
            {
                Age = 0;
            }
        }
    }
}
