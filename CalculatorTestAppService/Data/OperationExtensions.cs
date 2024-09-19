namespace CalculatorTestAppService.Data
{
  public static class OperationExtensions
  {
    public static Operation WithRight(this Operation thisOp, double rightOp) => thisOp with { RightOp = rightOp };
    public static Operation WithLeft(this Operation thisOp, double leftOp) => thisOp with { LeftOp = leftOp };
    public static bool IsMultiplicative(this Operation thisOp) => thisOp.OpKey == "*" || thisOp.OpKey == "/";
    public static bool IsAdditive(this Operation thisOp) => thisOp.OpKey == "+" || thisOp.OpKey == "-";
    public static bool IsBracket(this Operation thisOp) => thisOp.OpKey == "(" || thisOp.OpKey == ")";
    public static bool IsOpenBracket(this Operation thisOp) => thisOp.OpKey == "(";
    public static bool IsCloseBracket(this Operation thisOp) => thisOp.OpKey == ")";

    public static double GetResult(this Operation thisOp)
    {
      var leftOp = thisOp.LeftOp!.Value;
      var rightOp = thisOp.RightOp!.Value;
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
          throw new ArgumentException("Argument has no value");
      }
    }
  }
}
