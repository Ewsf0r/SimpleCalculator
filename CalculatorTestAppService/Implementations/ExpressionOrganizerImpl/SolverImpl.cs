using CalculatorTestAppService.Data;
using CalculatorTestAppService.Interfaces;

namespace CalculatorTestAppService.Implementations.ExpressionOrganizerImpl
{
  public class SolverImpl: ISolver
  {
    public static double Solve(IEnumerable<Operation> ops)
    {
      var opsArr = ops.ToArray();
      var multiplicationList = new List<Operation>();
      var additionList = new List<Operation>();
      var subOp = double.NaN;

      for (var i = 0; i < opsArr.Length; i++)
      {
        var operation = opsArr[i];
        if (operation.IsMultiplicative())
        {
          multiplicationList.Add(operation);
          if (i != opsArr.Length - 1 && opsArr[i + 1].IsMultiplicative()) continue;
          var multiplicationOp = SolveSubroutine(multiplicationList);
          if (additionList.Count == 0)
          {
            subOp = multiplicationOp;
            continue;
          }

          additionList[^1] = additionList[^1].WithRight(multiplicationOp);
          multiplicationList = new List<Operation>();
        }
        else
        {
          if (!double.IsNaN(subOp))
          {
            operation = operation.WithLeft(subOp);
            subOp = double.NaN;
          }
          additionList.Add(operation);
        }
      }

      if (!double.IsNaN(subOp)) return subOp;
      var result = SolveSubroutine(additionList);
      return result;
    }

    private static double SolveSubroutine(List<Operation> ops)
    {
      var result = ops[^1].GetResult();

      for (var i = 0; i < ops.Count - 1; i++)
      {
        var currOp = ops[i];
        var nextOp = ops[i + 1];
        nextOp = nextOp.WithLeft(currOp.GetResult());
        ops[i + 1] = nextOp;
        result = nextOp.GetResult();
      }

      return result;
    }

    public static bool TrySolve(IEnumerable<Operation> ops, out double? result)
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

    double ISolver.Solve(IEnumerable<Operation> ops) => Solve(ops);
    bool ISolver.TrySolve(IEnumerable<Operation> ops, out double? result) => TrySolve(ops, out result);
  }
}
