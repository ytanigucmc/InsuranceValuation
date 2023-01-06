using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity.modelCore.VariableAnnuity
{
    public class LifePayPlusVariableAnnuity: BaseVariableAnnuity, ILifePayPlusVariableAnnuity
    {
        public BaseMGWBRider WithdrawlRider { get; }
        public List<BaseDeathBenefitRider> DeathRiders { get; }

        double CumulativeDeathPayment;

        public LifePayPlusVariableAnnuity(DateTime contractDate, BasePolicyHolder contractOwner, int annuityStartAge, BasePolicyHolder annuiant, double mortalityExpenseRiskCharge, double fundFees, BaseFundsPortfolio funds, BaseMGWBRider withdrawlRider, List<BaseDeathBenefitRider> deathRiders) :
            base(contractDate, contractOwner, annuityStartAge, annuiant, mortalityExpenseRiskCharge, fundFees, funds)
        {
            WithdrawlRider = withdrawlRider;
            DeathRiders = deathRiders;
            List<BaseRider> riders = new List<BaseRider>();
            riders.Add(withdrawlRider);
            riders.AddRange(deathRiders);
            SetRiders(riders);
        }

        public override List<double> GetDeathBenefitBases()
        {
            return (from rider in DeathRiders select rider.GetBaseAmount()).ToList();
        }

        public override double GetDeathPayemntAmount(double rate)
        {
            return GetDeathBenefitBases().Max() * rate;
        }

        public override void TakeDeathPayment(double deathPaymentAmount)
        {
            base.TakeDeathPayment(deathPaymentAmount);
            CumulativeDeathPayment += deathPaymentAmount;
        }

        public override void RebalanceFunds(List<double> targetWeights)
        {
            if (IsRebalance())
            {
                base.RebalanceFunds(targetWeights);
            }

            if (GetContractValue()>0)
            {
                Funds.AddDollarAmount(1, CumulativeDeathPayment);
            }
            CumulativeDeathPayment = 0;

        }

        public override double GetMaximumWithdrawlAllowance()
        {
            return WithdrawlRider.GetMaximumWithdrawlAllowance(ContractOwner.GetAge());
        }
 

        public override double GetMaximumWithdrawlRate()
        {
            return WithdrawlRider.GetMaximumWithdrawlRate(ContractOwner.GetAge());
        }

        public override List<double> GetWtihdrawlBases()
        {
            return new List<double>() { WithdrawlRider.GetBaseAmount()};
        }

        public double GetWtihdrawlBase()
        {
            return GetWtihdrawlBases()[0];
        }

        public override bool IsRebalance()
        {
            return WithdrawlRider.IsRebalance();
        }

    }
}
