namespace CalculatorTestAppService.Data
{
  public static class OperationExtensions
  {
    public static Operation FromResult(double result) => new(OpResult: result);
    public static Operation WithRight(this Operation thisOp, Operation rightOp) => thisOp with { RightOp = rightOp };
    public static Operation WithLeft(this Operation thisOp, Operation leftOp) => thisOp with { LeftOp = leftOp };
    public static Operation WithResult(this Operation thisOp, double result) => thisOp with { OpResult = result };
    public static bool IsMultiplicative(this Operation thisOp) => thisOp.OpKey == "*" || thisOp.OpKey == "/";
    public static bool IsAdditive(this Operation thisOp) => thisOp.OpKey == "+" || thisOp.OpKey == "-";

    public static double GetResult(this Operation thisOp)
    {
      var leftOp = thisOp.LeftOp?.GetResult() ?? 0;
      var rightOp = thisOp.RightOp?.GetResult() ?? leftOp;
      switch (thisOp.OpKey)
      {
        case "*":
          return leftOp * rightOp;
        case "/":
        {
          if (rightOp == 0d)
            throw new ArgumentException("Division by zero");
          return leftOp / rightOp;
        }
        case "+":
          return leftOp + rightOp;
        case "-":
          return leftOp - rightOp;
        default:
        {
          if (!thisOp.OpResult.HasValue)
            throw new ArgumentException("Argument has no value");
          return thisOp.OpResult.Value;
        }
      }
    }
  }
}
