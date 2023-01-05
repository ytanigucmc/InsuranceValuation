using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public static class CashFlowHeaders
    {
        public static string Year = "Year";
        public static string Anniversary = "Anniversary";
        public static string Age = "Age";
        public static string Contribution = "Contribution";
        public static string PreFee = "PORT Pre-Fee";
        public static string Fees = "M&E/Fund Fees";
        public static string PreWithdrawl = "PORT Pre-Withdrawl";
        public static string WithdrawalAmount = "Withdrawal Amount";
        public static string PostWithdrawal = "PORT Post-Withdrawal";
        public static string RiderCharge = "Rider Charge";
        public static string PostCharges = "PORT Post-Charges";
        public static string DeathPayments = "Death Payments";
        public static string PostDeathClaims = "PORT Post-Death Claims";
        public static string PostRebalance = "PORT Post-Rebalance";
        public static string ROPDeathBase = "ROP Death Base";
        public static string NARDeathClaims = "NAR Death Claims";
        public static string DeathBenefitBase = "Death Benefit Base";
        public static string WithdrawalBase = "Withdrawal Base";
        public static string RiderWithdrawalAmount = "Withdrawal Amount_";
        public static string CumulativeWithdrawl = "Cumulative Withdrawl";
        public static string MaximumAnnualWithdrawal = "Maximum Annual Withdrawal";
        public static string MaximumAnnualWithdrawalRate = "Maximum Annual Withdrawal Rate";
        public static string EligibleStepUp = "Eligible Step-Up";
        public static string GrowthPhase = "Growth Phase";
        public static string WithdrawalPhase = "Withdrawal Phase";
        public static string AutomaticPeriodicBenefitStatus = "AutomaticPeriodicBenefitStatus";
        public static string LastDeath = "Last Death";
        public static string Return = "FUND Return";
        public static string RebalanceIndicator = "Rebalance Indicator";
        public static string QX = "qx";

        public static string PortfolioKey = "PORT";
        public static string FundKey = "FUND";

    }

    public static class PVHeaders
    {
        public static string DF = "DF";
        public static string DeathClaims = "Death Claims";
        public static string WithdrawalClaims = "Withdrawal Claims";
        public static string RiderCharges = "Rider Charges";
    }

    public class CashflowRecord
    {
        public int Year;
        public DateTime Anniversary;
        public int Age;
        public double PreFee;
        public double Fees;
        public double WithdrawalAmount;
    }
}
