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
		public void Basic_Test()
		{
			string function = string.Empty;
			Function f = null;

			// with spaces
			function = "3 * 2+  4";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("3*2+4", f.OriginalFunction, "Spaces were not trimmed");
		}

		[TestMethod]
		public void MathPow_Test()
		{
			string function = string.Empty;
			Function f = null;

			// Without variable
			function = "3^2";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Pow(3,2)", f.OriginalFunction, "Failed with positiv power");

			// Positiv 
			function = "x^2";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Pow(x,2)", f.OriginalFunction, "Failed with positiv power");

			// Negativ
			function = "-2^3";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Pow(-2,3)", f.OriginalFunction, "Failed with -2^3");

			function = "x^-2";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Pow(x,-2)", f.OriginalFunction, "Failed with negativ power");

			function = "2^-(x+3)";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Pow(2,-(x+3))", f.OriginalFunction, "Failed with 2^-(x+3)");

			function = "-(x+2)^3";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Pow(-(x+2),3)", f.OriginalFunction, "Failed with -(x+2)^3");

			// With Brackets
			function = "x^(2*9)";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Pow(x,(2*9))", f.OriginalFunction, "Failed with brackets in power");

			// Operations on left side
			function = "(x+3)^2";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Pow((x+3),2)", f.OriginalFunction, "Failed with brackets on left side");

			// with y
			function = "x^y";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Pow(x,y)", f.OriginalFunction, "Failed with y");

			function = "(x+y)^2";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Pow((x+y),2)", f.OriginalFunction, "Failed with (x+y)^2");

			function = "2^(x+y)";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Pow(2,(x+y))", f.OriginalFunction, "Failed with 2^(x+y)");
		}

		[TestMethod]
		public void Sqrt_Test()
		{
			string function = string.Empty;
			Function f = null;

			// sunshine case
			function = "sqrt(x)";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Sqrt(x)", f.OriginalFunction, "Failed in Sunshine case");

			// with operants
			function = "sqrt(x / 3)";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Sqrt(x/3)", f.OriginalFunction, "Failed with operants");

			// with brackets
			function = "sqrt( (2+3)*x)";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Sqrt((2+3)*x)", f.OriginalFunction, "Failed with brackets");

			// with y
			function = "sqrt( x * y)";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Sqrt(x*y)", f.OriginalFunction, "Failed with y");
		}

		[TestMethod]
		public void NaturalConstants_Test()
		{
			string function = string.Empty;
			Function f = null;

			// lower case Pi
			function = "2*pi+3";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("2*Math.PI+3", f.OriginalFunction, "Error with lower case pi");

			// upper case Pi
			function = "2*PI+3";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("2*Math.PI+3", f.OriginalFunction, "Error with upper case PI");

			// lower case i
			function = "2*Pi+3";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("2*Math.PI+3", f.OriginalFunction, "Error with the spelling Pi");

			// lower case e
			function = "2*e+3";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("2*Math.E+3", f.OriginalFunction, "Error with lower case e");

			// upper case E
			function = "2*E+3";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("2*Math.E+3", f.OriginalFunction, "Error with upper case E");
		}

		[TestMethod]
		public void Error_Test()
		{
			string function = string.Empty;
			Function f = null;


			// wrong typed pi constant
			try
			{
				function = "2*pie+3";
				f = _parser.ParseFunction(function);
				Assert.Fail("Wrong typed pi does not throw an exception");
			}
			catch (ParserException) { }

			// brackets missing
			try
			{
				function = "2*(2+3";
				f = _parser.ParseFunction(function);
				Assert.Fail("Missing bracket does not throw an exception");
			}
			catch (ParserException) { }
			
		}
		
		[TestMethod]
		public void NestedFuctions_Test()
		{
			string function = string.Empty;
			Function f = null;

			// Sqrt with power
			function = "sqrt(x^2)";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Sqrt(Math.Pow(x,2))", f.OriginalFunction, " Failed with pow inside a sqrt argument");

			// sin with power
			function = "sin(1/x^2)";
			f = _parser.ParseFunction(function);

			Assert.AreEqual("Math.Sin(1/Math.Pow(x,2))", f.OriginalFunction, "sin(1/x^2) failed");
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
