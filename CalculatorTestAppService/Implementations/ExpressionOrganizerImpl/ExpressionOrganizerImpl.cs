using CalculatorTestAppService.Data;
using CalculatorTestAppService.Interfaces;

namespace CalculatorTestAppService.Implementations.ExpressionOrganizerImpl
{
  public class ExpressionOrganizerImpl: IExpressionOrganizer
  {
    public static Operation Organize(IEnumerable<Operation> ops)
    {
      var opsArr = ops.ToArray();
      var multiplicationList = new List<Operation>();
      var additionList = new List<Operation>();

      for (var i = 0; i < opsArr.Length; i++)
      {
        var operation = opsArr[i];
        if (operation.IsMultiplicative())
        {
          multiplicationList.Add(operation);
          if (i != opsArr.Length - 1 && opsArr[i + 1].IsMultiplicative()) continue;
          var subOp = OrganizeSubroutine(multiplicationList);
          additionList.Add(subOp);
          multiplicationList = new List<Operation>();
        }
        else
          additionList.Add(operation);
      }

      var result = OrganizeSubroutine(additionList);
      return result;
    }

    private static Operation OrganizeSubroutine(List<Operation> ops)
    {
      var result = ops[^1];

      for (var i = 0; i < ops.Count - 1; i++)
      {
        var currOp = ops[i];
        var nextOp = ops[i + 1];
        if (currOp.IsAdditive() && nextOp.IsMultiplicative())
          nextOp = currOp.WithRight(nextOp);
        else
          nextOp = nextOp.WithLeft(currOp);
        ops[i + 1] = nextOp;
        result = nextOp;
      }

      return result;
    }

    public static bool TryOrganize(IEnumerable<Operation> ops, out Operation? result)
    {

      try
      {
        result = Organize(ops);
        return true;
      }
      catch
      {
        result = null;
        return false;
      }
    }

    Operation IExpressionOrganizer.Organize(IEnumerable<Operation> ops) => Organize(ops);
    bool IExpressionOrganizer.TryOrganize(IEnumerable<Operation> ops, out Operation? result) => TryOrganize(ops, out result);
  }
}
