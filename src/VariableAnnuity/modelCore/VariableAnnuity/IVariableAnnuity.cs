using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public interface IVariableAnnuity: IContract
    {
        int AnnuityStartAge { get;}
        BasePolicyHolder Annuiant { get;}
        double MortalityExpenseRiskCharge { get;}
        double FundFees { get;}
        BaseFundsPortfolio Funds { get;}
        List<BaseRider> Riders { get; }

        void SetRiders(List<BaseRider> riders);


        double GetFeeAmount();

        double GetFeeAmount(double baseValue);

        double GetRiderChargeAmount();

        double GetRiderChargeAmount(double baseValue);

        void PayFee(double feeAmount);

        void PayRiderCharge(double chargeAmont);

        void ContributeDollarAmount(double contributionAmount);

        void WithdrawDollarAmount(double withdrawAmount);

        void RebalanceFunds(List<double> targetWeights);

        void DeductPerentageAmountRiderBases(double percentage);

         void ApplyFundsReturns(List<double> fundReturns);

        double GetMaximumWithdrawlAllowance();

        double GetMaximumWithdrawlRate();
        double GetDeathPayemntAmount(double rate);

        void TakeDeathPayment(double amount);

        bool IsRebalance();

        List<double> GetDeathBenefitBases();

        List<double> GetWtihdrawlBases();

    }
}
