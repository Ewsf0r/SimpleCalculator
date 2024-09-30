using System.Collections.Immutable;

namespace CalculatorTestAppService.Interfaces.Operation
{
  public interface IArrayOperation: IOperation
  {
    ImmutableList<string>? RawValue { get; }
    ImmutableList<ImmutableList<IOperation>>? ParsedOperations { get; }
    ImmutableList<double>? ProcessedValues { get; }
    IOperation WithParsedOperations(ImmutableList<ImmutableList<IOperation>> parsedOperations);
    IOperation WithProcessedValues(ImmutableList<double> values);
    bool IsParsingRequired { get; }
    bool IsPreSolvingRequired { get; }
    bool IsSolvable { get; }
  }
}
