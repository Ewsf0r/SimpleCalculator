using CalculatorTestAppService.Data;
using CalculatorTestAppService.Implementations.ExpressionOrganizerImpl;

namespace CalculatorTestAppTests
{
  public class OrganizerTests
  {
    [Fact]
    public void CorrectBasicTest()
    {
      var testOp = new Operation(
        "+",
        OperationExtensions.FromResult(1),
        OperationExtensions.FromResult(2));
      var testOps = new[] { testOp };
      var actualResult = ExpressionOrganizerImpl.Organize(testOps);
      var expectedResult = testOp;
      Assert.Equal(expectedResult, actualResult);
    }
    [Fact]
    public void CorrectMultiplicationAndAdditionBasicTest()
    {
      var addition1Op = new Operation(
        "+",
        OperationExtensions.FromResult(1),
        OperationExtensions.FromResult(2));
      var multiplication1Op = new Operation(
        "*",
        OperationExtensions.FromResult(2),
        OperationExtensions.FromResult(3));
      var multiplication2Op = new Operation(
        "*",
        OperationExtensions.FromResult(3),
        OperationExtensions.FromResult(4));
      var addition2Op = new Operation(
        "+",
        OperationExtensions.FromResult(4),
        OperationExtensions.FromResult(5));
      var testOps = new[]
      {
        addition1Op,
        multiplication1Op,
        multiplication2Op,
        addition2Op
      };
      var actualResult = ExpressionOrganizerImpl.Organize(testOps);
      var expectedResult = addition2Op
        .WithLeft(addition1Op
          .WithRight(multiplication2Op
            .WithLeft(multiplication1Op)));
      Assert.Equal(expectedResult, actualResult);
    }
  }
}