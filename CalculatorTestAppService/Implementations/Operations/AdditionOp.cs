using CalculatorTestAppService.Interfaces.Operation;

namespace CalculatorTestAppService.Implementations.Operations
{
    public class AdditionOp(double? leftOp, double? rightOp) : BaseTwoElementsOperation(leftOp, rightOp)
  {
    public AdditionOp() : this(null, null) {}
    private AdditionOp(BaseTwoElementsOperation baseOp) : this(baseOp.LeftOp, baseOp.RightOp){}

    public override string[] Operators => new[] { "+" };
    public override int Order { get; } = 0;
    public override double GetResult() => LeftOp!.Value + RightOp!.Value;
    public override string ToString() => $"{LeftOp}+{RightOp}";

    public override IOperation Parse(string expressionStr, int opPosition)
    {
      var res = (BaseTwoElementsOperation)base.Parse(expressionStr, opPosition);
      return new AdditionOp(res);
    }
    public override IOperation WithLeft(double left) => new AdditionOp(left, RightOp);
    public override IOperation WithRight(double right) => new AdditionOp(LeftOp, right);
  }
}
