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

            string config_file = args[0];
            VariableAnnuityConfig config = new VariableAnnuityConfig(config_file);
            string output_dir = config.GetOutputDir();


            double initialPremium = config.GetInitialPremium();
            BaseRandomNumberGenerator randomNumberGenerator = new RandomNormalGenerator();
            BaseReturnGenerator fund1ReturnGenerator = new FixedReturnGenerator(config.GetRiskFreeRate());

            BaseReturnGenerator fund2ReturnGenerator;
            if (config.IsFixRandom())
            {
                fund2ReturnGenerator = new ReturnCalculatorFromList(config.ParseRandomReturns());              
            }
            else
            {
                fund2ReturnGenerator = new CustomLogNormalReturnCalculator(config.GetRiskFreeRate(), config.GetVolatility(), randomNumberGenerator);
            }


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
            BasePolicyHolderInterpolator mortalityTable = new ConstantPolicyHolderInterpolator(config.GetMortalityRate());
            WithdrawlStrategy withdrawStrategy = new WithdrawlStrategy(annuity, config.GetWithdrawlRate(), config.GetFirstWithdrawAge(), config.GetLastDeathAge());

            LifePlusVACashflowGenerationEngine2 simulationEngine = new LifePlusVACashflowGenerationEngine2(annuity, returnGenerators, simulationWithdrawRates, mortalityTable, withdrawStrategy);
            DataTable cashflowRecords = simulationEngine.GenerateCashflowRecords();

            IDiscountCurve discountCurve = new FlatRateDiscountCurve(config.GetRiskFreeRate());
            PVCalculationEngine PVEngine = new PVCalculationEngine(discountCurve);
            List<string> headerPVCalculation = new List<string>() { "NAR Death Claims", "Withdrawl Claims", "Rider Charges" };
            List<string> newHeaderPVCalculation = new List<string>() { "PV_DB_Claim", "PV_WB_Claim", "PV_RC" };
            DataTable PVs = PVEngine.FromDataTable(cashflowRecords, "year", headerPVCalculation, newHeaderPVCalculation);
            cashflowRecords.ToCSV(Path.Join(output_dir, "Cashflows3.csv"));
            PVs.ToCSV(Path.Join(output_dir, "PVCalculation5.csv"));

        }

    }
}
