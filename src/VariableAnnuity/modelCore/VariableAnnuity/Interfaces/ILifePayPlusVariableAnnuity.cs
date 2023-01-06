using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public interface ILifePayPlusVariableAnnuity: IVariableAnnuity
    {
        BaseMGWBRider WithdrawlRider {get;}
        List<BaseDeathBenefitRider> DeathRiders { get; }

        double GetWtihdrawlBase();
    }
}
