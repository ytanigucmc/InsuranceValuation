using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal class BaseVariableAnnuitySimulatable: IVariableAnnuitySimulatable
    {
        public BaseVariableAnnuity(DateTime contractDate, BasePolicyHolder contractOwner, DateTime annuityStartDate, BasePolicyHolder annuiant, double mortalityExpenseRiskCharge, double fundFees, BaseFundsPortfolio funds, List<IRider> riders) : base(contractDate, BasePolicyHolder contractOwner, DateTime annuityStartDate, BasePolicyHolder annuiant, double mortalityExpenseRiskCharge, double fundFees, BaseFundsPortfolio funds, List < IRider > riders)
    }
}
