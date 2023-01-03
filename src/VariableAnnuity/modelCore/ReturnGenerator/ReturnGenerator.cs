using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public class FixedReturnGenerator: BaseReturnGenerator
    {
        private double FixedReturn;

        public FixedReturnGenerator(double fixedReturn)
        {
            FixedReturn = fixedReturn;
        }

        public override double GetReturn()
        {
            return FixedReturn;
        }
    }

    public class CustomLogNormalReturnCalculator : BaseReturnGenerator
    {
        protected double Drift;
        protected double Volatility;
        private double ReturnConst;
        protected BaseRandomNumberGenerator RandomNormalGenerator;

        public CustomLogNormalReturnCalculator(double drift, double volatility, BaseRandomNumberGenerator randomNormalGenerator)
        {
            if (volatility < 0)
            {
                throw new ArgumentException("Volatility must be non-negative");
            }
            Drift = drift;
            Volatility = volatility;
            ReturnConst = Math.Log(1 + Drift) - 0.5 * Volatility * Volatility;
            RandomNormalGenerator = randomNormalGenerator;
        }
        public CustomLogNormalReturnCalculator(double drift, double volatility):this(drift, volatility, new RandomNormalGenerator())
        {
        }

        public override double GetReturn()
        {
            return Math.Exp(ReturnConst + Volatility * RandomNormalGenerator.GetRandom()) - 1;
        }
    }

    public class ReturnCalculatorFromList : BaseReturnGenerator
    {
        private Queue<double> Returns;
        public ReturnCalculatorFromList(List<double> returns)
        {
            Returns = new Queue<double>(returns);
        }

        public override double GetReturn()
        {
            if (Returns.Count == 0) 
            {
                throw new Exception("Supplied Return has been used up");
            }
            return Returns.Dequeue();
        }

    }
}
