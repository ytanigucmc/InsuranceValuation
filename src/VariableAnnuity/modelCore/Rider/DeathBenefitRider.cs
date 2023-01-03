using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.modelCore.Rider;
using VariableAnnuity.modelCore.VariableAnnuity.EventHandlerInterfaces;

namespace VariableAnnuity
{


    public class ReturnOfPremiumDeathBenefitRider: BaseRiderBaseComputable
    {
        public ReturnOfPremiumDeathBenefitRider(double baseAmount, double chargeRate): base(baseAmount, chargeRate)
        {
        }
    }

    public class LifePayPlusDeathBenefitRider : BaseRiderBaseComputable, IContributionMadeHandlable, IFeePaidHandlable, IWithdrawMadeHandlable, IRiderChargeHandlable, IAnniversaryReachedHandlable
    {
        private double CumulativeWithdrawAmountLastYear;

        private double CumulativeWithdrawAmountThisYear;

        private double CumulativeBasisAdjustment;

        public LifePayPlusDeathBenefitRider(double baseAmount, double chargeRate = 0) : base(baseAmount, chargeRate)
        {
            CumulativeWithdrawAmountLastYear = 0;
            CumulativeWithdrawAmountThisYear = 0;
            CumulativeBasisAdjustment = 0;
        }


        public void OnCotributionMade(object source, DollarAmountEventArgs args)
        {
            CumulativeBasisAdjustment += args.DollarAmount;
        }

        public void OnFeePaid(object source, DollarAmountEventArgs args)
        {
            CumulativeBasisAdjustment -= args.DollarAmount;
        }

        public void OnRiderChargePaid(object source, DollarAmountEventArgs args)
        {
            CumulativeBasisAdjustment -= args.DollarAmount;
        }

        public void OnWithdrawMade(object source, DollarAmountEventArgs args)
        {
            CumulativeWithdrawAmountThisYear += args.DollarAmount;
        }

        public void OnAnniversaryReached(object source, EventArgs args)
        {
            RiderBase.AddDollarAmount(CumulativeBasisAdjustment - CumulativeWithdrawAmountLastYear);
            CumulativeWithdrawAmountLastYear = CumulativeWithdrawAmountThisYear;
        }

    }
}
