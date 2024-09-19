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
      if (!BracketsCheck(expressionStr))
        throw new FormatException("Brackets placement exception");
      expressionStr = expressionStr.Replace(",", ".");
      var isFirst = true;
      var resBuilder = ImmutableList.CreateBuilder<Operation>();
      var subStr = "";

      double operand;

      foreach (var c in expressionStr)
      {
        //For any forbidden char
        if (Char.IsLetter(c) || (!Char.IsDigit(c) && !"+-/*.()".Contains(c)))
          throw new ArgumentException("Input string is incorrect");

        //Foe brackets
        if (c == '(' || c == ')')
        {
          if (subStr.Length > 0)
          {
            if (!Double.TryParse(subStr, CultureInfo.InvariantCulture, out operand))
              throw new ArgumentException("Can't parse one of the operands");
            if (resBuilder.Count != 0)
              resBuilder[^1] = resBuilder[^1].WithRight(operand);
            else
              throw new ArgumentException("No operation between operand and bracket");
            subStr = "";
          }
          resBuilder.Add(new Operation(c.ToString()));
          continue;
        }

        //For operations
        if (!Char.IsDigit(c) && "+-/*".Contains(c) && !(c == '-' && isFirst))
        {
          if (resBuilder.Count > 0 && resBuilder[^1].IsCloseBracket())
          {
            resBuilder.Add(new Operation(c.ToString()));
            continue;
          }
          if (!Double.TryParse(subStr, CultureInfo.InvariantCulture, out operand))
            throw new ArgumentException("Can't parse one of the operands");
          if (resBuilder.Count != 0)
            resBuilder[^1] = resBuilder[^1].WithRight(operand);
          resBuilder.Add(new Operation(c.ToString(), operand));
          subStr = "";
          continue;
        }

        //For digits
        subStr += c;
        isFirst = false;
      }

      //For last operand
      if (resBuilder.Count > 0 && !resBuilder[^1].IsCloseBracket())
      {
        if (!Double.TryParse(subStr, CultureInfo.InvariantCulture, out operand))
          throw new ArgumentException("Can't parse one of the operands");
        resBuilder[^1] = resBuilder[^1].WithRight(operand);
      }

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

    private static bool BracketsCheck(string expressionStr)
    {
      var bracketsSum = 0;
      foreach (var c in expressionStr)
      {
        if(c == '(') bracketsSum++;
        if(c == ')') bracketsSum--;
        if (bracketsSum < 0) return false;
      }
      return bracketsSum == 0;
    }
    ImmutableList<Operation> IParser.Parse(string expressionStr) => Parse(expressionStr);
    bool IParser.TryParse(string expressionStr, out ImmutableList<Operation>? result) => TryParse(expressionStr, out result);
  }
}
