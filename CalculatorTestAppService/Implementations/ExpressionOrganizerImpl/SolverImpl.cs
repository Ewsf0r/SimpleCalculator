using CalculatorTestAppService.Data;
using CalculatorTestAppService.Interfaces;

namespace CalculatorTestAppService.Implementations.ExpressionOrganizerImpl
{
  public class SolverImpl: ISolver
  {
    private readonly ILogger _logger;

    public SolverImpl(ILogger logger)
    {
      _logger = logger;
    }
    public double Solve(IEnumerable<Operation> ops)
    {
      var opsArr = ops.ToArray();
      if(opsArr.Length == 1)
        return opsArr[0].GetResult();
      var opsLayersList = new List<List<Operation>>{new()};
      var currentLayer = 0;
      for (var i = 0; i < opsArr.Length - 1; i++)
      {
        var isLastRound = i == opsArr.Length - 2;
        var currentOp = opsArr[i];
        var nextOp = opsArr[i + 1];
        if(!currentOp.IsBracket())
          opsLayersList[currentLayer].Add(currentOp);

        if ((currentOp.IsAdditive() && nextOp.IsMultiplicative()) || currentOp.IsOpenBracket())
        {
          opsLayersList.Add(new List<Operation>());
          currentLayer++;
        }
        else if ((currentOp.IsMultiplicative() && nextOp.IsAdditive()) || currentOp.IsCloseBracket())
        {
          var subroutineResult = SolveSubroutine(opsLayersList[currentLayer]);

          if (currentLayer > 0)
          {
            currentLayer--;
            opsLayersList.RemoveAt(opsLayersList.Count - 1);
            if (opsLayersList[currentLayer].Count > 0)
              opsLayersList[currentLayer][^1] = opsLayersList[currentLayer][^1].WithRight(subroutineResult);
            else
            {
              var nextNonBracketOpIndex = FindNextNonBracketOpIndex(opsArr, i);
              if(nextNonBracketOpIndex == -1) break;
              opsArr[nextNonBracketOpIndex] = opsArr[nextNonBracketOpIndex].WithLeft(subroutineResult);
            }
          }
          else
            opsArr[i + 1] = opsArr[i + 1].WithLeft(subroutineResult);
        }
        if (isLastRound && !nextOp.IsCloseBracket())
          opsLayersList[currentLayer].Add(opsArr[^1]);
      }

      double result;
      if (currentLayer > 0)
      {
        for (var i = currentLayer; i > 0 ;)
        {
          result = SolveSubroutine(opsLayersList[i]);
          i--;
          opsLayersList[i][^1] = opsLayersList[i][^1].WithRight(result);
        }
      }
      result = SolveSubroutine(opsLayersList[0]);
      return result;
    }

    private static double SolveSubroutine(List<Operation> ops)
    {
      if (ops.Count == 1)
        return ops[0].GetResult();

      for (var i = 0; i < ops.Count - 1; i++)
      {
        var currOp = ops[i];
        if(currOp.IsBracket())
          continue;
        var nextOp = ops[i + 1];
        nextOp = nextOp.WithLeft(currOp.GetResult());
        ops[i + 1] = nextOp;
      }

      var result = ops[^1].GetResult();
      return result;
    }

    private static int FindNextNonBracketOpIndex(Operation[] ops, int currentIndex)
    {
      for (var i = currentIndex + 1; i < ops.Length; i++)
        if (!ops[i].IsBracket()) return i;
      return -1;
    }

    public bool TrySolve(IEnumerable<Operation> ops, out double? result)
    {

      try
      {
        result = Solve(ops);
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
