using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal interface IFund
    {
        string GetFundNme();
        double GetFundAmount();
        void SetFundAmount(double amount);
        void GrowFund();
        void AddDollarAmount(double amount);
        void AddAmountByPercentage(double amountPercentage);
        void DeducDollartAmount(double amount);

        void DeductPercentageAmount(double amountPercentage);

    }
}
