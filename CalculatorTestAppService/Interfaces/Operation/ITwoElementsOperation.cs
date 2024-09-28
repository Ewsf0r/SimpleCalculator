namespace CalculatorTestAppService.Interfaces.Operation
{
    public interface ITwoElementsOperation : IOperation
    {
        double? LeftOp { get; }
        double? RightOp { get; }

        IOperation WithLeft(double left);
        IOperation WithRight(double right);
    }
}
