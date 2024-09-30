using CalculatorTestAppService.Interfaces.Operation;
using System.Collections.Immutable;

namespace CalculatorTestAppService.Implementations.Operations
{
    public class BracketsOp(
      IEnumerable<string>? rawData = null,
      IEnumerable<IEnumerable<IOperation>>? operations = null,
      IEnumerable<double>? processedValues = null) : BaseArrayOp(rawData, operations, processedValues)
  {
    public BracketsOp(IEnumerable<IOperation> operations) : this(operations: new []{operations}) { }
    public BracketsOp() : this(null, null, null) { }
    private BracketsOp(BaseArrayOp baseOp) : this(baseOp.RawValue, baseOp.ParsedOperations, baseOp.ProcessedValues) { }

    public override string[] Operators => new[] { "(", ")" };
    public override int Order { get; } = 10;
    public override double GetResult() => ProcessedValues![0];

    public override IOperation WithParsedOperations(ImmutableList<ImmutableList<IOperation>> parsedOperations)
      => new BracketsOp(operations: parsedOperations);
    public override IOperation WithProcessedValues(ImmutableList<double> values)
      => new BracketsOp(processedValues: values);
    public override IOperation Parse(string expressionStr, int opPosition)
    {
      var res = (BaseArrayOp)base.Parse(expressionStr, opPosition);
      if (res.RawValue!.Count > 1) throw new ArgumentException("Basic bracket operation does not support arrays");
      return new BracketsOp(res);
    }

    public override bool CheckExpression(string expressionStr)
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

    public override string ToString()
    {
      if (IsParsingRequired)
        return $"(...{RawValue![0]}...)";
      if (IsPreSolvingRequired)
        return $"(...{ParsedOperations!.Count} operations...)";
      if (IsSolvable)
        return $"({ProcessedValues![0]})";
      return "Empty bracket operation";
    }
  }
}