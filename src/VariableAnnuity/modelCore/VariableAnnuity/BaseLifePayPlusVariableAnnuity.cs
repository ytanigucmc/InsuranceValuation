using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.modelCore.VariableAnnuity.Interfaces;

namespace VariableAnnuity
{
    public abstract class BaseLifePayPlusVariableAnnuity: BaseVariableAnnuity, IMGWB, IDeathBenefit
    {
        public BaseMGWBRider WithdrawlRider;
        public List<BaseDeathBenefitRider> DeathRiders;

        public BaseLifePayPlusVariableAnnuity(DateTime contractDate, BasePolicyHolder contractOwner, int annuityStartAge, BasePolicyHolder annuiant, double mortalityExpenseRiskCharge, double fundFees, BaseFundsPortfolio funds, BaseMGWBRider withdrawlRider, List<BaseDeathBenefitRider> deathRiders) :
            base(contractDate, contractOwner,  annuityStartAge,  annuiant,  mortalityExpenseRiskCharge, fundFees, funds)
        {
            WithdrawlRider = withdrawlRider;
            DeathRiders = deathRiders;
            List<BaseRider> riders = new List<BaseRider>();
            riders.Add(withdrawlRider);
            riders.AddRange(deathRiders);
            SetRiders(riders);
        }

        public double GetMGWBBase()
        {
            return WithdrawlRider.GetBaseAmount();
        }

        public double GetMaximumBaseBenefit()
        {
            return WithdrawlRider.GetMaximumWithdrawlAllowance(ContractOwner.GetAge());
        }

        public double GetDeathBenefitBase()
        {
            return (from rider in DeathRiders select rider.GetBaseAmount()).Sum();
        }

        public double GetDeathPaymentAmount(double rate)
        {
            return GetDeathBenefitBase()*rate;
        }


    }
}
