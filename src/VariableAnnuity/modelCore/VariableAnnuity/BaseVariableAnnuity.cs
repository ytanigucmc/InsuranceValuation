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

    public abstract partial class BaseVariableAnnuity : BaseContract, IVariableAnnuity
    {
        public int AnnuityStartAge { get; protected set; }
        public BasePolicyHolder Annuiant { get; protected set; }
        public double MortalityExpenseRiskCharge { get; set; }
        public double FundFees { get; set; }
        public BaseFundsPortfolio Funds { get; set; }
        
        public List<BaseRider> Riders { get; protected set; }

        double CumDeathPayment;

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

        public BaseVariableAnnuity(DateTime contractDate, BasePolicyHolder contractOwner, int annuityStartAge, BasePolicyHolder annuiant, double mortalityExpenseRiskCharge, double fundFees, BaseFundsPortfolio funds) : base(contractDate, contractOwner)
        {
            AnnuityStartAge = annuityStartAge;
            Annuiant = annuiant;
            MortalityExpenseRiskCharge = mortalityExpenseRiskCharge;
            FundFees = fundFees;
            Funds = funds;
        }

        public void SetRiders(List<BaseRider> riders)
        {
            Riders = riders;
            RegisterRidersForEventHandlers(riders);
        }

        public override double GetContractValue()
        {
            return Funds.GetPortfolioAmount();
        }

        public virtual double GetFeeAmount()
        {
            return GetContractValue() * (MortalityExpenseRiskCharge + FundFees);
        }

        public virtual double GetFeeAmount(double baseValue)
        {
            return baseValue * (MortalityExpenseRiskCharge + FundFees);
        }

        public virtual double GetRiderChargeAmount()
        {
            return GetContractValue() * (from rider in Riders select rider.GetRiderChargeRate()).Sum();
        }

        public virtual double GetRiderChargeAmount(double baseValue)
        {
            return baseValue * (from rider in Riders select rider.GetRiderChargeRate()).Sum();
        }

        public virtual void PayFee(double feeAmount)
        {
            Funds.DeductDollarAmount(feeAmount);
            OnFeePaid(feeAmount);
        }

        public virtual void PayRiderCharge(double chargeAmont)
        {
            Funds.DeductDollarAmount(chargeAmont);
            OnRiderChargeMade(chargeAmont);
        }


        public virtual void ContributeDollarAmount(double contributionAmount)
        {
            OnContributionMade(contributionAmount);
        }

        public virtual void WithdrawDollarAmount(double withdrawAmount)
        {
            Funds.DeductDollarAmount(withdrawAmount);
            OnWithdrawMade(withdrawAmount);
        }

        public virtual void TakeDeathPayment(double deathPaymentAmount)
        {
            Funds.DeductDollarAmount(deathPaymentAmount);
            OnDeathPaymentTaken(deathPaymentAmount);
        }


        public virtual void RebalanceFunds(List<double> targetWeights)
        {
            Funds.Rebalance(targetWeights);

        }



        public virtual void DeductPerentageAmountRiderBases(double percentage)
        {
            foreach (BaseRider rider in Riders)
            {
                if (rider is IBaseComputable _rider1)
                {
                    _rider1.DecreaseBasePercentageAmount(percentage);
                }
            }
        }

        public virtual void ApplyFundsReturns(List<double> fundReturns)
        {
            Funds.ApplyReturns(fundReturns);
        }


        public override void AgeContractByOneYear()
        {

            ContractYear += 1;
            LastAnniversaryDate = LastAnniversaryDate.AddYears(1);
            ContractOwner.IncrementAge(1);
            if (!Object.ReferenceEquals(ContractOwner, Annuiant))
            {
                Annuiant.IncrementAge(1);
            }
            OnContractAgedByOneYear();
        }

        public override void UpdateOnAnniversaryReached()
        {
            OnAnniversaryReached();
        }

        public virtual bool IsRebalance()
        {
            return false;
        }

        public abstract double GetMaximumWithdrawlAllowance();

        public abstract double GetMaximumWithdrawlRate();

        public abstract double GetDeathPayemntAmount(double rate);
        public abstract List<double> GetDeathBenefitBases();

        public abstract List<double> GetWtihdrawlBases();




    }

    public abstract partial class BaseVariableAnnuity : BaseContract, IVariableAnnuity
    {
        public event EventHandler<DollarAmountEventArgs> FeePaid;

        public event EventHandler<DollarAmountEventArgs> RiderChargePaid;

        public event EventHandler<DollarAmountEventArgs> ContributionMade;

        public event EventHandler<DollarAmountEventArgs> WithdrawMade;

        public event EventHandler<DollarAmountEventArgs> DeathPaymentTaken;

        public event EventHandler<EventArgs> AnniversaryReached;

        public event EventHandler<EventArgs> ContractAgedByOneYear;

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
                    RiderChargePaid += _rider2.OnRiderChargePaid;
                }

                if (rider is IContributionMadeHandlable _rider3)
                {
                    ContributionMade += _rider3.OnCotributionMade;
                }

                if (rider is IWithdrawMadeHandlable _rider4)
                {
                    WithdrawMade += _rider4.OnWithdrawMade;
                }

                if (rider is IDeathPaymentTakenHandlable _rider5)
                {
                    DeathPaymentTaken += _rider5.OnDeathPaymentTaken;
                }

                if (rider is IAnniversaryReachedHandlable _rider6)
                {
                    AnniversaryReached += _rider6.OnAnniversaryReached;
                }

                if (rider is IContractAgedByOneYearHandlable _rider7)
                {
                    ContractAgedByOneYear += _rider7.OnContractAgedByOneYear;
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

        protected virtual void OnDeathPaymentTaken(double deathAmount)
        {
            DeathPaymentTaken?.Invoke(this, new DollarAmountEventArgs() { DollarAmount = deathAmount });
        }

        protected virtual void OnAnniversaryReached()
        {
            AnniversaryReached?.Invoke(this, new EventArgs());
        }

        protected virtual void OnContractAgedByOneYear()
        {
            ContractAgedByOneYear?.Invoke(this, new EventArgs());
        }
    }


}
