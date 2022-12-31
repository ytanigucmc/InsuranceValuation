using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity.modelCore.VariableAnnuity
{
    public abstract class BaseVariableAnnuity: BaseContract, IVariableAnnuity
    {
        public DateTime AnnuityStartDate { get; set; }
        public BasePolicyHolder Annuiant { get; set; }
        public double MortalityExpenseRiskCharge { get; set; }
        public double FundFees { get; set; }
        public BaseFundsPortfolio Funds { get; set; }
        
        public List<BaseRider> Riders { get; protected set; }

        public event EventHandler<DollarAmountArgs> FeePaid;

        public event EventHandler<DollarAmountArgs> RiderChargePaid;

        public event EventHandler<DollarAmountArgs> ContributionMade;

        public event EventHandler<DollarAmountArgs> WithdrawMade;

        public BaseVariableAnnuity(DateTime contractDate, BasePolicyHolder contractOwner, DateTime annuityStartDate, BasePolicyHolder annuiant, double mortalityExpenseRiskCharge, double fundFees, BaseFundsPortfolio funds, List<BaseRider> riders):base(contractDate, contractOwner)
        {
            AnnuityStartDate = annuityStartDate;
            Annuiant = annuiant;
            MortalityExpenseRiskCharge = mortalityExpenseRiskCharge;
            FundFees = fundFees;
            Funds = funds;
            Riders = riders;
        }

        public double CalculateFeeAmount()
        {
            return GetContractValue() * (MortalityExpenseRiskCharge + FundFees);
        }


        public double CalculateRiderChargeAmount()
        {
            return GetContractValue() * (from rider in Riders select rider.GetRiderChargeRate()).ToArray().Sum();
        }


        public abstract double PayFee();

        public abstract double PayRiderCharge();

        public abstract void ContributeDollarAmount(double amount);

        public abstract void WithdrawDollarAmount(double amount);

        public abstract void RebalanceFunds(List<double> targetWeights);

        public abstract void DeductPerentageAmountRiderBases(double percentage);


        protected virtual void OnFeePaid(double feeAmount)
        {
            FeePaid?.Invoke(this, new DollarAmountArgs() { DollarAmount = feeAmount });
        }

        protected virtual void OnRiderChargeMade(double chargeAmount)
        {
            RiderChargePaid?.Invoke(this, new DollarAmountArgs() { DollarAmount = chargeAmount });
        }

        protected virtual void OnContributionMade(double cotributionAmount)
        {
            ContributionMade?.Invoke(this, new DollarAmountArgs() { DollarAmount = cotributionAmount });
        }

        protected virtual void OnWithdrawMade(double withdrwAmount)
        {
            WithdrawMade?.Invoke(this, new DollarAmountArgs() { DollarAmount = withdrwAmount });
        }
    }

    public class DollarAmountArgs: EventArgs
    {
        public double DollarAmount { get; set; } 
    }
}
