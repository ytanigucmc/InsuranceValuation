using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal class FundsPortfolio : BaseFundsPortfolio
    {
        public FundsPortfolio(List<BaseFund> funds) : base(funds)
        {
        }

        public override double GetPortfolioAmount()
        {
            double sum = 0;
            GetFundsAmounts().ForEach(x => sum += x);
            return sum;
        }
        public override List<double> GetFundsAmounts()
        {
            return (from fund in Funds select fund.GetFundAmount()).ToList();
        }

        public override List<double> GetPortfolioWeights()
        {
            double portAmount = GetPortfolioAmount();
            return (from fund in GetFundsAmounts() select fund / portAmount).ToList();
        }

        public override void GrowFunds()
        {
            foreach (BaseFund fund in Funds)
            {
                fund.GrowFund();
            }
        }
        public override void AddAmount(double amount)
        {
            List<double> portWeights = GetPortfolioWeights();
            var funds_and_weights = Funds.Zip(portWeights, (fund, weight) => (fund, weight));
            foreach (var fw in funds_and_weights)
            {
                fw.fund.AddAmount(fw.weight * amount);
            }
        }
        public override void AddAmountByPercentage(double amountPercentage)
        {
            foreach(BaseFund fun in Funds)
            {
                fun.AddAmountByPercentage(amountPercentage);
            }
          
        }
        public override void DeductAmount(double amount)
        {
            AddAmount(-amount);
        }
        public override void DeductAmountByPercentage(double amountPercentage)
        {
            AddAmountByPercentage(-amountPercentage);
        }

        public override void Rebalance(List<double> weights)
        {
            double portAmount = GetPortfolioAmount();
            var funds_and_weights = Funds.Zip(weights, (fund, weight) => (fund, weight));
            foreach (var fw in funds_and_weights)
            {
                fw.fund.SetFundAmount(portAmount*fw.weight);
            }

        }
    }
}