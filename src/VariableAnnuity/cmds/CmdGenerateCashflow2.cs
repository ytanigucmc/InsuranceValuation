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
using VariableAnnuity.modelCore.VariableAnnuity;

namespace VariableAnnuity
{
    internal class VariableAnnuityMain2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter output directory: ");
            //string output_dir = Console.ReadLine();
            string output_dir = "D:\\variable_annuity\\output";
            string config_file = args[0];
            VariableAnnuityConfig config = new VariableAnnuityConfig(config_file);
            int aaa = config.GetStepUpPeriod();

            double riskFreeRate = 0.03;
            double volatility = 0.16;
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
            BaseReturnGenerator fund1ReturnGenerator = new FixedReturnGenerator(config.GetRiskFreeRate());
            //BaseReturnGenerator fund2ReturnGenerator = new CustomLogNormalReturnCalculator(config.GetRiskFreeRate(), config.GetVolatility(), randomNumberGenerator);
            BaseReturnGenerator fund2ReturnGenerator = new ReturnCalculatorFromList(ReturnsSaved.Returns);
            List<BaseReturnGenerator> returnGenerators = new List<BaseReturnGenerator>() { fund1ReturnGenerator, fund2ReturnGenerator };
            IFund fund1 = new Fund("Fund1", 0.8 * initialPremium * config.GetTargetFixedFundAllocation());
            IFund fund2 = new Fund("Fund2", 0.8 * initialPremium * (1 - config.GetTargetFixedFundAllocation()));
            BaseFundsPortfolio funds = new FundsPortfolio("AV", new List<IFund> { fund1, fund2 });

            (List<double>, List<double>) withdrawMax = config.GetMaxAnnualWithdraw();
            IInterpolation maxWithdrawRates = new StepInterpolation(withdrawMax.Item1.ToArray(), withdrawMax.Item2.ToArray().ToArray());
            IInterpolation simulationWithdrawRates = new StepInterpolation(new double[] { 0, 71, 76, 80 }, new double[] { 0, 0.05, 0.06, 0.07 });
            BaseDeathBenefitRider returnOfPremiumDeathRider = new ReturnOfPremiumDeathBenefitRider(config.GetInitialPremium(), 0);
            BaseDeathBenefitRider lifePlusDeathRider = new LifePayPlusDeathBenefitRider(config.GetInitialPremium(), 0);
            BaseMGWBRider lifePlusMGWBRider = new LifePayPlusMGWBRider(config.GetInitialPremium(), config.GetRiderChargeRate(), config.GetStepUp(), config.GetStepUpPeriod(), config.GetAnnuityCommencementAge(), maxWithdrawRates, config.GetLastDeathAge());
            var deathRiders = new List<BaseDeathBenefitRider>(){ returnOfPremiumDeathRider, lifePlusDeathRider};

            BasePolicyHolder policyHolder = new PolicyHolder(config.GetStartingAge());
            BasePolicyHolder annuiant = policyHolder;
            ILifePayPlusVariableAnnuity annuity = new LifePayPlusVariableAnnuity(config.GetContractDate(), policyHolder, config.GetAnnuityCommencementAge(), annuiant, config.GetMEFee(), config.GetFundFees(), funds, lifePlusMGWBRider, deathRiders);
            BasePolicyHolderInterpolator mortalityTable = new ConstantPolicyHolderInterpolator(mortalityRate);
            WithdrawlStrategy withdrawStrategy = new WithdrawlStrategy(annuity, config.GetWithdrawlRate(), config.GetFirstWithdrawAge(), config.GetLastDeathAge());

            LifePlusVACashflowGenerationEngine2 simulationEngine = new LifePlusVACashflowGenerationEngine2(annuity, returnGenerators, simulationWithdrawRates, mortalityTable, withdrawStrategy);
            DataTable cashflowRecords = simulationEngine.GenerateCashflowRecords();

            IDiscountCurve discountCurve = new FlatRateDiscountCurve(riskFreeRate);
            PVCalculationEngine PVEngine = new PVCalculationEngine(discountCurve);
            List<string> headerPVCalculation = new List<string>() { "NAR Death Claims", "Withdrawl Claims", "Rider Charges" };
            List<string> newHeaderPVCalculation = new List<string>() { "PV_DB_Claim", "PV_WB_Claim", "PV_RC" };
            DataTable PVs = PVEngine.FromDataTable(cashflowRecords, "year", headerPVCalculation, newHeaderPVCalculation);
            cashflowRecords.ToCSV(Path.Join(output_dir, "Cashflows2.csv"));
            PVs.ToCSV(Path.Join(output_dir, "PVCalculation2.csv"));

        }

    }
}
