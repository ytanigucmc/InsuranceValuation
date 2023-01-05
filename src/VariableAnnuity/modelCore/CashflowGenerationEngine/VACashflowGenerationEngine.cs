﻿using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.modelCore.Rider;

namespace VariableAnnuity.modelCore.CashflowGenerationEngine
{
    public class LifePlusVACashflowGenerationEngine : BaseVACashflowGenerationEngine
    {
        IInterpolation WithdrawlScheule;
        BasePolicyHolderInterpolator MortalityTable;
        public VariableAnnuityCashflowRecorder recorder;
        private double contractValueLastYear;
        private List<BaseRiderBaseComputable> deathBenefitRiders;
        private LifePayPlusMGWBRider MGWBRider;
        private ReturnOfPremiumDeathBenefitRider RoPDeathBenefitRider;
        private LifePayPlusDeathBenefitRider LifePlusDeathBenefitRider;
        private bool RebalanceIndicator;
        private double DeathPaymentBase;
        private double DeathPaymentAmount;
        private double PortValuePostDeathPayment;
        private double WithdrawlAmount;
        private double MaxWithdrawlAmount;
        private double MaxWithdrawlRate;
        private double CumulativeWithdrawl;
        private double DeathClaimAmount;
        private double FeeAmount;
        private List<double> fundsReturns;

        public LifePlusVACashflowGenerationEngine(BaseVariableAnnuity annuity, List<BaseReturnGenerator> returnGenerators, IInterpolation withdrawlSchedule, BasePolicyHolderInterpolator mortalityTable):base(annuity, returnGenerators)
        {
            WithdrawlScheule = withdrawlSchedule;
            MortalityTable = mortalityTable;
            recorder = new VariableAnnuityCashflowRecorder();
            contractValueLastYear = Annuity.GetContractValue();
            deathBenefitRiders = (from rider in annuity.Riders where rider.GetRiderTypeName() == RiderTypeNames.DeathBenefit select (BaseRiderBaseComputable)rider).ToList();
            RoPDeathBenefitRider = (ReturnOfPremiumDeathBenefitRider)(from rider in annuity.Riders where rider.GetRiderName() == "ReturnOfPremiumDeathBenefitRider" select rider).ToList()[0];
            LifePlusDeathBenefitRider = (LifePayPlusDeathBenefitRider)(from rider in annuity.Riders where rider.GetRiderName() == "LifePayPlusDeathBenefitRider" select rider).ToList()[0];
            MGWBRider = (LifePayPlusMGWBRider)(from rider in annuity.Riders where rider.GetRiderTypeName() == RiderTypeNames.MGWB select rider).ToList()[0];
            RebalanceIndicator = MGWBRider.RebalanceIndiactor;
        }

        public override DataTable GenerateCashflowRecords()
        {
            List<double> targetWeights = new List<double> { 0.2, 0.8 };

            int year = 0;
            int lastDeathAge = 100;
            for (int age = Annuity.ContractOwner.GetAge(); age <= lastDeathAge; age++, year++)
            {

                if (year == 0)
                {
                    continue;
                }

                FeeAmount = CalculateFeesAmount();
                fundsReturns = GetFundReturns();
                ApplyFundReturns(fundsReturns);
                AgeContractByOneYear();
                RecordDateAge();
                DiscountRiderBases();
                MakeContribution(0);
                PayFees(FeeAmount);
                MakeWithdrawl();
                PayRiderCharge();
                MakeDeathPayments();
                RebalanceFunds(targetWeights);
                UpdateOnAnniversaryReached();
                RecordRidersInfo();
                recorder.PushCurrentRecord();
            }

            DataTable dt = recorder.ToDataTale();
            dt.ToCSV("D:\\variable_annuity\\output\\output.csv");
            return dt;
        }


        private List<double> GetFundReturns()
        {
            return (from generator in ReturnGenerators select generator.GetReturn()).ToList();
        }

        private void ApplyFundReturns(List<double> fundReturns)
        {
            Annuity.ApplyFundsReturns(fundReturns);
        }

        private void RecordDateAge()
        {
            recorder.AddElement("Year", Annuity.ContractYear);
            recorder.AddDateTime("Anniversary", Annuity.LastAnniversaryDate);
            recorder.AddElement("Age", Annuity.ContractOwner.GetAge());

            //recordHolder.Add(CashFlowHeaders.Year, Annuity.ContractYear);
            //recordHolder.Add(CashFlowHeaders.Anniversary, Annuity.LastAnniversaryDate);
            //recordHolder.Add(CashFlowHeaders.Age, Annuity.ContractOwner.GetAge());
        }

        private double CalculateFeesAmount()
        {
            return Math.Max(Annuity.GetFeeAmount() - DeathPaymentAmount * (Annuity.FundFees + Annuity.MortalityExpenseRiskCharge), 0);
        }

        private void AgeContractByOneYear()
        {
            Annuity.AgeContractByOneYear();
            DeathPaymentBase = (from rider in deathBenefitRiders select ((IBaseComputable)rider).GetBaseAmount()).Max();

        }

        private void DiscountRiderBases()
        {
            Annuity.DeductPerentageAmountRiderBases(MortalityTable.Interpolate(Annuity.ContractOwner));
        }

