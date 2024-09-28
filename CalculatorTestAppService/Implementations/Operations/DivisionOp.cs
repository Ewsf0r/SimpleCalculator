using CalculatorTestAppService.Interfaces.Operation;

namespace CalculatorTestAppService.Implementations.Operations
{
    public class DivisionOp(double? leftOp, double? rightOp) : BaseTwoElementsOperation(leftOp, rightOp)
  {
    public DivisionOp() : this(null, null) { }
    private DivisionOp(BaseTwoElementsOperation baseOp) : this(baseOp.LeftOp, baseOp.RightOp) { }
    public override string[] Operators => new[] { "/" };
    public override int Order { get; } = 1;
    public override double GetResult() => LeftOp!.Value / RightOp!.Value;
    public override string ToString() => $"{LeftOp}/{RightOp}";

    public override IOperation Parse(string expressionStr, int opPosition)
    {
      var res = (BaseTwoElementsOperation)base.Parse(expressionStr, opPosition);
      return new DivisionOp(res);
    }
    public override IOperation WithLeft(double left) => new DivisionOp(left, RightOp);
    public override IOperation WithRight(double right) => new DivisionOp(LeftOp, right);
  }
}