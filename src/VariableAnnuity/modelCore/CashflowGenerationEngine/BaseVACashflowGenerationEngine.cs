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
        public BaseVariableAnnuity Annuity;
        public List<BaseReturnGenerator> ReturnGenerators;
        public BaseVACashflowGenerationEngine(BaseVariableAnnuity annuity, List<BaseReturnGenerator> returnGenerators)
        {
            Annuity = annuity;
            ReturnGenerators = returnGenerators;
        }
        public abstract DataTable GenerateCashflowRecords();
    }
}
