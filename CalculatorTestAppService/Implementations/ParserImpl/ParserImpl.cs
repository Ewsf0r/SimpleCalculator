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
      expressionStr = expressionStr.Replace(",", ".");
      var isFirst = true;
      var resBuilder = ImmutableList.CreateBuilder<Operation>();
      var subStr = "";

      double operand;

      foreach (var c in expressionStr)
      {
        if (Char.IsLetter(c) || (!Char.IsDigit(c) && !"+-/*.".Contains(c)))
          throw new ArgumentException("Input string is incorrect");
        if (!Char.IsDigit(c) && "+-/*".Contains(c) && !(c == '-' && isFirst))
        {
          if (!Double.TryParse(subStr, CultureInfo.InvariantCulture, out operand))
            throw new ArgumentException("Can't parse one of the operands");
          if (resBuilder.Count != 0)
            resBuilder[^1] = resBuilder[^1].WithRight(operand);
          resBuilder.Add(new Operation(c.ToString(), operand));
          subStr = "";
          continue;
        }

        subStr += c;
        isFirst = false;
      }

      if (!Double.TryParse(subStr, CultureInfo.InvariantCulture, out operand))
        throw new ArgumentException("Can't parse one of the operands");
      resBuilder[^1] = resBuilder[^1].WithRight(operand);

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
