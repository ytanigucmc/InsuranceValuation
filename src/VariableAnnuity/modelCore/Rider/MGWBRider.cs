using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public enum MGWRRidePhase
    {
        Initial,
        GrowthPhase,
        WithdrawPhase,
        AutomaticPeriodicBenefitStatus,
        LastDeath
    }

    public class MGWBRider: BaseRider, IBaseComputable, IWithdrawable
    {


        public double BaseAmount { get;set; }
        public double StepUpRate { get; set; }

        public bool StepUpEligibility { get; set; }

        public bool RebalanceIndiactor { get; set; }

        public MGWRRidePhase RiderPhase = MGWRRidePhase.Initial;

        public IInterpolation UnivariateMortalityTable { get; set; }

        public IInterpolation MaximumAnnualWithdrawl { get; set; }

        public MGWBRider(double baseAmount, double chargeRate, double stepUpRate, IInterpolation mortalityTable, IInterpolation maximumAnnualWithdrawl) : base(chargeRate)
        {
            BaseAmount = baseAmount;
            StepUpRate = stepUpRate;
            UnivariateMortalityTable = mortalityTable;
            MaximumAnnualWithdrawl = maximumAnnualWithdrawl;
        }

        
        public override double GetRiderChargeAmount()
        {
            ContractValue * RiderChargeRate;

        }
        
        public double GetBaseAmount()
        {
            return BaseAmount;
        }

        public void UpdateBaseAmount(CashflowRecords Records)
        {
            double temp1 =0, temp2 = 0, temp3 = 0;
            if (RiderPhase == MGWRRidePhase.GrowthPhase)
            {
                temp1 = Records[-1]["AV Post-Death Claims"];
            }

            temp2 = Records[-1]["WithdrawlBase"] * (1 - UnivariateMortalityTable.Interpolate(Age)) + Records[-1]["Contribution"];

            if (StepUpEligibility)
            {
                temp3 = Records[-1]["WithdrawlBase"] * (1 - UnivariateMortalityTable.Interpolate(Age)) * (1 + StepUpRate) + Records[-1]["Contribution"] - Records[-1]["M&E/Fund Fees"] - Records[-1]["RiderCharge"];
            }

            BaseAmount =  new double[] { temp1, temp2, temp3 }.Max();
        }

        public void Withdraw(double dollarAmount)
        {
            if (RiderPhase == MGWRRidePhase.WithdrawPhase && dollarAmount > 0) 
            {
                BaseAmount -= GetWithdrawlExcessAllowance(dollarAmount);
            }
            if (RiderPhase == MGWRRidePhase.GrowthPhase && dollarAmount >0)
            {
                RiderPhase = MGWRRidePhase.WithdrawPhase;
                BaseAmount -= GetWithdrawlExcessAllowance(dollarAmount);
            }
        }

        private double GetWithdrawlExcessAllowance(double dollarAmount)
        {
            return Math.Max(dollarAmount - BaseAmount * MaximumAnnualWithdrawl.Interpolate(Age), 0);
        }

    }
}
