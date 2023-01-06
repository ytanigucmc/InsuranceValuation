using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public interface IContract
    {
        DateTime ContractDate { get; }

        DateTime LastAnniversaryDate{get;}
        int ContractYear { get;}
        BasePolicyHolder ContractOwner { get;}

        double GetContractValue();

        void AgeContractByOneYear();

        void UpdateOnAnniversaryReached();
    }
}
