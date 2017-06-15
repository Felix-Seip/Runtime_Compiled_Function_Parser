using System.Reflection;

namespace RuntimeFunctionParser
{
    public class Function
    {
        private MethodInfo _mathFunction;

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

		public Function(string originalFunction)
		{
			Parser p = new Parser();
			p.ParseFunction(originalFunction);
		}

        public double Solve(double x, double y)
        {
            return (double)_mathFunction.Invoke(null, new object[] { x, y });
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
