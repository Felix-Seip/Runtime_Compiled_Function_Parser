using System.Reflection;

namespace RuntimeFunctionParser
{
    public class Function
    {

        private MethodInfo m_mathFunction;
        private string m_originalFunction;

        public Function(MethodInfo mathFunction, string originalFunction)
        {
            m_mathFunction = mathFunction;
            m_originalFunction = originalFunction;
        }

		public Function(string originalFunction)
		{
			Parser p = new Parser();
			p.ParseFunction(originalFunction);
		}

        public double Solve(double x, double y)
        {
            return (double)m_mathFunction.Invoke(null, new object[] { x, y });
        }

        public override string ToString()
        {
            return m_originalFunction;
        }
    }
}
