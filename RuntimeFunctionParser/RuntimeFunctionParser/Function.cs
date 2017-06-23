using System;
using System.Reflection;

namespace RuntimeFunctionParser
{
    public class Function
    {
        public MethodInfo _mathFunction { set; get; }

        private string _originalFunction;
        public string OriginalFunction
        {
            private set { _originalFunction = value; }
            get { return _originalFunction; }
        }

        private string _parsedFunction;
        public string ParsedFunction
        {
            private set { _parsedFunction = value; }
            get { return _parsedFunction; }
        }

        public Function(MethodInfo mathFunction, string originalFunction, string parsedFunction)
        {
            _mathFunction = mathFunction;
            OriginalFunction = originalFunction;
            ParsedFunction = parsedFunction;
        }

		public Function(string originalFunction, bool parse)
        {
            if(parse)
            {
                Parser p = new Parser();
                _mathFunction = p.ParseFunction(originalFunction)._mathFunction;
            }
        }

        public double Solve(double x, double y)
        {
            return (double)_mathFunction.Invoke(null, new object[] { x, y, 0 });
        }

        public double Solve(double x, double y, double z)
        {
            return (double)_mathFunction.Invoke(null, new object[] { x, y, z});
        }

        public Function UpdateFunction(string updatedFunction)
        {
            return new Parser().ParseFunction(updatedFunction);
        }

        public override string ToString()
        {
            return OriginalFunction;
        }
    }
}
