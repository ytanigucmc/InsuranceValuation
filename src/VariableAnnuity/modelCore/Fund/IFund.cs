using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal interface IFund
    {
        double GetFundAmount();
        void SetFundAmount(double amount);
        void GrowFund();
        void AddAmount(double amount);
        void AddAmountByPercentage(double amountPercentage);
        void DeductAmount(double amount);

        void DeducAmountByPercentage(double amountPercentage);

    }
}
