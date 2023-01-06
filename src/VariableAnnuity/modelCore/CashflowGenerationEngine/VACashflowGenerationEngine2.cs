using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class LifePlusVACashflowGenerationEngine2 : BaseVACashflowGenerationEngine2
    {
        private BasePolicyHolderInterpolator MortalityTable;
        private WithdrawlStrategy WithdrawlStrat;
        public LifePayPlusRecorder recorder;
        private LifePayPlusMGWBRider MGWBRider;
        private double FeesBase;
        private double DeathPaymentBase;
        private double DeathPaymentAmount;
        private double LastPortValuePostDeathPayment;
        private double ThisPortValuePostDeathPayment;
        private double WithdrawlAmount;
        private double MaxWithdrawlAmount;
        private double MaxWithdrawlRate;
        private double CumulativeWithdrawl;
        private double DeathClaimAmount;
        private List<double> fundsReturns;

        public LifePlusVACashflowGenerationEngine2(ILifePayPlusVariableAnnuity annuity, List<BaseReturnGenerator> returnGenerators, IInterpolation withdrawlSchedule, BasePolicyHolderInterpolator mortalityTable, WithdrawlStrategy withdrawlStrat) : base(annuity, returnGenerators)
        {
            MortalityTable = mortalityTable;
            recorder = new LifePayPlusRecorder();
            MGWBRider = (LifePayPlusMGWBRider)annuity.WithdrawlRider;
            FeesBase = Annuity.GetContractValue();
            WithdrawlStrat = withdrawlStrat;
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

                fundsReturns = GetFundReturns();
                ApplyFundReturns(fundsReturns);
                AgeContractByOneYear();
                RecordDateAge();
                DiscountRiderBases();
                MakeContribution(0);
                PayFees(FeesBase);
                MakeWithdrawl();
                PayRiderCharge();
                MakeDeathPayments();
                RebalanceFunds(targetWeights);
                UpdateOnAnniversaryReached();
                RecordRidersInfo();
                recorder.PushCurrentRecord();
            }

            DataTable dt = recorder.ToDataTale();

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
        }

        private void AgeContractByOneYear()
        {
            Annuity.AgeContractByOneYear();
            DeathPaymentBase = Annuity.GetDeathBenefitBases().Max(); 
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
        }


        private void PayFees(double fBase)
        {
            double fundFeesAmount = Annuity.GetFeeAmount(fBase);
            Annuity.PayFee(fundFeesAmount);
            recorder.AddElement("M&E/Fund Fees", fundFeesAmount);
            recorder.AddFundsData(Annuity, "Pre-Withdrawl");
        }


        private void MakeWithdrawl()
        {
            WithdrawlAmount = WithdrawlStrat.GetWithdrawlAmount();
            CumulativeWithdrawl += WithdrawlAmount;
            MaxWithdrawlRate = Annuity.GetMaximumWithdrawlAllowance();
            MaxWithdrawlAmount = Annuity.GetMaximumWithdrawlAllowance();

            Annuity.WithdrawDollarAmount(WithdrawlAmount);
            recorder.AddElement("Withdrawal Amount", WithdrawlAmount);
            recorder.AddFundsData(Annuity, "Post-Withdrawl");
        }

        private void PayRiderCharge()
        {
            double riderCharge = Annuity.GetRiderChargeAmount();
            Annuity.PayRiderCharge(riderCharge);
            recorder.AddElement("Rider Charges", riderCharge);
            recorder.AddFundsData(Annuity, "Post-Charges");
        }

        private void MakeDeathPayments()
        {
            DeathPaymentAmount = DeathPaymentBase * MortalityTable.Interpolate(Annuity.ContractOwner);
            DeathClaimAmount = Math.Max(DeathPaymentAmount - Annuity.GetContractValue(), 0);
            Annuity.TakeDeathPayment(DeathPaymentAmount);
            LastPortValuePostDeathPayment = ThisPortValuePostDeathPayment;
            ThisPortValuePostDeathPayment = Annuity.GetContractValue();
            FeesBase = Annuity.GetContractValue();
            recorder.AddElement("Death Payments", DeathPaymentAmount);
            recorder.AddFundsData(Annuity, "Post-Death Claims");
        }

        private void RebalanceFunds(List<double> targetWeights)
        {

            Annuity.RebalanceFunds(targetWeights);
            recorder.AddFundsData(Annuity, "Post-Rebalance");
        }

        private void UpdateOnAnniversaryReached()
        {
            Annuity.UpdateOnAnniversaryReached();
        }

        private void RecordRidersInfo()
        {

            recorder.AddElement("ROP Death Base", Annuity.GetDeathBenefitBases()[0]);
            recorder.AddElement("NAR Death Claims", DeathClaimAmount);
            recorder.AddElement("Death Benefit base", Annuity.GetDeathBenefitBases()[1]);
            recorder.AddElement("Withdrawal Base", Annuity.GetWtihdrawlBase());
            recorder.AddElement("Withdrawal Amount_", WithdrawlAmount);
            recorder.AddElement("Cumulative Withdrawal", CumulativeWithdrawl);
            recorder.AddElement("Maximum Annual Withdrawal", MaxWithdrawlAmount);
            recorder.AddElement("Maximum Annual Withdrawal Rate", MaxWithdrawlRate);
            recorder.AddElement("Withdrawl Claims", Math.Max(WithdrawlAmount - LastPortValuePostDeathPayment, 0));
            recorder.AddLifePlusPhaseIndicators(MGWBRider);
            recorder.AddFundsReturn(Annuity, fundsReturns);
            recorder.AddBoolAsOneZeoro("Rebalance Indicator", Annuity.IsRebalance());
            recorder.AddElement("qx", MortalityTable.Interpolate(Annuity.ContractOwner));
        }
    }
}
