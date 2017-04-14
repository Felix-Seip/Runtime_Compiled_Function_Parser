﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
							for (int j = 0; j < splitSide.Length; j++)
							{
								if (!IsMathematicalSymbol(splitSide[j], '(', ')', '*', '+', '-', '/') && j != 0)
								{
									rightSideOfPower += splitSide[j];
								}
								else if (IsMathematicalSymbol(splitSide[j], '(', ')', '*', '+', '-', '/') && j == 0)
								{
									rightSideOfPower += splitSide[j];
								}
								else
								{
									break;
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
