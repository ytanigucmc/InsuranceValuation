using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal abstract class Fund : BaseFund
    {
        protected BaseReturnGenerator PercentageReturnGenerator;

        public Fund(double startAmount, BaseReturnGenerator perentageReturnGenerator): base(startAmount)
        {
            PercentageReturnGenerator = perentageReturnGenerator;
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

        public override void AddAmount(double amount)
        {
            FundAmount += amount;
            FundAmount = Math.Max(0, FundAmount);
        }
        public override void AddAmountByPercentage(double percentageAmount)
        {
            AddAmount(percentageAmount * FundAmount);
        }

        public override void DeductAmount(double amount)
        {
            AddAmount(-amount);
        }

        public override void DeducAmountByPercentage(double percentageAmount)
        {
            DeductAmount(-percentageAmount * FundAmount);
        }

    }
}