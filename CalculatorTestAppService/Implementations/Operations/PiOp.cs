using CalculatorTestAppService.Interfaces.Operation;

namespace CalculatorTestAppService.Implementations.Operations
{
  public class PiOp: ISingleValueOperation
  {
    public string[] Operators { get; } = new[] { "pi" };
    public int Order { get; } = 100;
    public bool CanParse(string operatorStr) => Operators.Contains(operatorStr);

    public IOperation? Parse(string expressionStr, int opPosition) => new PiOp();

    public double GetResult() => Value;

    public double Value { get; } = Math.PI;
  }
}
