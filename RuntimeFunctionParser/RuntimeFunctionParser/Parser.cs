using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RuntimeFunctionParser
{
	public class Parser
	{
		public Function ParseFunction(string function)
		{
			try
			{
				function = ReplaceUnknowns(function);

				string code = @"using System;            
                            namespace RuntimeFunctionParser
                            {                
                                public class MathFunctions
                                {                
                                    public static double UserFunction(double x, double y)
                                    {
                                        return math_func;
                                    }
                                }
                            }";

				string finalCode = code.Replace("math_func", function);

				CSharpCodeProvider provider = new CSharpCodeProvider();
				CompilerResults results = provider.CompileAssemblyFromSource(new CompilerParameters(), finalCode);

				Type binaryFunction = results.CompiledAssembly.GetType("RuntimeFunctionParser.MathFunctions");
				return new Function(binaryFunction.GetMethod("UserFunction"), function);
			}
			catch (Exception ex)
			{
				ParserException pe = ex as ParserException;
				if (pe == null)
					pe = new ParserException("Function could not be parsed", ex.InnerException);

				throw pe;
			}
		}

		private string ReplaceUnknowns(string function)
		{
			function = function.ToLower();
			function = Regex.Replace(function, @"\s+", ""); // Remove all Spaces
			function = function.Replace(',', '.');

			if (function.Contains("^"))
			{
				int count = function.Count(f => f == '^');
				for (int k = 0; k < count; k++)
				{
					string[] splitPower = function.Split(new char[] { '^' }, 2);

					string leftSideOfPower = "";
					string rightSideOfPower = "";

					for (int i = 0; i < splitPower.Length; i++)
					{
						char[] splitSide = splitPower[i].ToCharArray();
						bool leftSide = i == 0 ? true : false;
						if (leftSide)
						{
							for (int j = splitSide.Length - 1; j >= 0; j--)
							{
								if (!IsMathematicalSymbol(splitSide[j], '(', '*', '+', '-', '/'))
								{
									leftSideOfPower += splitSide[j];
								}
								else
								{
									break;
								}
							}
							char[] charArray = leftSideOfPower.ToCharArray();
							Array.Reverse(charArray);
							leftSideOfPower = new string(charArray);
						}
						else
						{
							if (splitSide[0].Equals('('))
							{
								if (!splitSide.Contains(')'))
									throw new ParserException("Missing ')' in Pow statement");

								for (int j = 0; j <= Array.IndexOf(splitSide, ')'); j++)
								{
									rightSideOfPower += splitSide[j];
								}
							}
							else
							{
								int indexOfFirstOccuredNumber = Array.IndexOf(splitSide, splitSide.First(c => char.IsDigit(c)));
								for (int j = 0; j <= indexOfFirstOccuredNumber; j++)
								{
									rightSideOfPower += splitSide[j];
								}
							}
						}
					}
					string fullStringToReplace = "Math.Pow(" + leftSideOfPower + "," + rightSideOfPower + ")";
					function = function.Replace(leftSideOfPower + "^" + rightSideOfPower, fullStringToReplace);
				}
			}

			if (function.Contains("e"))
			{
				function = function.Replace("e", "Math.E");
			}
			if (function.Contains("pi"))
			{
				function = function.Replace("pi", "Math.PI");
			}

			if (function.Contains("sqrt("))
			{
				function = function.Replace("sqrt(", "Math.Sqrt(");
			}

			if (function.Contains("cos("))
			{
				function = function.Replace("cos(", "Math.Cos(");
			}
			if (function.Contains("sin("))
			{
				function = function.Replace("sin(", "Math.Sin(");
			}
			if (function.Contains("log("))
			{
				function = function.Replace("log(", "Math.Log(");
			}
			if (function.Contains("abs("))
			{
				function = function.Replace("abs(", "Math.Abs(");
			}
			if (function.Contains("sign("))
			{
				function = function.Replace("sign(", "Math.Sign(");
			}

			return function;
		}

		/// <summary>
		/// Checks if a single character is one of the given symbols
		/// </summary>
		/// <param name="charToCheck">char to check the symbols with</param>
		/// <param name="symbols">symbols to check with</param>
		/// <returns></returns>
		private bool IsMathematicalSymbol(char charToCheck, params char[] symbols)
		{
			bool isCorrectSymbol = false;

			if (char.IsWhiteSpace(charToCheck))
				return isCorrectSymbol;

			foreach (char symbol in symbols)
			{
				if (!symbol.Equals(charToCheck))
					continue;

				isCorrectSymbol = true;
				break;
			}

			return isCorrectSymbol;
		}

	}
}
