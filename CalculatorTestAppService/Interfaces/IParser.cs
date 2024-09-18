using System.Collections.Immutable;
using CalculatorTestAppService.Data;

namespace CalculatorTestAppService.Interfaces
{
  public interface IParser
  {
    ImmutableList<Operation> Parse(string expressionStr);

    bool TryParse(string expressionStr, out ImmutableList<Operation>? result);
  }
}
