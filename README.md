# Runtime_Compiled_Function_Parser
This Project implements a function parser that uses the .NET Runtime Compiler to parse strings into function objects

## Install
To install RuntimeFunctionParser, run the following command in the Package Manager Console:
`Install-Package RuntimeFunctionParser`

## Usage
```C#
using RuntimeFunctionParser;

Parser parser = new Parser();
Function f = parser.ParseFunction("x^2");
double y = f.Solve(2,0);
```

### Supported Operations
* ^ or pow  (Exponential)
* sqrt  (Square root)
* sin (Sinus)
* cos (Cosinus)
* log (Logarithm)
* abs (Absolute)

### Supported Constants
* pi 
* e
