using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.modelCore.VariableAnnuity.EventHandlerInterfaces;

namespace VariableAnnuity
{

    public class DollarAmountEventArgs : EventArgs
    {
        public double DollarAmount { get; set; }
    }

    public abstract partial class BaseVariableAnnuity: BaseContract, IVariableAnnuity
    {
        public int AnnuityStartAge { get; protected set; }
        public BasePolicyHolder Annuiant { get; protected set; }
        public double MortalityExpenseRiskCharge { get; set; }
        public double FundFees { get; set; }
        public BaseFundsPortfolio Funds { get; set; }
        
        public List<BaseRider> Riders { get; protected set; }

        public BaseVariableAnnuity(DateTime contractDate, BasePolicyHolder contractOwner, int annuityStartAge, BasePolicyHolder annuiant, double mortalityExpenseRiskCharge, double fundFees, BaseFundsPortfolio funds, List<BaseRider> riders):base(contractDate, contractOwner)
        {
            AnnuityStartAge = annuityStartAge;
            Annuiant = annuiant;
            MortalityExpenseRiskCharge = mortalityExpenseRiskCharge;
            FundFees = fundFees;
            Funds = funds;
            Riders = riders;
            RegisterRidersForEventHandlers(riders);
        }

        public double CalculateFeeAmount()
        {
            return GetContractValue() * (MortalityExpenseRiskCharge + FundFees);
        }


        public double CalculateRiderChargeAmount()
        {
            return GetContractValue() * (from rider in Riders select rider.GetRiderChargeRate()).ToArray().Sum();
        }

        public abstract void PayFee(double feeAMount);

        public abstract void PayRiderCharge(double riderChargeAmount);

        public abstract void ContributeDollarAmount(double contributionAmount);

        public abstract void WithdrawDollarAmount(double withdrawAmount);

        public abstract void RebalanceFunds(List<double> targetWeights);

        public abstract void DeductPerentageAmountRiderBases(double percentage);

    }

    public partial class BaseVariableAnnuity : BaseContract, IVariableAnnuity
    {
        public event EventHandler<DollarAmountEventArgs> FeePaid;

        public event EventHandler<DollarAmountEventArgs> RiderChargePaid;

        public event EventHandler<DollarAmountEventArgs> ContributionMade;

        public event EventHandler<DollarAmountEventArgs> WithdrawMade;

        public event EventHandler<EventArgs> AnniversaryReached;

        private void RegisterRidersForEventHandlers(List<BaseRider> riders)
        {
            foreach(BaseRider rider in riders)
            {
                if (rider is IFeePaidHandlable _rider1)
                {
                    FeePaid += _rider1.OnFeePaid;
                }

                if (rider is IRiderChargeHandlable _rider2)
                {
                    FeePaid += _rider2.OnRiderChargePaid;
                }

                if (rider is IContributionMadeHandlable _rider3)
                {
                    FeePaid += _rider3.OnCotributionMade;
                }

                if (rider is IWithdrawMadeHandlable _rider4)
                {
                    FeePaid += _rider4.OnWithdrawMade;
                }

                if (rider is IAnniversaryReachedHandlable _rider5)
                {
                    AnniversaryReached += _rider5.OnAnniversaryReached;
                }
            }
        }

        protected virtual void OnFeePaid(double feeAmount)
        {
            FeePaid?.Invoke(this, new DollarAmountEventArgs() { DollarAmount = feeAmount });
        }

        protected virtual void OnRiderChargeMade(double chargeAmount)
        {
            RiderChargePaid?.Invoke(this, new DollarAmountEventArgs() { DollarAmount = chargeAmount });
        }

        protected virtual void OnContributionMade(double cotributionAmount)
        {
            ContributionMade?.Invoke(this, new DollarAmountEventArgs() { DollarAmount = cotributionAmount });
        }

        protected virtual void OnWithdrawMade(double withdrwAmount)
        {
            WithdrawMade?.Invoke(this, new DollarAmountEventArgs() { DollarAmount = withdrwAmount });
        }

        protected virtual void OnAnniversaryReached()
        {
            AnniversaryReached?.Invoke(this, new EventArgs());
        }
    }


}
