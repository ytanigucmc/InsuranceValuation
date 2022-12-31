using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class FundsPortfolio : BaseFundsPortfolio
    {
        public FundsPortfolio(string portfolioName, List<BaseFund> funds) : base(portfolioName, funds)
        {
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
        public override void AddDollarAmount(double amount)
        {
            List<double> portWeights = GetPortfolioWeights();
            var funds_and_weights = Funds.Zip(portWeights, (fund, weight) => (fund, weight));
            foreach (var fw in funds_and_weights)
            {
                fw.fund.AddDollarAmount(fw.weight * amount);
            }
        }
        public override void AddPercentageAmount(double amountPercentage)
        {
            foreach(BaseFund fun in Funds)
            {
                fun.AddAmountByPercentage(amountPercentage);
            }
          
        }
        public override void DeductDollarAmount(double amount)
        {
            AddDollarAmount(-amount);
        }
        public override void DeductPercentageAmount(double amountPercentage)
        {
            AddPercentageAmount(-amountPercentage);
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