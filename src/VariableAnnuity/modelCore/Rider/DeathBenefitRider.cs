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

    public class LifePayPlusDeathBenefitRider : BaseDeathBenefitRider, IContributionMadeHandlable, IFeePaidHandlable, IWithdrawMadeHandlable, IRiderChargeHandlable
    {
        protected double WithdrawAmountLastYear;
        public LifePayPlusDeathBenefitRider(double baseAmount, double chargeRate = 0) : base(baseAmount, chargeRate)
        {
            WithdrawAmountLastYear = 0;
        }


        public void OnCotributionMade(object source, DollarAmountEventArgs args)
        {
            BaseAmount += args.DollarAmount;
        }

        public void OnFeePaid(object source, DollarAmountEventArgs args)
        {
            BaseAmount -= args.DollarAmount;
        }

        public void OnRiderChargePaid(object source, DollarAmountEventArgs args)
        {
            BaseAmount -= args.DollarAmount;
        }

        public void OnWithdrawMade(object source, DollarAmountEventArgs args)
        {
            BaseAmount -= WithdrawAmountLastYear;
            WithdrawAmountLastYear = args.DollarAmount;
        }

    }
}
