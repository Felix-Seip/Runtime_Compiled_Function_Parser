using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuntimeFunctionParser;

namespace RuntimeParserTest
{
    [TestClass]
    public class DerivativeTests
    {
        [TestMethod]
        public void Derivative_Test()
        {
            string function = "x^2";
            Function func = new Parser().ParseFunction(function);
            Derivative derivative = new Derivative(func, 0.00001);
            double actual = derivative.Solve(2, 0, false, true);
            double expected = 4;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PartialDerivative_Test()
        {
            string function = "x^2+x*y^2"; //Derivative is 2x + y^2
            Function func = new Parser().ParseFunction(function);
            Derivative derivative = new Derivative(func, 0.00001);

            double x = 2;
            double y = 5;

            bool partialX = true;
            double result = derivative.Solve(x, y, true, partialX);

            double expected = 29;

            Assert.AreEqual(expected, result);
        }
    }
}
