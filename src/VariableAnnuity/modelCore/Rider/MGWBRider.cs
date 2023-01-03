using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.modelCore.Rider;
using VariableAnnuity.modelCore.VariableAnnuity.EventHandlerInterfaces;

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

    public class MGWBRider : BaseRiderBaseComputable, IContributionMadeHandlable, IFeePaidHandlable, IWithdrawMadeHandlable, IRiderChargeHandlable, IAnniversaryReachedHandlable
    {

        public double StepUpRate { get; set; }

        public bool StepUpEligibility { get; set; }

        public bool RebalanceIndiactor { get; set; }

        public int AnnuityCommencementAge { get; set; }

        public int DeathAge { get; set; }

        public MGWRRidePhase RiderPhase = MGWRRidePhase.Initial;

        public IInterpolation MaximumAnnualWithdrawl { get; set; }

        private double CumulativeBaseAdjustment;

        private double CumulativeContribution;

        private double CumulativeWithdraw;

        public MGWBRider(double baseAmount, double chargeRate, double stepUpRate, int annuityCommencementAge ,IInterpolation maximumAnnualWithdrawl) : base(baseAmount, chargeRate)
        {
            StepUpRate = stepUpRate;
            MaximumAnnualWithdrawl = maximumAnnualWithdrawl;
            AnnuityCommencementAge = annuityCommencementAge;
            CumulativeBaseAdjustment = 0;
            CumulativeContribution = 0;
            CumulativeWithdraw = 0;
        }

        private double GetWithdrawlExcessAllowance(double withdrawAmount, int age)
        {
            return Math.Max(withdrawAmount - GetMaximumWithdrawAllowance(age), 0);
        }

        private double GetMaximumWithdrawAllowance(int age)
        {
            return RiderBase.GetDollarAmount() * MaximumAnnualWithdrawl.Interpolate(age);
        }

        public void OnCotributionMade(object source, DollarAmountEventArgs args)
        {
            CumulativeBaseAdjustment += args.DollarAmount;
            CumulativeContribution += args.DollarAmount;

        }

        public void OnFeePaid(object source, DollarAmountEventArgs args)
        {
            CumulativeBaseAdjustment -= args.DollarAmount;
        }

        public void OnRiderChargePaid(object source, DollarAmountEventArgs args)
        {
            CumulativeBaseAdjustment -= args.DollarAmount;
        }

        public void OnWithdrawMade(object source, DollarAmountEventArgs args)
        {
            CumulativeWithdraw += args.DollarAmount;
        }

        public void OnAnniversaryReached(object source, EventArgs args)
        {
            BaseVariableAnnuity annuity = (BaseVariableAnnuity)source;
            double contractValue = annuity.GetContractValue();
            UpdateRiderBase(contractValue, annuity.ContractOwner.GetAge());
            UpdateRiderPhase(annuity.ContractOwner.GetAge(), contractValue);
            CumulativeBaseAdjustment = 0;
            CumulativeContribution = 0;
            CumulativeWithdraw = 0;
        }

        private void UpdateRiderBase(double contractValue, int age)
        {
            double excessWithdraw = GetWithdrawlExcessAllowance(CumulativeWithdraw, age);
            double currentBaseAdjusted = RiderBase.GetDollarAmount() + CumulativeContribution - excessWithdraw;
            double rachetBase = contractValue * Convert.ToInt32(RiderPhase == MGWRRidePhase.GrowthPhase);
            double stepUpeBase = RiderBase.GetDollarAmount() * (1 + StepUpRate) * Convert.ToInt32(StepUpEligibility) + CumulativeBaseAdjustment - excessWithdraw;
            RiderBase.SetDollarAmount(new double[] { currentBaseAdjusted, rachetBase, stepUpeBase }.Max());
        }

        private void UpdateRiderPhase(int age, double contractValue)
        {
            if (CumulativeWithdraw == 0 && age <= AnnuityCommencementAge && age  < DeathAge )
            {
                RiderPhase = MGWRRidePhase.GrowthPhase;
            }    


            else if ((CumulativeWithdraw > 0 || age > AnnuityCommencementAge || age > DeathAge) && contractValue > 0)
            {
                RiderPhase = MGWRRidePhase.WithdrawPhase;
            }

            else if (age < DeathAge || RiderPhase == MGWRRidePhase.WithdrawPhase || contractValue == 0)
            {
                RiderPhase = MGWRRidePhase.AutomaticPeriodicBenefitStatus;
            }

            else if (age < DeathAge || RiderPhase == MGWRRidePhase.AutomaticPeriodicBenefitStatus)
            {
                RiderPhase = MGWRRidePhase.AutomaticPeriodicBenefitStatus;
            }

            else if (age == DeathAge)
            {
                RiderPhase = MGWRRidePhase.LastDeath;
            }

            else
            {
                throw new Exception("Condition not handled by UpdateRiderBase has been reached");
            }
           
        }

    }
}
