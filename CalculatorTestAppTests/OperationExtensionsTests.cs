using CalculatorTestAppService.Data;

namespace CalculatorTestAppTests
{
  public class OperationExtensionsTests
  {
    [Fact]
    public void CalculationBasicTest()
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
      var testOp = addition2Op
        .WithLeft(addition1Op
          .WithRight(multiplication2Op
            .WithLeft(multiplication1Op)));
      var actualResult = testOp.GetResult();
      var expectedResult = 30d;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void DivisionByZeroThrowsTest()
    {
      var testOp = new Operation(
        "/",
        OperationExtensions.FromResult(1),
        OperationExtensions.FromResult(0));
      Assert.Throws(typeof(ArgumentException), () => testOp.GetResult());
    }

    [Fact]
    public void EmptyOperationCalculationThrowsTest()
    {
      var testOp = new Operation();
      Assert.Throws(typeof(ArgumentException), () => testOp.GetResult());
    }
  }
}