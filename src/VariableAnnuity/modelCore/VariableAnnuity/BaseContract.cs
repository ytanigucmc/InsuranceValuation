using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BaseContract: IContract
    {
        public DateTime ContractDate { get; protected set; }
        public int ContractYear { get; protected set; }
        public BasePolicyHolder ContractOwner { get; protected set; }

        public BaseContract(DateTime contractDate,BasePolicyHolder contractOwner)
        {
            ContractDate = contractDate;
            ContractYear = 0;
            ContractOwner = contractOwner;
        }

        public abstract double GetContractValue();
    }
}
