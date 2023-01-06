﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity.modelCore.VariableAnnuity.EventHandlerInterfaces;

namespace VariableAnnuity
{
    public class VariableAnnuity2 : BaseVariableAnnuity
    {
        public double BaseValuePostDeathPayment { get; set; }
        public double BaseValueRiderChargeAmount { get; set; }
        public double DeathPaymentsAmount { get; set; }

        public VariableAnnuity2(DateTime contractDate, BasePolicyHolder contractOwner, int annuityStartAge, BasePolicyHolder annuiant, double mortalityExpenseRiskCharge, double fundFees, BaseFundsPortfolio funds, List<BaseRider> riders) :
            base(contractDate, contractOwner, annuityStartAge, annuiant, mortalityExpenseRiskCharge, fundFees, funds, riders)
        {
            BaseValuePostDeathPayment = this.GetContractValue();
        }


        public override double GetContractValue()
        {
            return Funds.GetPortfolioAmount();
        }

        public override double GetFeeAmount()
        {
            return BaseValueRiderChargeAmount;
        }


        public double GetRiderChargeAmount()
        {
            return BaseValueRiderChargeAmount * (from rider in Riders select rider.GetRiderChargeRate()).Sum();
        }


        public override void PayFee(double feeAmount)
        {
            Funds.DeductDollarAmount(feeAmount);
            OnFeePaid(feeAmount);
        }

        public override void PayRiderCharge(double chargeAmont)
        {
            Funds.DeductDollarAmount(chargeAmont);
            OnRiderChargeMade(chargeAmont);
        }


        public override void ContributeDollarAmount(double contributionAmount)
        {
            OnContributionMade(contributionAmount);
        }

        public override void WithdrawDollarAmount(double withdrawAmount)
        {
            Funds.DeductDollarAmount(withdrawAmount);
            OnWithdrawMade(withdrawAmount);
        }

        public override void RebalanceFunds(List<double> targetWeights)
        {
            Funds.Rebalance(targetWeights);
            Funds.AddDollarAmount(1, DeathPaymentsAmount);

        }



        public override void AgeContractByOneYear()
        {

            ContractYear += 1;
            LastAnniversaryDate = LastAnniversaryDate.AddYears(1);
            ContractOwner.IncrementAge(1);
            if (!Object.ReferenceEquals(ContractOwner, Annuiant))
            {
                Annuiant.IncrementAge(1);
            }
            OnContractAgedByOneYear();
        }

        public override void UpdateOnAnniversaryReached()
        {
            OnAnniversaryReached();
        }

        public override void DeductPerentageAmountRiderBases(double percentage)
        {
            foreach (BaseRider rider in Riders)
            {
                if (rider is IBaseComputable _rider1)
                {
                    _rider1.DecreaseBasePercentageAmount(percentage);
                }
            }
        }
    }
}