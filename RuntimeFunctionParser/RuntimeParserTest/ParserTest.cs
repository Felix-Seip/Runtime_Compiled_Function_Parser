using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuntimeFunctionParser;

namespace RuntimeParserTest
{
	[TestClass]
	public class ParserTest
	{
		private static Parser _parser;

		[ClassInitialize]
		public static void InitializeClass(TestContext tc)
		{
			_parser = new Parser();
		}

		[TestMethod]
		public void MathPow_Test()
		{
			// Positiv 
			string function = "2*x^2";
			Function f = _parser.ParseFunction(function);

			Assert.AreEqual("2*Math.Pow(x,2)", f.OriginalFunction);

			// Negativ
			function = "2*x^-2";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("2*Math.Pow(x,-2)", f.OriginalFunction);

			// With Brackets
			function = "2*x^(2*9)";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("2*Math.Pow(x,2*9)", f.OriginalFunction);
		}

		[TestMethod]
		public void PolynominalParse_Test()
		{
			// Positiv 3. Degree
			string function = "2*x^3+3*x^2+x+6";

			Function f = _parser.ParseFunction(function);
			Assert.AreEqual("2*Math.Pow(x,3)+3*Math.Pow(x,2)+x+6", f.OriginalFunction);
		}
	}
}
