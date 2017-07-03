using System;

namespace RuntimeFunctionParser
{
    public class Derivative : Function
    {
        private const double hValue = 0.00001;
        private Function baseFunction;

        private const string nonPartialFirstDerivativeHMethod = "((x - y) / 0.00001)";
        private const string partialFirstDerivativeHMethod = "((x - y) / (2 * 0.00001))";
        private const string secondDerivativeHMethod = "(y - (2 * x) + z) / (0.00001^2)";

        private const string partialSecondDerivativeXXHMethod = "(y^2 - (2 * ((4 * x) * y)) - (2 * ((4 * x) * z)) + (2 * (z * y)) + (4 * x^2) + (4 * z^2)) / 0.00001^4";

        private Enums.eDerivativePower eDerivativePower;
        private Enums.eDerivativeType eDerivativeType;

        public Derivative(Function baseFunc, Enums.eDerivativePower derivativePower, Enums.eDerivativeType derivativeType)
            : base(DetermineDerivationHMethod(derivativePower, derivativeType), true)
        {
            baseFunction = baseFunc;
            eDerivativePower = derivativePower;
            eDerivativeType = derivativeType;
        }

        private static string DetermineDerivationHMethod(Enums.eDerivativePower derivativePower, Enums.eDerivativeType derivativeType)
        {
            switch (derivativeType)
            {
                case Enums.eDerivativeType.eNormal:
                    switch (derivativePower)
                    {
                        case Enums.eDerivativePower.eFirstDerivative:
                            return nonPartialFirstDerivativeHMethod;
                        case Enums.eDerivativePower.eSecondDerivative:
                            return secondDerivativeHMethod;
                    }
                    break;
                case Enums.eDerivativeType.ePartial:
                    switch(derivativePower)
                    {
                        case Enums.eDerivativePower.eFirstDerivative:
                            return partialFirstDerivativeHMethod;
                        case Enums.eDerivativePower.eSecondDerivative:
                            return partialSecondDerivativeXXHMethod;
                    }
                    break;
            }

            return "";
        }

        public double Solve(double x, double y, bool xValue)
        {
            if (eDerivativeType == Enums.eDerivativeType.ePartial)
            {
                if (xValue)
                {
                    return FirstPartialDerivative(x, y, true);
                }
                else
                {
                    return FirstPartialDerivative(x, y, false);
                }
            }
            else
            {
                if (eDerivativePower == Enums.eDerivativePower.eFirstDerivative)
                {
                    return FirstDerivativeFunction(x, false);
                }
                else
                {
                    return SecondDerivativeFunction(x, false);
                }
            }
        }

        private double FirstDerivativeFunction(double x, bool arithmeticMiddle = false)
        {
            double fFromXPlusH = baseFunction.Solve(x + hValue, 0);
            double fFromX = baseFunction.Solve(x, 0);

            return Math.Round(Solve(fFromXPlusH, fFromX), 4);
        }

        private double SecondDerivativeFunction(double x, bool arithmeticMiddle = false)
        {
            double fFromXPlusH = baseFunction.Solve(x + hValue, 0);
            double fFromX = baseFunction.Solve(x - hValue, 0);

            return Math.Round(Solve(baseFunction.Solve(x, 0), fFromXPlusH, fFromX), 4);
        }

        private double FirstPartialDerivative(double x, double y, bool xValue)
        {
            double fFromXPlusH = baseFunction.Solve(x + hValue, y);
            double fFromX = baseFunction.Solve(x - hValue, y);

            double fFromYPlusH = baseFunction.Solve(x, y + hValue);
            double fFromY = baseFunction.Solve(x, y - hValue);

            double res = Solve(fFromXPlusH, fFromX);


            if (xValue)
            {
                return Math.Round(Solve(fFromXPlusH, fFromX), 4);
            }
            else
            {
                return Math.Round(Solve(fFromYPlusH, fFromY), 4);
            }
        }
    }
}
