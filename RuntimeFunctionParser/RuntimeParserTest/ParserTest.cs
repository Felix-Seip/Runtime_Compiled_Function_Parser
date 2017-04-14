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
		public void Power_Test()
		{
			string function = "2*x^2";

			Function f = _parser.ParseFunction(function);
			
			Assert.IsNotNull(f);
		}

		[TestMethod]
		public void PolynominalParse_Test()
		{
			string function = "2+-1,27411634756996*x+0*x^2+2,27411634756996*x^3";

			Function f = _parser.ParseFunction(function);
			Assert.IsNotNull(f);
		}
	}
}
