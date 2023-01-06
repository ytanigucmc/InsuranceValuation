using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity.modelCore.VariableAnnuity.Interfaces
{
    public interface IDeathBenefit
    {
        public double GetDeathBenefitBase();

        public double GetDeathPaymentAmount(double rate);
    }
}
