using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BaseMoneyAccount: IMoneyAccount
    {

        protected double MoneyAmount;

        public BaseMoneyAccount(double startAmount)
        {
            if (startAmount < 0)
            {
                throw new ArgumentException("Start amount of Fund must be non-negative.");
            }
            MoneyAmount = startAmount;
        }

        public abstract double GetDollarAmount();
        public abstract void SetDollarAmount(double amount);
        public abstract void AddDollarAmount(double amount);
        public abstract void AddPercentageAmount(double percentageAmount);
        public abstract void DeductDollarAmount(double amount);
        public abstract void DeductPercentageAmount(double percentageAmount);

    }
}

