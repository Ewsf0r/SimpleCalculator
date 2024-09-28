using CalculatorTestAppService.Implementations.Operations;
using CalculatorTestAppService.Implementations.Parser;

namespace CalculatorTestAppTests
{
  public class OperationExtensionsTests
  {
    [Fact]
    public void DivisionByZeroThrowsTest()
    {
      var testOp = new DivisionOp(
        1,
        0);
      var actualResult = testOp.GetResult();
      Assert.True(double.IsInfinity(actualResult));
    }

    [Fact]
    public void AdditionParsingBasicTest()
    {
      var op = new AdditionOp();
      var actualResult = op.Parse("1+2", 1);
      var expectedResult = new AdditionOp(1, 2);
      Assert.Equal(expectedResult, actualResult);
    }
  }
}