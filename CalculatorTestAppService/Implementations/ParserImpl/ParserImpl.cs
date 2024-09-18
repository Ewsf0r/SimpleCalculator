using System.Collections.Immutable;
using System.Globalization;
using CalculatorTestAppService.Data;
using CalculatorTestAppService.Interfaces;

namespace CalculatorTestAppService.Implementations.ParserImpl
{
  public class ParserImpl: IParser
  {
    public static ImmutableList<Operation> Parse(string expressionStr)
    {
      var isFirst = true;
      var resBuilder = ImmutableList.CreateBuilder<Operation>();
      var subStr = "";

      double operandD;
      Operation operandOp;

      foreach (var c in expressionStr)
      {
        if (Char.IsLetter(c) || (!Char.IsDigit(c) && !"+-/*".Contains(c)))
          throw new ArgumentException("Input string is incorrect");
        if (!Char.IsDigit(c) && "+-/*".Contains(c) && !(c == '-' && isFirst))
        {
          if (!Double.TryParse(subStr, CultureInfo.InvariantCulture, out operandD))
            throw new ArgumentException("Can't parse one of the operands");
          operandOp = OperationExtensions.FromResult(operandD);
          if (resBuilder.Count != 0)
            resBuilder[^1] = resBuilder[^1].WithRight(operandOp);
          resBuilder.Add(new Operation(c.ToString(), operandOp));
          subStr = "";
          continue;
        }

        subStr += c;
        isFirst = false;
      }

      if (!Double.TryParse(subStr, CultureInfo.InvariantCulture, out operandD))
        throw new ArgumentException("Can't parse one of the operands");
      operandOp = OperationExtensions.FromResult(operandD);
      resBuilder[^1] = resBuilder[^1].WithRight(operandOp);

      return resBuilder.ToImmutable();
    }

    public static bool TryParse(string expressionStr, out ImmutableList<Operation>? result)
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
    ImmutableList<Operation> IParser.Parse(string expressionStr) => Parse(expressionStr);
    bool IParser.TryParse(string expressionStr, out ImmutableList<Operation>? result) => TryParse(expressionStr, out result);
  }
}
