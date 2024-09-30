using System.Collections.Immutable;
using System.Text;
using CalculatorTestAppService.Interfaces.Operation;

namespace CalculatorTestAppService.Implementations.Operations
{
  public class BaseArrayOp(
    IEnumerable<string>? rawData = null,
    IEnumerable<IEnumerable<IOperation>>? operations = null,
    IEnumerable<double>? processedValues = null) : IArrayOperation, IEquatable<BaseArrayOp>
  {
    public ImmutableList<string>? RawValue { get; } = rawData?.ToImmutableList();
    public ImmutableList<ImmutableList<IOperation>>? ParsedOperations { get; } = operations?.Select(op => op.ToImmutableList()).ToImmutableList();
    public ImmutableList<double>? ProcessedValues { get; } = processedValues?.ToImmutableList();

    public virtual string[] Operators { get; } = [];
    public virtual int Order { get; } = -1;

    public bool CanParse(string operatorStr) => Operators.Contains(operatorStr);

    public virtual IOperation Parse(string expressionStr, int opPosition)
    {
      if (expressionStr.Equals(")")) return null;
      var bracketsSum = 1;
      var subStr = new StringBuilder();
      for (var i = opPosition + 1; i < expressionStr.Length; i++)
      {
        var c = expressionStr[i];
        subStr.Append(c);
        if (c == '(') bracketsSum++;
        if (c == ')') bracketsSum--;
        if (bracketsSum == 0)
          return new BaseArrayOp(subStr
            .Remove(subStr.Length-1, 1)
            .ToString()
            .Split(","));
      }

      throw new ArgumentException("Can't parse brackets content");
    }

    public virtual double GetResult()
    {
      throw new NotImplementedException();
    }

    public virtual bool CheckExpression(string expressionStr) => true;

    public virtual IOperation WithParsedOperations(ImmutableList<ImmutableList<IOperation>> parsedOperations)
      => new BaseArrayOp(operations: parsedOperations);
    public virtual IOperation WithProcessedValues(ImmutableList<double> values)
      => new BaseArrayOp(processedValues: values);

    public bool IsParsingRequired => RawValue != null;
    public bool IsPreSolvingRequired => !IsParsingRequired && ParsedOperations?.Count > 0;
    public bool IsSolvable => !IsParsingRequired && !IsPreSolvingRequired && ProcessedValues?.Count > 0;

    public bool Equals(BaseArrayOp? other)
    {
      if (other is null) return false;
      if (ReferenceEquals(this, other)) return true;
      if((ParsedOperations == null && other.ParsedOperations != null) ||
         (ParsedOperations != null && other.ParsedOperations == null))
        return false;
      if(ParsedOperations != null && other.ParsedOperations != null)

        for (var i = 0; i < ParsedOperations.Count; i++)
        {
          var operation = ParsedOperations[i];
          var otherOperation = other.ParsedOperations[i];
          if (!operation.SequenceEqual(otherOperation)) return false;
        }

      var res = ((ProcessedValues == null && other.ProcessedValues == null) ||
                    (ProcessedValues != null && other.ProcessedValues != null && ProcessedValues!.SequenceEqual(other.ProcessedValues!)))
                && ((RawValue == null && other.RawValue == null) ||
                    (RawValue != null && other.RawValue != null && RawValue!.SequenceEqual(other.RawValue!)))
                && IsParsingRequired == other.IsParsingRequired
                && IsPreSolvingRequired == other.IsPreSolvingRequired;
      return res;
    }

    public override bool Equals(object? obj)
    {
      if (obj is null) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((BaseArrayOp)obj);
    }

    public override int GetHashCode() => HashCode.Combine(RawValue, ParsedOperations, ProcessedValues,
      Operators, Order, IsParsingRequired, IsPreSolvingRequired);

    public override string ToString()
    {
      if (IsParsingRequired)
        return $"Array of: {string.Join(",", RawValue!)}";
      if (IsPreSolvingRequired)
        return $"Array of: {ParsedOperations!.Count} operations";
      if (IsSolvable)
        return $"Array of: {ProcessedValues!.Count} values";
      return "Empty array operation";
    }
  }
}
