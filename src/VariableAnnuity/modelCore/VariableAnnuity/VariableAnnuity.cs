using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.modelCore.VariableAnnuity.EventHandlerInterfaces;

namespace VariableAnnuity
{
    public class VariableAnnuity: BaseVariableAnnuity
    {

        public VariableAnnuity(DateTime contractDate, BasePolicyHolder contractOwner, int annuityStartAge, BasePolicyHolder annuiant, double mortalityExpenseRiskCharge, double fundFees, BaseFundsPortfolio funds, List<BaseRider> riders):
            base(contractDate, contractOwner, annuityStartAge, annuiant, mortalityExpenseRiskCharge, fundFees, funds, riders)
        {
        }

        public override double GetMaximumWithdrawlAllowance() { return 0; }
        public override double GetMaximumWithdrawlRate() { return 0; }
        public override double GetDeathPayemntAmount(double rate) { return 0; }
        public override List<double> GetDeathBenefitBases() { return new List<double>(); }
        public override List<double> GetWtihdrawlBases() { return new List<double>(); }

    }
}
