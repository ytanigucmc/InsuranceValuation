using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class MoneyAccount: BaseMoneyAccount
    {
        public MoneyAccount(double startAmount) : base(startAmount)
        {
        }

        public override double GetDollarAmount()
        {
            return MoneyAmount;
        }

        public override void SetDollarAmount(double amount)
        {
            MoneyAmount = amount;
        }

        public override void AddDollarAmount(double amount)
        {
            MoneyAmount += amount;
            MoneyAmount = Math.Max(0, MoneyAmount);
        }
        public override void AddAmountByPercentage(double percentageAmount)
        {
            AddDollarAmount(percentageAmount * MoneyAmount);
        }

        public override void DeducDollartAmount(double amount)
        {
            AddDollarAmount(-amount);
        }

        public override void DeductPercentageAmount(double percentageAmount)
        {
            DeducDollartAmount(-percentageAmount * MoneyAmount);
        }
    }
}
