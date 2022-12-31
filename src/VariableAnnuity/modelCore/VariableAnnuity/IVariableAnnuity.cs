using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal interface IVariableAnnuity: IContract
    {
        DateTime AnnuityStartDate { get;}
        BasePolicyHolder Annuiant { get;}
        double MortalityExpenseRiskCharge { get;}
        double FundFees { get;}
        BaseFundsPortfolio Funds { get;}
        List<BaseRider> Riders { get; }

        double CalculateFeeAmount();

        double PayFee();

        double CalculateRiderChargeAmount();

        double PayRiderCharge();

        void WithdrawDollarAmount(double amount);

        void RebalanceFunds(List<double> targetWeights);



    }
}
