using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public  class Fund : MoneyAccount, IFund
    {
        public string FundName;
        protected BaseReturnGenerator PercentageReturnGenerator;

        public Fund(string fundName, double startAmount): base(startAmount)
        {
            FundName = fundName;
        }

        public string GetFundName()
        {
            return FundName;
        }

        public void ApplyReturn(double fundReturn)
        {
            AddPercentageAmount(fundReturn);
        }

    }
}