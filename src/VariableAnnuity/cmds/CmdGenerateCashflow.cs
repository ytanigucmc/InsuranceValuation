using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.configs;

namespace VariableAnnuity
{
    internal class VariableAnnuityMain
    {
        static void Main(string[] args)
        {
            Console.WriteLine("In main now");
            string config_file = args[0];
            VariableAnnuityConfig config = new VariableAnnuityConfig(config_file);

            double drift = 0.03;
            double volatility = 0.11;
            double initialPremium = 100000;
            double riderChargeRate = 0.0085;
            double stepUpRate = 0.06;
            int annuityCommenceAge = 70;
            double mortalityRiskCharge = 0.014;
            double fundFees = 0.0015;
            DateTime contractDate = new DateTime(2016, 8, 1);
            List<double> targetWeights = new List<double> { 0.2, 0.8 };
            int lastDeathAge = 100;
            double mortalityRate = 0.005;
            double contribution = 0;
            double defaultWithdrawRate = 0.03;

            BaseRandomNumberGenerator randomNumberGenerator = new RandomNormalGenerator();
            BaseReturnGenerator fund1ReturnGenerator = new FixedReturnGenerator(drift);
            BaseReturnGenerator fund2ReturnGenerator = new CustomLogNormalReturnCalculator(drift, volatility, randomNumberGenerator);
            IFund fund1 = new Fund("Fund1", 0.8 * initialPremium * targetWeights[0], fund1ReturnGenerator);
            IFund fund2 = new Fund("Fund2", 0.8 * initialPremium * targetWeights[1], fund2ReturnGenerator);
            BaseFundsPortfolio funds = new FundsPortfolio("AV", new List<IFund> { fund1, fund2 });

            IInterpolation maxWithdrawRates = new StepInterpolation(new double[] { 0, 59.6, 65, 76, 80 }, new double[] { 0, 0.04, 0.05, 0.06, 0.07 });
            IInterpolation simulationWithdrawRates = new StepInterpolation(new double[] { 0, 71, 76, 80 }, new double[] { 0, 0.05, 0.06, 0.07 });
            BaseRider returnOfPremiumDeathRider = new ReturnOfPremiumDeathBenefitRider(initialPremium, 0);
            BaseRider lifePlusDeathRider = new LifePayPlusDeathBenefitRider(initialPremium, 0);
            BaseRider lifePlusMGWBRider = new MGWBRider(initialPremium, riderChargeRate, stepUpRate, annuityCommenceAge, maxWithdrawRates);
            List<BaseRider> riders = new List<BaseRider> { returnOfPremiumDeathRider, lifePlusDeathRider, lifePlusMGWBRider };

            BasePolicyHolder policyHolder = new PolicyHolder(60);
            BasePolicyHolder annuiant = policyHolder;
            BaseVariableAnnuity annuity = new VariableAnnuity(contractDate, policyHolder, annuityCommenceAge, annuiant, mortalityRiskCharge, fundFees, funds, riders);


            int startYear = policyHolder.GetAge();

            VariableAnnuityCashflowRecorder recorder = new VariableAnnuityCashflowRecorder();

            double cumulativeWithdraw = 0;
            for (int year = startYear; year != lastDeathAge; year++)
            {
                if (year == startYear)
                {
                    continue;
                }

                double fundFeesAmount = annuity.CalculateFeeAmount();

                annuity.AgeContractByOneYear();
                recorder.AddElement<double>("Contribution", contribution);
                recorder.AddFundsData(annuity, "Pre-Fee");
               


                double deathPaymentAmount = Math.Max(((IBaseComputable)lifePlusDeathRider).GetBaseAmount(), ((IBaseComputable)lifePlusMGWBRider).GetBaseAmount()) * mortalityRate;

                annuity.DeductPerentageAmountRiderBases(mortalityRate);
                
                annuity.ContributeDollarAmount(contribution);

                annuity.PayFee(fundFeesAmount);
                recorder.AddElement<double>("M&E/Fund Fees", fundFeesAmount);
                recorder.AddFundsData(annuity, "Pre-Withdrawl");

                double withdrawAmount = ((IBaseComputable)lifePlusMGWBRider).GetBaseAmount()  * simulationWithdrawRates.Interpolate(annuity.ContractOwner.GetAge());
                annuity.WithdrawDollarAmount(withdrawAmount);
                cumulativeWithdraw += withdrawAmount;
                recorder.AddElement<double>("Withdrawal Amount", withdrawAmount);
                recorder.AddFundsData(annuity, "Post-Withdrawl");

                double riderCharge = annuity.CalculateRiderChargeAmount();
                annuity.PayRiderCharge(riderCharge);
                recorder.AddElement<double>("Rider Charges", riderCharge);
                recorder.AddFundsData(annuity, "Post-Charges");


                annuity.Funds.DeductDollarAmount(deathPaymentAmount);
                recorder.AddElement<double>("Death Payments", deathPaymentAmount);
                recorder.AddFundsData(annuity, "Post-Death Claims");

                annuity.RebalanceFunds(targetWeights);
                recorder.AddFundsData(annuity, "Post-Rebalance");

                annuity.UpdateOnAnniversaryReached();
                recorder.AddElement<double>("ROP Death Base", ((IBaseComputable)returnOfPremiumDeathRider).GetBaseAmount());


                recorder.AddElement<double>("Death Benefit base", ((IBaseComputable)lifePlusDeathRider).GetBaseAmount());
                recorder.AddElement<double>("Withdrawal Base", ((IBaseComputable)lifePlusMGWBRider).GetBaseAmount());
                recorder.AddElement<double>("Withdrawal Amount", withdrawAmount);
                recorder.AddElement<double>("Cumulative Withdrawal", cumulativeWithdraw);
                recorder.AddLifePlusPhaseIndicators((MGWBRider)lifePlusMGWBRider);
                double aaa = 1;
            }
        }

    }
}
