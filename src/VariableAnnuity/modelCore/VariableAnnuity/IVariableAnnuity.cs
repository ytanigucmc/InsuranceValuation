using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal interface IVariableAnnuity: IContract
    {
        int AnnuityStartAge { get;}
        BasePolicyHolder Annuiant { get;}
        double MortalityExpenseRiskCharge { get;}
        double FundFees { get;}
        BaseFundsPortfolio Funds { get;}
        List<BaseRider> Riders { get; }

        double CalculateFeeAmount();

        void PayFee(double amount);

        double CalculateRiderChargeAmount();

        void PayRiderCharge(double amount);

        void ContributeDollarAmount(double amount);

        void WithdrawDollarAmount(double amount);

        void RebalanceFunds(List<double> targetWeights);

        void DeductPerentageAmountRiderBases(double percentage);



    }
}
