using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class VariableAnnuity
    {
        public DateTime ContractDate { get; protected set; }
        public int ContractYear { get; protected set; }
        public DateTime AnnuityStartDate { get; protected set; }
        public BasePolicyHolder ContractOwner { get; protected set; }

        public BasePolicyHolder Annuiant { get; protected set; }

        public BaseFundsPortfolio Funds { get; protected set; }

        public List<IRider> Riders { get; protected set; }

        public double MortalityExpenseRiskCharge { get; protected set; }

        public double FundFees { get; protected set; }



        public VariableAnnuity(DateTime contractDate, DateTime annuityStartDate, BasePolicyHolder contractOwner, BasePolicyHolder annuiant, BaseFundsPortfolio funds, double mortalityExpenseRiskCharge, double fundFees)
        {
            ContractYear = 0;
            ContractDate = contractDate;
            AnnuityStartDate = annuityStartDate;
            ContractOwner = contractOwner;
            Annuiant = annuiant;
            Funds = funds;
            MortalityExpenseRiskCharge = mortalityExpenseRiskCharge;
            FundFees = fundFees;
        
        }

        public void AdvanceYear()
        {
            ContractYear+= 1;
            ContractOwner.IncrementAge(1);
            if (!Object.ReferenceEquals(ContractOwner, Annuiant))
            {
                Annuiant.IncrementAge(1);
            }

            foreach (IRider rider in Riders) 
            {
                rider.AdvanceYear();
            }
        }

        public double GetContractValue()
        {
            return Funds.GetPortfolioAmount();
        }

        public double GetFeeAmount()
        {
            return GetContractValue() * (MortalityExpenseRiskCharge + FundFees);
        }

        public double GetRiderChargeAmount()
        {
            return GetContractValue() * (from rider in Riders select rider.GetRiderChargeRate()).ToArray().Sum();
        }



    }
}
