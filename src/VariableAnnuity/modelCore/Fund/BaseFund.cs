using MathNet.Numerics.Optimization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BaseFund: IFund
    {
        protected string FundName;

        protected double FundAmount;

        public BaseFund(string fundName, double startAmount)
        {
            if (startAmount < 0) 
            {
                throw new ArgumentException("Start amount of Fund must be non-negative.");
            }
            FundName = fundName;
            FundAmount = startAmount;
        }

        public abstract double GetFundAmount();
        public abstract string GetFundName();
        public abstract void GrowFund();
        public abstract void SetFundAmount(double amount);

        public abstract void AddDollarAmount(double amount);
        public abstract void AddAmountByPercentage(double percentageAmount);
        public abstract void DeducDollartAmount(double amount);
        public abstract void DeductPercentageAmount(double percentageAmount);

    }
}
