using System.Collections.Immutable;
using CalculatorTestAppService.Interfaces.Operation;

namespace CalculatorTestAppService.Implementations.Operations
{
  public class AverageOp(
    IEnumerable<string>? rawData = null,
    IEnumerable<IEnumerable<IOperation>>? operations = null,
    IEnumerable<double>? processedValues = null) : BaseArrayOp(rawData, operations, processedValues)
  {
    public AverageOp() : this(null, null, null) { }
    private AverageOp(BaseArrayOp baseOp) : this(baseOp.RawValue, baseOp.ParsedOperations, baseOp.ProcessedValues) { }

    public override string[] Operators { get; } = new[] { "avg(" };
    public override int Order { get; } = 11;

    public override IOperation Parse(string expressionStr, int opPosition)
    {
      var res = (BaseArrayOp)base.Parse(expressionStr, opPosition);
      return new AverageOp(res);
    }

    public override double GetResult()
    {
      if (IsSolvable)
        return ProcessedValues!.Average();
      return double.NaN;
    }

    public override IOperation WithParsedOperations(ImmutableList<ImmutableList<IOperation>> parsedOperations)
      => new AverageOp(operations: parsedOperations);
    public override IOperation WithProcessedValues(ImmutableList<double> values)
      => new AverageOp(processedValues: values);

    public override string ToString()
    {
      if (IsParsingRequired)
        return $"Average of: {string.Join(",", RawValue!)}";
      if(IsPreSolvingRequired)
        return $"Average of: {ParsedOperations!.Count} operations";
      if (IsSolvable)
        return $"Average of: {ProcessedValues!.Count} values";
      return "Empty average operation";
    }
  }
}
