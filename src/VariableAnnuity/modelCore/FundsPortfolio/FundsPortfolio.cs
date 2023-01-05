using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class FundsPortfolio : BaseFundsPortfolio
    {
        public FundsPortfolio(string portfolioName, List<IFund> funds) : base(portfolioName, funds)
        {
        }


        public override List<double> GetPortfolioWeights()
        {
            double portAmount = GetPortfolioAmount();
            return (from fund in GetFundsAmounts() select fund / portAmount).ToList();
        }

        public override void ApplyReturns(List<double> fundReturns)
        {
            foreach (var fr in Funds.Zip(fundReturns, Tuple.Create))
            {
                fr.Item1.ApplyReturn(fr.Item2);
            }
        }
        public override void AddDollarAmount(double amount)
        {
            if (GetPortfolioAmount() > 0)
            {
                List<double> portWeights = GetPortfolioWeights();
                var funds_and_weights = Funds.Zip(portWeights, (fund, weight) => (fund, weight));
                foreach (var fw in funds_and_weights)
                {
                    fw.fund.AddDollarAmount(fw.weight * amount);
                }
            }
        }

        public override void AddDollarAmount(int index, double amount)
        {
            Funds[index].AddDollarAmount(amount);
        }

        public override void AddDollarAmount(string fundName, double amount)
        {
            IFund fundSpec = (from fund in Funds where fund.GetFundName() == fundName select fund).ToList()[0];
            fundSpec.AddDollarAmount(amount);
        }

        public override void AddPercentageAmount(double amountPercentage)
        {
            foreach(IFund fun in Funds)
            {
                fun.AddPercentageAmount(amountPercentage);
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
            if (Funds.Count != weights.Count)
            {
                throw new Exception("Number of funds and target rebalacning weights do not match");
            }
            double portAmount = GetPortfolioAmount();
            var funds_and_weights = Funds.Zip(weights, (fund, weight) => (fund, weight));
            foreach (var fw in funds_and_weights)
            {
                fw.fund.SetDollarAmount(portAmount*fw.weight);
            }

        }

    }
}