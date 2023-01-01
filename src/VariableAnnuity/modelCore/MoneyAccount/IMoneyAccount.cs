using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public interface IMoneyAccount
    {
        double GetDollarAmount();
        void SetDollarAmount(double amount);
        void AddDollarAmount(double amount);
        void AddAmountByPercentage(double amountPercentage);
        void DeducDollartAmount(double amount);
        void DeductPercentageAmount(double amountPercentage);
    }
}
