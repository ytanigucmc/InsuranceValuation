using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BaseDeathBenefitRider: BaseRider, IBaseComputable
    {
        public double BaseAmount { get; set; }
        public BaseDeathBenefitRider(double baseAmount, double chargeRate):base(chargeRate) 
        {
            BaseAmount = baseAmount;
        }

        public double GetBaseAmount()
        {
            return BaseAmount;
        }

        public abstract void IncreaseBaseDollarAmount(double dollar);

        public abstract void IncreaseBasePercentageAmount(double percentage);

        public abstract void DecreaseBaseDollarAmount(double dollar);

        public abstract void DecreaseBasePercentageAmount(double percentage);

    }


    public class ReturnOfPremiumDeathBenefitRider: BaseDeathBenefitRider
    {
        public ReturnOfPremiumDeathBenefitRider(double baseAmount, double chargeRate): base(baseAmount, chargeRate)
        {
        }
        public override void IncreaseBasePerentageAmount(double percentage)
        {
            BaseAmount += percentage * BaseAmount;
        }

        public override void DeductBasePerentageAmount(double percentage)
        {
            IncreaseBasePerentageAmount(-percentage);
        }
    }

    public class LifePayPlusDeathBenefitRider : BaseDeathBenefitRider
    {
        public LifePayPlusDeathBenefitRider(double baseAmount, double chargeRate = 0) : base(baseAmount, chargeRate)
        {
        }
        public override void UpdateBaseAmount()
        {
            BaseAmount = Math.Max((1 - MortalityRate) * BaseAmount + Contribution - Fees - PrevWithdrawAmount - RiderChargeRate, 0) ;
        }

        public override double GetClaimAmount()
        {
            return BaseAmount * MortalityRate;
        }
    }
}
