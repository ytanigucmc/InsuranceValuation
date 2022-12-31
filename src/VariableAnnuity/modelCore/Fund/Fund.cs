using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class Fund : BaseFund
    {
        protected BaseReturnGenerator PercentageReturnGenerator;

        public Fund(string fundName, double startAmount, BaseReturnGenerator perentageReturnGenerator): base(fundName, startAmount)
        {
            PercentageReturnGenerator = perentageReturnGenerator;
        }

        public override string GetFundName()
        {
            return FundName;
        }

        public override double GetFundAmount()
        {
            return FundAmount;
        }

        public override void SetFundAmount(double amount)
        {
            FundAmount = amount;
        }

        public override void GrowFund()
        {
            AddAmountByPercentage(PercentageReturnGenerator.GetReturn());
        }

        public override void AddDollarAmount(double amount)
        {
            FundAmount += amount;
            FundAmount = Math.Max(0, FundAmount);
        }
        public override void AddAmountByPercentage(double percentageAmount)
        {
            AddDollarAmount(percentageAmount * FundAmount);
        }

        public override void DeducDollartAmount(double amount)
        {
            AddDollarAmount(-amount);
        }

        public override void DeductPercentageAmount(double percentageAmount)
        {
            DeducDollartAmount(-percentageAmount * FundAmount);
        }

    }
}