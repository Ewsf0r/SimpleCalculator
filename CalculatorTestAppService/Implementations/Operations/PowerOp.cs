using CalculatorTestAppService.Interfaces.Operation;

namespace CalculatorTestAppService.Implementations.Operations
{
  public class PowerOp(double? leftOp, double? rightOp) : BaseTwoElementsOperation(leftOp, rightOp)
  {
    public PowerOp() : this(null, null) { }

    private PowerOp(BaseTwoElementsOperation baseOp) : this(baseOp.LeftOp, baseOp.RightOp) { }

    public override string[] Operators => new[] { "^" };
    public override int Order { get; } = 2;
    public override double GetResult() => Math.Pow(LeftOp!.Value, RightOp!.Value);
    public override string ToString() => $"{LeftOp}^{RightOp}";

    public override IOperation Parse(string expressionStr, int opPosition)
    {
      var res = (BaseTwoElementsOperation)base.Parse(expressionStr, opPosition);
      return new PowerOp(res);
    }
    public override IOperation WithLeft(double left) => new PowerOp(left, RightOp);
    public override IOperation WithRight(double right) => new PowerOp(LeftOp, right);
  }
}
