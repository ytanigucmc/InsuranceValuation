using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public interface IBaseComputable
    {
        double GetBaseAmount();

        void IncreaseBaseDollarAmount(double dollar);

        void IncreaseBasePercentageAmount(double percentage);

        void DecreaseBaseDollarAmount(double dollar);

        void DecreaseBasePercentageAmount(double percentage);

    }
}
