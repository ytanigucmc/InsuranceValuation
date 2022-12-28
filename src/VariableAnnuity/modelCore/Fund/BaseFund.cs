using MathNet.Numerics.Optimization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal abstract class BaseFund: IFund
    {
        protected double FundAmount;

        public BaseFund(double startAmount)
        {
            if (startAmount < 0) 
            {
                throw new ArgumentException("Start amount of Fund must be non-negative.");
            }
            FundAmount = startAmount;
        }

        public abstract double GetFundAmount();
        public abstract void GrowFund();
        public abstract void SetFundAmount(double amount);

        public abstract void AddAmount(double amount);
        public abstract void AddAmountByPercentage(double percentageAmount);
        public abstract void DeductAmount(double amount);
        public abstract void DeducAmountByPercentage(double percentageAmount);

    }
}
