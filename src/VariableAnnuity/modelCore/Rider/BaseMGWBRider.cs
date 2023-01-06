using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public enum MGWRRiderPhase
    {
        GrowthPhase,
        WithdrawPhase,
        AutomaticPeriodicBenefitStatus,
        LastDeath
    }

    public abstract class BaseMGWBRider: BaseRiderBaseComputable
    {


        public IInterpolation MaximumAnnualWithdrawl;

        public MGWRRiderPhase RiderPhase { get; set; }
        public BaseMGWBRider(double baseAmount, double chargeRate, IInterpolation maximumAnnualWithdrawl) :base(baseAmount, chargeRate)
        {
            MaximumAnnualWithdrawl = maximumAnnualWithdrawl;
            RiderPhase = MGWRRiderPhase.GrowthPhase;
            
        }


        public bool IsPhase(MGWRRiderPhase phase)
        {
            return RiderPhase == phase;
        }

        public virtual void SetPhase(MGWRRiderPhase phase)
        {
            RiderPhase = phase;
        }

        public virtual double GetMaximumWithdrawlAllowance(int age)
        {
            return RiderBase.GetDollarAmount() * MaximumAnnualWithdrawl.Interpolate(age);
        }

        public virtual double GetMaximumWithdrawlAllowance(int age, double baseAmount)
        {
            return baseAmount * MaximumAnnualWithdrawl.Interpolate(age);
        }

        public virtual double GetMaximumWithdrawlRate(int age)
        {
            return MaximumAnnualWithdrawl.Interpolate(age);
        }

        public virtual double GetWithdrawlExcessAllowance(double withdrawAmount, int age)
        {
            return Math.Max(withdrawAmount - GetMaximumWithdrawlAllowance(age), 0);
        }

        public abstract bool IsAccountDepleted();

        public virtual bool IsRebalance()
        {
            return false;
        }


    }
}
