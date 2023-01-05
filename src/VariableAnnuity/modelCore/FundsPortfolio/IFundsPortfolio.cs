using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VariableAnnuity
{
    internal interface IFundsPortfolio
    {
        string GetPortfolioName();
        double GetPortfolioAmount();
        List<string> GetFundsNames();
        List<double> GetFundsAmounts();
        List<string> GetPortfolioAndFundsNames();
        List<double> GetPortfolioAndFundsAmounts();
        void ApplyReturns(List<double> fundsReturns);
        void AddDollarAmount(double amount);
        void AddDollarAmount(int index, double amount);
        void AddDollarAmount(string fundName, double amount);
        void AddPercentageAmount(double amountPercentage);
        void DeductDollarAmount(double amount);
        void DeductPercentageAmount(double amountPercentage);
        List<double> GetPortfolioWeights();



        void Rebalance(List<double> weights);
        
    }
}
