using CalculatorTestAppService.Data;

namespace CalculatorTestAppService.Interfaces
{
  public interface IExpressionOrganizer
  {
    Operation Organize(IEnumerable<Operation> ops);

    bool TryOrganize(IEnumerable<Operation> ops, out Operation? result);
  }
}
