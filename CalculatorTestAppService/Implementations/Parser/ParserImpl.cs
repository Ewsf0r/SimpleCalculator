using System.Collections.Immutable;
using CalculatorTestAppService.Interfaces;
using CalculatorTestAppService.Interfaces.Operation;

namespace CalculatorTestAppService.Implementations.Parser
{
  public class ParserImpl : IParser
  {
    private readonly ImmutableList<IOperation> p_operations;

    public ParserImpl()
    {
      var iOperationType = typeof(IOperation);
      p_operations = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(t => iOperationType.IsAssignableFrom(t))
        .Where(type =>
          !type.IsAbstract &&
          !type.IsGenericType &&
          type.GetConstructor([]) != null)
        .Select(type => (IOperation)Activator.CreateInstance(type))
        .ToImmutableList();
    }

    public ImmutableList<IOperation> Parse(string expressionStr)
    {
      if (p_operations.Any(op => !op.CheckExpression(expressionStr)))
        throw new FormatException("Some of expressions operations are formated wrongly");

      var isFirst = true;
      var resBuilder = ImmutableList.CreateBuilder<IOperation>();

      var operationSubStr = "";

      bool canParse(int position, out int offset)
      {
        offset = 0;
        var parsable = false;
        foreach (var operationType in p_operations)
        {
          if (operationType.CanParse(operationSubStr))
          {
            var operation = operationType.Parse(expressionStr, position);
            if (operation != null)
            {
              if (operation is IArrayOperation arrayOperation && arrayOperation.IsParsingRequired)
              {
                offset = arrayOperation.RawValue!.Length + 1;
                operation = arrayOperation.WithParsedValue(Parse(arrayOperation!.RawValue));
              }

              resBuilder.Add(operation);
            }

            parsable = true;
            break;
          }
        }

        return parsable;
      }

      for (var i = 0; i < expressionStr.Length; i++)
      {
        var c = expressionStr[i];
        // for numbers
        if ((char.IsDigit(c) || c == '.' || (isFirst && c == '-')) && operationSubStr.Length != 0)
        {
          if (!canParse(i, out var offset))
            throw new ArgumentException("Can't parse one of the operations");
          operationSubStr = "";
          i += offset;
        }
        // for operations
        else
        {
          operationSubStr += c;
          if (canParse(i, out var offset))
          {
            operationSubStr = "";
            i += offset;
          }
        }
        isFirst = false;
      }
      return resBuilder.ToImmutable();
    }
    ImmutableList<IOperation> IParser.Parse(string expressionStr) => Parse(expressionStr);
  }
}