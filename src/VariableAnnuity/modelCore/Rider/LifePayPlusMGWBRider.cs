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


    public class LifePayPlusMGWBRider : BaseMGWBRider, IContributionMadeHandlable, IFeePaidHandlable, IWithdrawMadeHandlable, IRiderChargeHandlable, IAnniversaryReachedHandlable, IContractAgedByOneYearHandlable, IDeathPaymentTakenHandlable
    {

        public double StepUpRate { get; set; }

        public double StepUpPeriod { get; set; }

        public bool StepUpEligibility { get; set; }

        public bool RebalanceIndiactor { get; set; }

        public int AnnuityCommencementAge { get; set; }

        public int DeathAge { get; set; }

        private double CumulativeBaseAdjustment;

        private double CumulativeWithdraw;

        private double CumulativeDeathPayment;




        public LifePayPlusMGWBRider(double baseAmount, double chargeRate, double stepUpRate, int stepUpPeirod ,int annuityCommencementAge, IInterpolation maximumAnnualWithdrawl, int deathAge) : base(baseAmount, chargeRate, maximumAnnualWithdrawl)
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

        public override bool IsAccountDepleted()
        {
            return RiderPhase == MGWRRiderPhase.AutomaticPeriodicBenefitStatus;
        }

        public override void SetPhase(MGWRRiderPhase phase)
        {
            base.SetPhase(phase);
            UpdateRebalanceIndicator();
        }

        public override double GetMaximumWithdrawlAllowance(int age)
        {
            return IsPhase(MGWRRiderPhase.GrowthPhase) ? 0 : base.GetMaximumWithdrawlAllowance(age);
        }

        public override double GetMaximumWithdrawlAllowance(int age, double baseAmount)
        {
            return IsPhase(MGWRRiderPhase.GrowthPhase) ? 0 : base.GetMaximumWithdrawlAllowance( age, baseAmount);
        }

        public override double GetMaximumWithdrawlRate(int age)
        {
            return IsPhase(MGWRRiderPhase.GrowthPhase) ? 0 : base.MaximumAnnualWithdrawl.Interpolate(age);
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

        public void OnDeathPaymentTaken(object source, DollarAmountEventArgs args)
        {
            CumulativeDeathPayment += args.DollarAmount;
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
            CumulativeDeathPayment = 0;
        }

        private void UpdateRiderBase(double contractValue, int age)
        {
            double excessWithdraw = GetWithdrawlExcessAllowance(CumulativeWithdraw, age);
            double currentBaseAdjusted = RiderBase.GetDollarAmount() - excessWithdraw;
            double rachetBase = (contractValue + CumulativeDeathPayment) * Convert.ToInt32(RiderPhase == MGWRRiderPhase.GrowthPhase) ;
            double stepUpeBase = (RiderBase.GetDollarAmount() * (1 + StepUpRate) + CumulativeBaseAdjustment - excessWithdraw) * Convert.ToInt32(StepUpEligibility);
            RiderBase.SetDollarAmount(new double[] { currentBaseAdjusted, rachetBase, stepUpeBase }.Max());
        }

        private void UpdateRebalanceIndicator()
        {
            if (RiderPhase == MGWRRiderPhase.WithdrawPhase || RiderPhase == MGWRRiderPhase.AutomaticPeriodicBenefitStatus)
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
                RiderPhase = MGWRRiderPhase.LastDeath;
            }

            else if (RiderPhase == MGWRRiderPhase.WithdrawPhase && contractValue>0)
            {
                RiderPhase = MGWRRiderPhase.WithdrawPhase;
            }


            else if ((CumulativeWithdraw > 0 || age > AnnuityCommencementAge) &&  contractValue > 0)
            {
                RiderPhase = MGWRRiderPhase.WithdrawPhase;
            }

            else if (CumulativeWithdraw == 0 && age <= AnnuityCommencementAge)
            {
                RiderPhase = MGWRRiderPhase.GrowthPhase;
            }

            else if (RiderPhase == MGWRRiderPhase.WithdrawPhase && contractValue == 0)
            {
                RiderPhase = MGWRRiderPhase.AutomaticPeriodicBenefitStatus;
            }

            else if (RiderPhase == MGWRRiderPhase.AutomaticPeriodicBenefitStatus)
            {
                RiderPhase = MGWRRiderPhase.AutomaticPeriodicBenefitStatus;
            }

            else
            {
                throw new Exception("Condition not handled by UpdateRiderBase has been reached");
            }
           
        }

        public override bool IsRebalance()
        {
            return RebalanceIndiactor;
        }

    }
}
