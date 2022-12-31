using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    internal interface IContract
    {
        DateTime ContractDate { get; }
        int ContractYear { get;}
        BasePolicyHolder ContractOwner { get;}

        double GetContractValue();
    }
}
