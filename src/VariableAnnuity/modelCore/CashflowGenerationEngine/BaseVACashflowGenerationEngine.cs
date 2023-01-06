using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{

    public abstract class BaseVACashflowGenerationEngine
    {
        public ILifePayPlusVariableAnnuity Annuity;
        public List<BaseReturnGenerator> ReturnGenerators;
        public BaseVACashflowGenerationEngine(ILifePayPlusVariableAnnuity annuity, List<BaseReturnGenerator> returnGenerators)
        {
            Annuity = annuity;
            ReturnGenerators = returnGenerators;
        }
        public abstract DataTable GenerateCashflowRecords();
    }
}
