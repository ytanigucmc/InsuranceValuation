using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.modelCore.VariableAnnuity.EventHandlerInterfaces;

namespace VariableAnnuity
{
    public enum MGWRRidePhase
    {
        GrowthPhase,
        WithdrawPhase,
        AutomaticPeriodicBenefitStatus,
        LastDeath
    }

    public class LifePayPlusMGWBRider : BaseRiderBaseComputable, IContributionMadeHandlable, IFeePaidHandlable, IWithdrawMadeHandlable, IRiderChargeHandlable, IAnniversaryReachedHandlable, IContractAgedByOneYearHandlable
    {

        public double StepUpRate { get; set; }

        public double StepUpPeriod { get; set; }

        public bool StepUpEligibility { get; set; }

        public bool RebalanceIndiactor { get; set; }

        public int AnnuityCommencementAge { get; set; }

        public int DeathAge { get; set; }

        public MGWRRidePhase RiderPhase = MGWRRidePhase.GrowthPhase;

        public IInterpolation MaximumAnnualWithdrawl { get; set; }

        private double CumulativeBaseAdjustment;

        private double CumulativeWithdraw;

        public LifePayPlusMGWBRider(double baseAmount, double chargeRate, double stepUpRate, int stepUpPeirod ,int annuityCommencementAge, IInterpolation maximumAnnualWithdrawl, int deathAge) : base(baseAmount, chargeRate)
        {
            StepUpRate = stepUpRate;
            StepUpPeriod = stepUpPeirod;
            MaximumAnnualWithdrawl = maximumAnnualWithdrawl;
            AnnuityCommencementAge = annuityCommencementAge;
            CumulativeBaseAdjustment = 0;
            CumulativeWithdraw = 0;
            RiderTypeName = RiderTypeNames.MGWB;
            RiderName = "LifePayPlusMGWBRider";
            DeathAge = deathAge;
            StepUpEligibility = true;
            RebalanceIndiactor = false;
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
            if (args.DollarAmount < 0)
            {
                throw new Exception("Cannot make negative value contribution");
            }
            RiderBase.AddDollarAmount(args.DollarAmount);

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

        public void OnContractAgedByOneYear(object source, EventArgs args)
        {
            BaseVariableAnnuity annuity = (BaseVariableAnnuity)source;
            double contractValue = annuity.GetContractValue();
            UpdateRiderPhase(annuity.ContractOwner.GetAge(), contractValue);
            UpdateStepUpEligibility(annuity.ContractYear);
            UpdateRebalanceIndicator();

        }

        public void OnAnniversaryReached(object source, EventArgs args)
        {
            BaseVariableAnnuity annuity = (BaseVariableAnnuity)source;
            double contractValue = annuity.GetContractValue();
            UpdateRiderPhase(annuity.ContractOwner.GetAge(), contractValue);
            UpdateRebalanceIndicator();
            UpdateRiderBase(contractValue, annuity.ContractOwner.GetAge());
            CumulativeBaseAdjustment = 0;
            CumulativeWithdraw = 0;
        }

        private void UpdateRiderBase(double contractValue, int age)
        {
            double excessWithdraw = GetWithdrawlExcessAllowance(CumulativeWithdraw, age);
            double currentBaseAdjusted = RiderBase.GetDollarAmount() - excessWithdraw;
            double rachetBase = contractValue * Convert.ToInt32(RiderPhase == MGWRRidePhase.GrowthPhase);
            double stepUpeBase = (RiderBase.GetDollarAmount() * (1 + StepUpRate) + CumulativeBaseAdjustment - excessWithdraw) * Convert.ToInt32(StepUpEligibility);
            RiderBase.SetDollarAmount(new double[] { currentBaseAdjusted, rachetBase, stepUpeBase }.Max());
        }

        private void UpdateRebalanceIndicator()
        {
            if (RiderPhase == MGWRRidePhase.WithdrawPhase || RiderPhase == MGWRRidePhase.AutomaticPeriodicBenefitStatus)
            {
                RebalanceIndiactor = true;
            }

            else
            {
                RebalanceIndiactor = false;
            }
        }

        private void UpdateStepUpEligibility(int contractYear)
        {
            StepUpEligibility = (contractYear <= StepUpPeriod);
        }

        private void UpdateRiderPhase(int age, double contractValue)
        {

            if (age >= DeathAge)
            {
                RiderPhase = MGWRRidePhase.LastDeath;
            }

            else if (CumulativeWithdraw == 0 && age <= AnnuityCommencementAge )
            {
                RiderPhase = MGWRRidePhase.GrowthPhase;
            }    


            else if ((CumulativeWithdraw > 0 || age > AnnuityCommencementAge) &&  contractValue > 0)
            {
                RiderPhase = MGWRRidePhase.WithdrawPhase;
            }

            else if (RiderPhase == MGWRRidePhase.WithdrawPhase && contractValue == 0)
            {
                RiderPhase = MGWRRidePhase.AutomaticPeriodicBenefitStatus;
            }

            else if (RiderPhase == MGWRRidePhase.AutomaticPeriodicBenefitStatus)
            {
                RiderPhase = MGWRRidePhase.AutomaticPeriodicBenefitStatus;
            }

            else
            {
                throw new Exception("Condition not handled by UpdateRiderBase has been reached");
            }
           
        }

    }
}
