namespace CalculatorTestAppService.Interfaces.Operation
{
  public interface ISingleValueOperation: IOperation
  {
    double? Value { get; }
  }
}
