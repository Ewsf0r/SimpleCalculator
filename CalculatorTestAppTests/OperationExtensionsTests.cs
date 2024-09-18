using CalculatorTestAppService.Data;

namespace CalculatorTestAppTests
{
  public class OperationExtensionsTests
  {
    [Fact]
    public void DivisionByZeroThrowsTest()
    {
      var testOp = new Operation(
        "/",
        1,
        0);
      Assert.Throws(typeof(ArgumentException), () => testOp.GetResult());
    }

    [Fact]
    public void EmptyOperationCalculationThrowsTest()
    {
      var testOp = new Operation();
      Assert.Throws(typeof(InvalidOperationException), () => testOp.GetResult());
    }
  }
}