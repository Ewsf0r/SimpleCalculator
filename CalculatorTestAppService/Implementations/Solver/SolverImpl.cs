using CalculatorTestAppService.Interfaces;
using CalculatorTestAppService.Interfaces.Operation;
using System.Collections.Immutable;

namespace CalculatorTestAppService.Implementations.Solver
{
  public class SolverImpl : ISolver
  {
    public double Solve(IEnumerable<IOperation> ops)
    {
      var opsArr = ops.ToArray();
      if (opsArr.Length == 1)
      {
        if (opsArr[0] is IArrayOperation arrayOperation && arrayOperation.IsSolvingRequired)
          return Solve(arrayOperation.ParsedValue!);
        return opsArr[0].GetResult();
      }

      var opsLayersList = new List<List<IOperation>> { new() };
      var currentLayer = 0;
      for (var i = 0; i < opsArr.Length - 1; i++)
      {
        var isLastRound = i == opsArr.Length - 2;
        var currentOp = opsArr[i];
        var nextOp = opsArr[i + 1];

        var currentOrder = currentOp.Order;
        var nextOrder = nextOp.Order;
        opsLayersList[currentLayer].Add(currentOp);
        if (currentOrder < nextOrder)
        {
          opsLayersList.Add(new List<IOperation>());
          currentLayer++;
        }
        else if (currentOrder > nextOrder)
        {
          var subroutineResult = SolveSubroutine(opsLayersList[currentLayer]);

          if (currentLayer > 0)
          {
            currentLayer--;
            opsLayersList.RemoveAt(opsLayersList.Count - 1);
            if (opsLayersList[currentLayer].Count > 0)
              opsLayersList[currentLayer][^1] =
                ((ITwoElementsOperation)opsLayersList[currentLayer][^1]).WithRight(subroutineResult);
          }
          else
          {
            opsLayersList[0].Clear();
            opsArr[i + 1] = ((ITwoElementsOperation)opsArr[i + 1]).WithLeft(subroutineResult);
          }
        }
        if (isLastRound)
          opsLayersList[currentLayer].Add(opsArr[^1]);
      }

      double result;
      if (currentLayer > 0)
      {
        for (var i = currentLayer; i > 0; i--)
        {
          result = SolveSubroutine(opsLayersList[i]);
          opsLayersList[i - 1][^1] = ((ITwoElementsOperation)opsLayersList[i - 1][^1]).WithRight(result);
        }
      }

      result = SolveSubroutine(opsLayersList[0]);
      return result;
    }

    private double SolveSubroutine(List<IOperation> ops)
    {
      if (ops.Count == 1)
      {
        if (ops[0] is IArrayOperation arrayOperation && arrayOperation.IsSolvingRequired)
          return Solve(arrayOperation.ParsedValue!);
        return ops[0].GetResult();
      }

      for (var i = 0; i < ops.Count - 1; i++)
      {
        var currOp = ops[i];
        var nextOp = ops[i + 1];
        nextOp = ((ITwoElementsOperation)nextOp).WithLeft(currOp.GetResult());
        ops[i + 1] = nextOp;
      }

      var result = ops[^1].GetResult();
      return result;
    }
  }
}
