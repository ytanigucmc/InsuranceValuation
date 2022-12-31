using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class RecordNames
    {
        public static string Year = "Year";
        public static string Anniversary = "Anniversary";
        public static string Age = "Age";
        public static string PreFee = "Pre-Fee";
        public static string Fees = "M&E/Fund Fees";
        public static string WithdrawalAmount = "Withdrawal Amount";
        public static string PostWithdrawal = "Post-Withdrawal";
        public static string RiderCharge = "Rider Charge";
        public static string PostCharges = "Post-Charges";
        public static string DeathPayments = "Death Payments";
        public static string PostDeathClaims = "Post-Death Claims";
        public static string PostRebalance = "Post-Rebalance";
        public static string WithdrawalBase = "Withdrawal Base";
        public static string RiderWithdrawalAmount = "Withdrawal Amount";
        public static string CumulativeWithdrawl = "Cumulative Withdrawl";
        public static string MaximumAnnualWithdrawal = "Maximum Annual Withdrawal";
        public static string MaximumAnnualWithdrawalRate = "Maximum Annual Withdrawal Rate";
        public static string EligibleStepUp = "Eligible Step-Up";
        public static string WithdrawalPhase = "Withdrawal Phase";
        public static string AutomaticPeriodicBenefitStatus = "AutomaticPeriodicBenefitStatus";
        public static string LastDeath = "Last Death";
        public static string Return = "Return";
        public static string RebalanceIndicator = "Rebalance Indicator";
        public static string DF = "DF";
        public static string QX = "qx";
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
