using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BaseRiderBaseComputable: BaseRider, IBaseComputable
    {
        public MoneyAccount RiderBase;
        public BaseRiderBaseComputable(double baseAmount, double chargeRate): base(chargeRate)
        {
            RiderBase = new MoneyAccount(baseAmount);
        }

        public double GetBaseAmount()
        {
            return RiderBase.GetDollarAmount();
        }

        public void IncreaseBaseDollarAmount(double dollar)
        {
            RiderBase.AddDollarAmount(dollar);
        }

        public void IncreaseBasePercentageAmount(double percentage)
        {
            RiderBase.AddPercentageAmount(percentage);
        }

        public void DecreaseBaseDollarAmount(double dollar)
        {
            RiderBase.DeductDollarAmount(dollar);
        }

        public void DecreaseBasePercentageAmount(double percentage)
        {
            RiderBase.DeductPercentageAmount(percentage);
        }

    }
}
