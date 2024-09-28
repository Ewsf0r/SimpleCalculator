using System.Collections.Immutable;
using CalculatorTestAppService.Interfaces.Operation;

namespace CalculatorTestAppService.Interfaces
{
    public interface IParser
  {
    ImmutableList<IOperation> Parse(string expressionStr);

    bool TryParse(string expressionStr, out ImmutableList<IOperation>? result)
    {
      try
      {
        result = Parse(expressionStr);
        return true;
      }
      catch
      {
        result = null;
        return false;
      }
    }
  }
}
