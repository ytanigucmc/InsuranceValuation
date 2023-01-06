using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BaseDeathBenefitRider: BaseRiderBaseComputable
    {
        public BaseDeathBenefitRider(double baseAmount, double chargeRate):base(baseAmount, chargeRate) { }

        public virtual double GetDeathPaymentAmount(double baseAmounnt, double rate)
        {
            return baseAmounnt * rate;
        }

        public virtual double GetDeathPaymentAmount(double rate)
        {
            return GetBaseAmount() * rate;
        }
    }
}
