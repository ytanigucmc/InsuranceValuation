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
        void AddPercentageAmount(double amountPercentage);
        void DeductDollarAmount(double amount);
        void DeductPercentageAmount(double amountPercentage);
    }
}
