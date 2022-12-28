using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal interface IFundsPortfolio
    {
        double GetPortfolioAmount();
        List<double> GetFundsAmounts();
        void GrowFunds();
        void AddAmount(double amount);
        void AddAmountByPercentage(double amountPercentage);
        void DeductAmount(double amount);
        void DeductAmountByPercentage(double amountPercentage);

        void Rebalance(List<double> weights);
        
    }
}
