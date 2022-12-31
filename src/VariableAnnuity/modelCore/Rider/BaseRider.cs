using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class BaseRider : IRider
    {

        public double RiderChargeRate;

        public BaseRider(double chargeRate)
        {
            RiderChargeRate = chargeRate;
        }

        public double GetRiderChargeRate()
        {
            return RiderChargeRate;
        }




    }

}
