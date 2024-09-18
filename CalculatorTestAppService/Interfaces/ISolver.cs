using CalculatorTestAppService.Data;

namespace CalculatorTestAppService.Interfaces
{
  public interface ISolver
  {
    double Solve(IEnumerable<Operation> ops);

    bool TrySolve(IEnumerable<Operation> ops, out double? result);
  }
}
