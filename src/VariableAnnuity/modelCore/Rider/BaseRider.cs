using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public static class RiderTypeNames
    {
        public static string DeathBenefit = "DeathBenefit";
        public static string MGWB = "MGWB";
    }

    public abstract class BaseRider : IRider
    {

        public double RiderChargeRate;
        public string RiderTypeName;
        public string RiderName;

        public BaseRider(double chargeRate)
        {
            RiderChargeRate = chargeRate;
            RiderTypeName = "";
            RiderName = "";
        }

        public double GetRiderChargeRate()
        {
            return RiderChargeRate;
        }

        public string GetRiderName()
        {
            return RiderName;
        }
        public string GetRiderTypeName()
        {
            return RiderTypeName;
        }
    }

}
