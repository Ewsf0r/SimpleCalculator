using System.Collections.Immutable;
using CalculatorTestAppService.Implementations.Operations;
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
      expressionStr = expressionStr.ToLowerInvariant();
      if (p_operations.Any(op => !op.CheckExpression(expressionStr)))
        throw new FormatException("Some of expressions operations are formated wrongly");

      var isFirst = true;
      var resBuilder = ImmutableList.CreateBuilder<IOperation>();

      var operationSubStr = "";

      bool CanParse(int position, out int offset)
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
                var parsedOps = ImmutableList.CreateBuilder<ImmutableList<IOperation>>();
                foreach (var value in arrayOperation.RawValue!)
                {
                  offset += value!.Length + 1;
                  parsedOps.Add(Parse(value));
                }
                operation = arrayOperation.WithParsedOperations(parsedOps.ToImmutable());
              }

              resBuilder.Add(operation);
            }

            parsable = true;
            break;
          }
        }

        return parsable;
      }

      var numberStr = "";
      for (var i = 0; i < expressionStr.Length; i++)
      {
        var c = expressionStr[i];
        // for numbers
        if (char.IsDigit(c) || c == '.' || (isFirst && c == '-'))
        {
          if (operationSubStr.Length != 0)
          {
            if (!CanParse(i, out var offset))
              throw new ArgumentException("Can't parse one of the operations");
            operationSubStr = "";
            i += offset;
          }
          numberStr += c;
        }
        // for operations
        else
        {
          operationSubStr += c;
          if (CanParse(i, out var offset))
          {
            operationSubStr = "";
            i += offset;
          }
        }
        isFirst = false;
      }
      if(resBuilder.Count==0 && !string.IsNullOrEmpty(numberStr))
        resBuilder.Add(BaseSingleValueOp.Parse(numberStr));
      return resBuilder.ToImmutable();
    }
    ImmutableList<IOperation> IParser.Parse(string expressionStr) => Parse(expressionStr);
  }
}