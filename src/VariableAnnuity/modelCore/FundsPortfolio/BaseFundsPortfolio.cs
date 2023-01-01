using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BaseFundsPortfolio : IFundsPortfolio
    {

        protected string PortfolioName;
        protected List<IFund> Funds;



        public BaseFundsPortfolio(string portfolioName, List<IFund> funds)
        {
            PortfolioName = portfolioName;
            Funds = funds;
        }

        public string GetPortfolioName()
        {
            return PortfolioName;
        }

        public double GetPortfolioAmount()
        {
            double sum = 0;
            GetFundsAmounts().ForEach(x => sum += x);
            return sum;
        }

        public List<string> GetFundsNames()
        {
            return (from fund in Funds select fund.GetFundName()).ToList();
        }


        public List<double> GetFundsAmounts()
        {
            return (from fund in Funds select fund.GetDollarAmount()).ToList();
        }

        public List<string> GetPortfolioAndFundsNames()
        {
            List<string> names = GetFundsNames();
            names.Insert(0, GetPortfolioName());
            return names;
        }

        public List<double> GetPortfolioAndFundsAmounts()
        {
            List<double> amounts = GetFundsAmounts();
            amounts.Insert(0, GetPortfolioAmount());
            return amounts;
        }
        public abstract List<double> GetPortfolioWeights();
        public abstract void GrowFunds();
        public abstract void AddDollarAmount(double amount);
        public abstract void AddPercentageAmount(double amountPercentage);
        public abstract void DeductDollarAmount(double amount);
        public abstract void DeductPercentageAmount(double amountPercentage);
        public abstract void Rebalance(List<double> weights);

    }
}
