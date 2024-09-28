using CalculatorTestAppService.Interfaces.Operation;

namespace CalculatorTestAppService.Interfaces
{
    public interface ISolver
  {
    double Solve(IEnumerable<IOperation> ops);

    bool TrySolve(IEnumerable<IOperation> ops, out double? result)
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