        private void MakeContribution(double contribution)
        {
            Annuity.ContributeDollarAmount(contribution);
            recorder.AddElement("Contribution", contribution);
            recorder.AddFundsData(Annuity, "Pre-Fee");

            //recordHolder.Add(CashFlowHeaders.Contribution, contribution);
            //recordHolder.AddFundsData(Annuity, "Pre-Fee");
        }


        private void PayFees(double fundFeesAmount)
        {
            Annuity.PayFee(fundFeesAmount);
            recorder.AddElement("M&E/Fund Fees", fundFeesAmount);
            recorder.AddFundsData(Annuity, "Pre-Withdrawl");

            //recordHolder.Add(CashFlowHeaders.Fees, fundFeesAmount);
            //recordHolder.AddFundsData(Annuity, "Pre-Withdrawl");
        }


        private void MakeWithdrawl()
        {
            WithdrawlAmount = WithdrawAmountStrategy();
            CumulativeWithdrawl +=  WithdrawlAmount;
            MaxWithdrawlRate = MGWBRider.RiderPhase == MGWRRidePhase.GrowthPhase ? 0 :MGWBRider.MaximumAnnualWithdrawl.Interpolate(Annuity.ContractOwner.GetAge());
            MaxWithdrawlAmount = MGWBRider.GetBaseAmount() * MaxWithdrawlRate;
            Annuity.WithdrawDollarAmount(WithdrawlAmount);
            recorder.AddElement("Withdrawal Amount", WithdrawlAmount);
            recorder.AddFundsData(Annuity, "Post-Withdrawl");

            //recordHolder.Add(CashFlowHeaders.WithdrawalAmount, WithDrawlAmount);
            //recordHolder.AddFundsData(Annuity, "Post-Withdrawl");

        }

        private void PayRiderCharge()
        {
            double riderCharge = Annuity.CalculateRiderChargeAmount();
            Annuity.PayRiderCharge(riderCharge);
            recorder.AddElement("Rider Charges", riderCharge);
            recorder.AddFundsData(Annuity, "Post-Charges");
        }
        
        private void MakeDeathPayments()
        {
            DeathPaymentAmount = DeathPaymentBase * MortalityTable.Interpolate(Annuity.ContractOwner);
            DeathClaimAmount = Math.Max(DeathPaymentAmount - Annuity.GetContractValue(), 0);
            Annuity.Funds.DeductDollarAmount(DeathPaymentAmount);
            PortValuePostDeathPayment = Annuity.GetContractValue();
            recorder.AddElement("Death Payments", DeathPaymentAmount);
            recorder.AddFundsData(Annuity, "Post-Death Claims");
        }

        private void RebalanceFunds(List<double> targetWeights)
        {
            if (MGWBRider.RebalanceIndiactor)
            {
                Annuity.RebalanceFunds(targetWeights);
            }

            
            if (Annuity.GetContractValue() > 0)
            {
                Annuity.Funds.AddDollarAmount(1, DeathPaymentAmount);
            }
            recorder.AddFundsData(Annuity, "Post-Rebalance");
            contractValueLastYear = Annuity.GetContractValue();
        }

        private void UpdateOnAnniversaryReached()
        {
            Annuity.UpdateOnAnniversaryReached();
            DeathPaymentBase = Math.Max(RoPDeathBenefitRider.GetBaseAmount(), LifePlusDeathBenefitRider.GetBaseAmount());
        }

        private void RecordRidersInfo()
        {

            recorder.AddElement("ROP Death Base", RoPDeathBenefitRider.GetBaseAmount());
            recorder.AddElement("NAR Death Claims", DeathClaimAmount);
            recorder.AddElement("Death Benefit base", LifePlusDeathBenefitRider.GetBaseAmount());
            recorder.AddElement("Withdrawal Base", MGWBRider.GetBaseAmount());
            recorder.AddElement("Withdrawal Amount_", WithdrawlAmount);
            recorder.AddElement("Cumulative Withdrawal", CumulativeWithdrawl);
            recorder.AddElement("Death Claims", Math.Max(WithdrawlAmount - PortValuePostDeathPayment, 0));
            recorder.AddElement("Maximum Annual Withdrawal", MaxWithdrawlAmount);
            recorder.AddElement("Maximum Annual Withdrawal Rate", MaxWithdrawlRate);
            recorder.AddLifePlusPhaseIndicators(MGWBRider);
            recorder.AddFundsReturn(Annuity, fundsReturns);
            recorder.AddBoolAsOneZeoro("Rebalance Indicatot", MGWBRider.RebalanceIndiactor);
            recorder.AddElement("qx", MortalityTable.Interpolate(Annuity.ContractOwner));
        }

        private double WithdrawAmountStrategy()
        {
            if (MGWBRider.RiderPhase == MGWRRidePhase.WithdrawPhase)
            {
                return MGWBRider.GetBaseAmount() * 0.03;
            }

            else if (MGWBRider.RiderPhase == MGWRRidePhase.AutomaticPeriodicBenefitStatus)
            {
                return MGWBRider.GetBaseAmount() * WithdrawlScheule.Interpolate(Annuity.ContractOwner.GetAge());
            }

            else { return 0; }         
        }
    }
}
