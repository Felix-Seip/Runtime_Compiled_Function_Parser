using Microsoft.CSharp;
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
					string[] splitPower = new string[1];
					splitPower = function.Split(new char[] { '^' }, 2);

					string leftSideOfPower = "";
					string rightSideOfPower = "";

					for (int i = 0; i < splitPower.Length; i++)
					{
						char[] leftSplitSide = splitPower[i].ToCharArray();
						bool leftSide = i == 0 ? true : false;
						if (leftSide)
						{
							for (int j = leftSplitSide.Length - 1; j >= 0; j--)
							{
								if (leftSplitSide[j] != '(' && leftSplitSide[j] != '+' && leftSplitSide[j] != '*' &&
									leftSplitSide[j] != '-' && leftSplitSide[j] != '/')
								{
									if (leftSide)
									{
										leftSideOfPower += leftSplitSide[j];
									}
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
							for (int j = 0; j < leftSplitSide.Length; j++)
							{
								if ((leftSplitSide[j] != '(' && leftSplitSide[j] != ')' && leftSplitSide[j] != '*' &&
									leftSplitSide[j] != '+' && leftSplitSide[j] != '-' && leftSplitSide[j] != '/') && j != 0)
								{
									rightSideOfPower += leftSplitSide[j];
								}
								else if ((leftSplitSide[j] == '(' || leftSplitSide[j] == ')' || leftSplitSide[j] == '*' ||
									leftSplitSide[j] == '+' || leftSplitSide[j] == '-' || leftSplitSide[j] == '/') && j == 0)
								{
									rightSideOfPower += leftSplitSide[j];
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
	}
}
