using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BaseFundsPortfolio: IFundsPortfolio
    {

        protected List<BaseFund> Funds;

        public BaseFundsPortfolio(List<BaseFund> funds)
        {
            Funds = funds;
        }

        public abstract double GetPortfolioAmount();
        public abstract List<double> GetFundsAmounts();

        public abstract List<double> GetPortfolioWeights();

        public abstract void GrowFunds();
        public abstract void AddAmount(double amount);
        public abstract void AddAmountByPercentage(double amountPercentage);
        public abstract void DeductAmount(double amount);
        public abstract void DeductAmountByPercentage(double amountPercentage);

        public abstract void Rebalance(List<double> weights);
    }
}
