using System;

namespace RuntimeFunctionParser
{
    public class Derivative : Function
    {
        private double hValue;
        private Function baseFunction;

        public Derivative(Function baseFunc, double h) : base("", false)
        {
            hValue = h;
            baseFunction = baseFunc;
        }

        public double Solve(double x, double y, bool partialDerivative, bool xValue)
        {
            if (partialDerivative)
            {
                if (xValue)
                {
                    return PartialDerivative(x, y, true);
                }
                else
                {
                    return PartialDerivative(x, y, false);
                }
            }
            else
            {
                return Derivation_Approximation(x, false);
            }
        }

    private double Derivation_Approximation(double x, bool arithmeticMiddle = false)
    {
        double fFromXPlusH = baseFunction.Solve(x + hValue, 0);
        double fFromX = baseFunction.Solve(x, 0);
        if (arithmeticMiddle)
        {
            return Math.Round((fFromXPlusH - fFromX) / (2 * hValue), 4);
        }
        else
        {
            return Math.Round(((fFromXPlusH - fFromX) / hValue), 4);
        }
    }

    private double PartialDerivative(double x, double y, bool xValue)
    {
        double fFromXPlusH = baseFunction.Solve(x + hValue, y);
        double fFromX = baseFunction.Solve(x - hValue, y);

        double fFromYPlusH = baseFunction.Solve(x, y + hValue);
        double fFromY = baseFunction.Solve(x, y - hValue);

        if (xValue)
        {
            return Math.Round((fFromXPlusH - fFromX) / (2 * hValue), 4);
        }
        else
        {
            return Math.Round((fFromYPlusH - fFromY) / (2 * hValue), 4);
        }
    }
}
}
