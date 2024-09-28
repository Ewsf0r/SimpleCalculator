using System.Collections.Immutable;
using System.Text;
using CalculatorTestAppService.Interfaces.Operation;

namespace CalculatorTestAppService.Implementations.Operations
{
    public class BracketsOp : IArrayOperation
  {
    public string[] Operators => new[] { "(", ")" };
    public int Order { get; } = 10;

    public ImmutableList<IOperation>? ParsedValue { get; }
    public string? RawValue { get; }

    public BracketsOp(string? rawData = null, IEnumerable<IOperation>? operations = null)
    {
      RawValue = rawData;
      ParsedValue = operations?.ToImmutableList();
    }
    public BracketsOp(){}

    public bool CanParse(string operatorStr) => Operators.Contains(operatorStr);

    public double GetResult() => throw new NotImplementedException();
    public IOperation Parse(string expressionStr, int opPosition)
    {
      if (expressionStr[opPosition] == ')')
        return null;
      var bracketsSum = 1;
      var subStr = new StringBuilder();
      for (var i = opPosition + 1; i < expressionStr.Length; i++)
      {
        var c = expressionStr[i];
        subStr.Append(c);
        if (c == '(') bracketsSum++;
        if (c == ')') bracketsSum--;
        if (bracketsSum == 0)
        {
          subStr.Remove(subStr.Length - 1, 1);
          return new BracketsOp(subStr.ToString());
        }
      }

      throw new ArgumentException("Can't parse brackets content");
    }

    bool IOperation.CheckExpression(string expressionStr)
    {
      var bracketsSum = 0;
      foreach (var c in expressionStr)
      {
        if (c == '(') bracketsSum++;
        if (c == ')') bracketsSum--;
        if (bracketsSum < 0) return false;
      }
      return bracketsSum == 0;
    }

    public IOperation WithParsedValue(ImmutableList<IOperation> parsedValue) => new BracketsOp(operations: parsedValue);

    public bool IsParsingRequired => !String.IsNullOrEmpty(RawValue);
    public bool IsSolvingRequired => String.IsNullOrEmpty(RawValue) && ParsedValue?.Count > 0;



    public bool Equals(BracketsOp? other)
    {
      if (other is null) return false;
      if (ReferenceEquals(this, other)) return true;
      var res = ((ParsedValue == null && other.ParsedValue == null) ||
                 ParsedValue != null && other.ParsedValue != null && ParsedValue!.SequenceEqual(other.ParsedValue!))
                && string.Equals(RawValue, other.RawValue)
                && IsParsingRequired == other.IsParsingRequired
                && IsSolvingRequired == other.IsSolvingRequired;
      return res;
    }

    public override bool Equals(object? obj)
    {
      if (obj is null) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((BracketsOp)obj);
    }

    public override int GetHashCode() => HashCode.Combine(RawValue, ParsedValue, Operators, Order, IsParsingRequired, IsSolvingRequired);

    public override string ToString() => $"(...{RawValue ?? ParsedValue!.Count + " parsed operations"}...)";
  }
}