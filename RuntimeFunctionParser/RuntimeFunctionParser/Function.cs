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

        public Function(MethodInfo mathFunction, string originalFunction)
        {
            _mathFunction = mathFunction;
			OriginalFunction = originalFunction;
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

        public override string ToString()
        {
            return OriginalFunction;
        }
    }
}
