using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class WithdrawlStrategy: BaseWithdrawlStrategy
    {
        ILifePayPlusVariableAnnuity Annuity;
        double DefaultWithdrawlRate;
        int FirstWithdrawlAge;
        int LastWithdrawlAge;

        public WithdrawlStrategy(ILifePayPlusVariableAnnuity annuity, double defaultWithdrawlRate, int firstWithdrawlAge, int lastWithdrawlAge):base()
        { 
            Annuity = annuity;
            DefaultWithdrawlRate = defaultWithdrawlRate;
            FirstWithdrawlAge = firstWithdrawlAge;
            LastWithdrawlAge = lastWithdrawlAge;
        }

        public override double GetWithdrawlAmount()
        {
            int age = Annuity.ContractOwner.GetAge();
            if (age <= FirstWithdrawlAge || age >= LastWithdrawlAge)
                return 0;

            else if (age == (FirstWithdrawlAge + 1))
            {
                Annuity.WithdrawlRider.SetPhase(MGWRRiderPhase.WithdrawPhase);
                return Annuity.GetWtihdrawlBase() * DefaultWithdrawlRate;
            }

            else if (Annuity.GetContractValue() > 0)
            {
                return Annuity.GetWtihdrawlBase() * DefaultWithdrawlRate;
            }

            else if (Annuity.GetContractValue() == 0)
                return Annuity.GetMaximumWithdrawlAllowance();

            else
                throw new Exception("Unhandled condition for withdraw strategy");



        }


    }
}
