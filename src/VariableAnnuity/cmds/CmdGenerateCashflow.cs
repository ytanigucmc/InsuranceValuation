using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.configs;
using VariableAnnuity.modelCore.CashflowGenerationEngine;
using VariableAnnuity.modelCore.PresentValueCalculator;

namespace VariableAnnuity
{
    internal class VariableAnnuityMain
    {
        static void Main(string[] args)
        {
            Console.WriteLine("In main now");
            string config_file = args[0];
            VariableAnnuityConfig config = new VariableAnnuityConfig(config_file);

            double riskFreeRate = 0.03;
            double volatility = 0.11;
            double initialPremium = 100000;
            double riderChargeRate = 0.0085;
            double stepUpRate = 0.06;
            int stepUpPeriod = 10;
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
            BaseReturnGenerator fund1ReturnGenerator = new FixedReturnGenerator(riskFreeRate);
            //BaseReturnGenerator fund2ReturnGenerator = new CustomLogNormalReturnCalculator(drift, volatility, randomNumberGenerator);
            BaseReturnGenerator fund2ReturnGenerator = new ReturnCalculatorFromList(ReturnsSaved.Returns);
            List<BaseReturnGenerator> returnGenerators = new List<BaseReturnGenerator>() { fund1ReturnGenerator, fund2ReturnGenerator };
            IFund fund1 = new Fund("Fund1", 0.8 * initialPremium * targetWeights[0]);
            IFund fund2 = new Fund("Fund2", 0.8 * initialPremium * targetWeights[1]);
            BaseFundsPortfolio funds = new FundsPortfolio("AV", new List<IFund> { fund1, fund2 });

            IInterpolation maxWithdrawRates = new StepInterpolation(new double[] { 0, 59.6, 65, 76, 80 }, new double[] { 0, 0.04, 0.05, 0.06, 0.07 });
            IInterpolation simulationWithdrawRates = new StepInterpolation(new double[] { 0, 71, 76, 80 }, new double[] { 0, 0.05, 0.06, 0.07 });
            BaseRider returnOfPremiumDeathRider = new ReturnOfPremiumDeathBenefitRider(initialPremium, 0);
            BaseRider lifePlusDeathRider = new LifePayPlusDeathBenefitRider(initialPremium, 0);
            BaseRider lifePlusMGWBRider = new LifePayPlusMGWBRider(initialPremium, riderChargeRate, stepUpRate, stepUpPeriod ,annuityCommenceAge, maxWithdrawRates);
            List<BaseRider> riders = new List<BaseRider> { returnOfPremiumDeathRider, lifePlusDeathRider, lifePlusMGWBRider };

            BasePolicyHolder policyHolder = new PolicyHolder(60);
            BasePolicyHolder annuiant = policyHolder;
            BaseVariableAnnuity annuity = new VariableAnnuity(contractDate, policyHolder, annuityCommenceAge, annuiant, mortalityRiskCharge, fundFees, funds, riders);
            BasePolicyHolderInterpolator mortalityTable = new ConstantPolicyHolderInterpolator(0.005);

            int startYear = policyHolder.GetAge();
            LifePlusVACashflowGenerationEngine simulationEngine = new LifePlusVACashflowGenerationEngine(annuity, returnGenerators, simulationWithdrawRates, mortalityTable);
            DataTable cashflowRecords = simulationEngine.GenerateCashflowRecords();
            var a = cashflowRecords.Rows[0]["year"];

            IDiscountCurve discountCurve = new FlatRateDiscountCurve(riskFreeRate);
            List<int> years = cashflowRecords.AsEnumerable().Select(item => item.Field<int>("year")).ToList();
            List<double> deathClaims = cashflowRecords.AsEnumerable().Select(item => item.Field<double>("Death Claims")).ToList();
            List<double> riderCharges = cashflowRecords.AsEnumerable().Select(item => item.Field<double>("RiderCharged")).ToList();
        }

    }
}
