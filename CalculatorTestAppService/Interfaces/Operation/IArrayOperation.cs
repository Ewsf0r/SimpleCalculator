using System.Collections.Immutable;

namespace CalculatorTestAppService.Interfaces.Operation
{
    public interface IArrayOperation : IOperation
    {
        string? RawValue { get; }
        ImmutableList<IOperation>? ParsedValue { get; }
        IOperation WithParsedValue(ImmutableList<IOperation> parsedValue);
        bool IsParsingRequired { get; }
        bool IsSolvingRequired { get; }
    }
}
