using CalculatorTestAppService.Interfaces.Operation;
using System.Globalization;

namespace CalculatorTestAppService.Implementations.Operations
{
  public class BaseSingleValueOp(double value): ISingleValueOperation, IEquatable<BaseSingleValueOp>
  {
    public string[] Operators { get; } = [];
    public int Order { get; } = 100;
    public bool CanParse(string operatorStr) => false;

    public static IOperation Parse(string valueStr)
    {
      var val = double.Parse(valueStr, CultureInfo.InvariantCulture);
      return new BaseSingleValueOp(val);
    }

    public IOperation Parse(string expressionStr, int opPosition) => Parse(expressionStr);
    

    public double GetResult() => Value;

    public double Value { get; } = value;
    public bool Equals(BaseSingleValueOp? other)
    {
      if (other is null) return false;
      if (ReferenceEquals(this, other)) return true;
      var res = Math.Abs(Value - other.Value) < 0.0001;
      return res;
    }

    public override bool Equals(object? obj)
    {
      if (obj is null) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((BaseSingleValueOp)obj);
    }

    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString(NumberFormatInfo.InvariantInfo);
  }
}
