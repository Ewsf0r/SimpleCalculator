using CalculatorTestAppService.Interfaces.Operation;
using System.Globalization;

namespace CalculatorTestAppService.Implementations.Operations
{
  public class BaseTwoElementsOperation(double? leftOperand, double? rightOperand)
    : ITwoElementsOperation, IEquatable<BaseTwoElementsOperation>
  {
    public double? LeftOp { get; } = leftOperand;
    public double? RightOp { get; } = rightOperand;

    public virtual string[] Operators { get; } = [];
    public virtual int Order { get; } = -1;
    public bool CanParse(string operatorStr) => Operators.Contains(operatorStr);

    public virtual double GetResult()
    {
      throw new NotImplementedException();
    }

    public virtual IOperation Parse(string expressionStr, int opPosition)
    {
      if (opPosition == 0)
        throw new ArgumentException("Operation doesn't start with a number");

      int leftOperandStartPosition;
      for (leftOperandStartPosition = opPosition - 1; leftOperandStartPosition >= 0; leftOperandStartPosition--)
      {
        var c = expressionStr[leftOperandStartPosition];
        if (!char.IsDigit(c) && c != '.') break;
      }

      leftOperandStartPosition++;

      double? nullableLeftOp = null;
      if (double.TryParse(
            expressionStr.Substring(
              leftOperandStartPosition,
              opPosition - leftOperandStartPosition),
            CultureInfo.InvariantCulture,
            out var leftOperand))
        nullableLeftOp = leftOperand;
      if (nullableLeftOp != null && leftOperandStartPosition == 1 && expressionStr[0] == '-')
        nullableLeftOp = -nullableLeftOp;

      int rightOperandFinishPosition;
      for (rightOperandFinishPosition = opPosition + 1;
           rightOperandFinishPosition < expressionStr.Length;
           rightOperandFinishPosition++)
      {
        var c = expressionStr[rightOperandFinishPosition];
        if (!char.IsDigit(c) && c != '.') break;
      }

      rightOperandFinishPosition--;

      double? nullableRightOp = null;
      if (double.TryParse(
            expressionStr.Substring(
              opPosition + 1,
              rightOperandFinishPosition - opPosition),
            CultureInfo.InvariantCulture,
            out var rightOperand))
        nullableRightOp = rightOperand;
      return new BaseTwoElementsOperation(nullableLeftOp, nullableRightOp);
    }

    public virtual IOperation WithLeft(double left) => new BaseTwoElementsOperation(left, RightOp);
    public virtual IOperation WithRight(double right) => new BaseTwoElementsOperation(LeftOp, right);

    public bool Equals(BaseTwoElementsOperation? other)
    {
      if (other is null) return false;
      if (ReferenceEquals(this, other)) return true;
      var res = Nullable.Equals(LeftOp, other.LeftOp)
                && Nullable.Equals(RightOp, other.RightOp);
      return res;
    }

    public override bool Equals(object? obj)
    {
      if (obj is null) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((BaseTwoElementsOperation)obj);
    }

    public override int GetHashCode() => HashCode.Combine(LeftOp, RightOp, Operators, Order);
  }
}
