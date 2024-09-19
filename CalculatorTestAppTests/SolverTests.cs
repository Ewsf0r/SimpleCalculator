using CalculatorTestAppService.Data;
using CalculatorTestAppService.Implementations.ExpressionOrganizerImpl;
using Xunit.Abstractions;

namespace CalculatorTestAppTests
{
  public class SolverTests
  {
    public SolverTests(ITestOutputHelper outputHelper)
    {
      var logger = new TestLogger<SolverImpl>(outputHelper);
      TestSubject = new SolverImpl(logger);
    }
    public SolverImpl TestSubject { get; set; }

    [Fact]
    public void CorrectBasicTest()
    {
      var testOp = new Operation(
        "+",
        1,
        2);
      var testOps = new[] { testOp };
      var actualResult = TestSubject.Solve(testOps);
      var expectedResult = 3;
      Assert.Equal(expectedResult, actualResult);
    }
    [Fact]
    public void CorrectMultiplicationAndAdditionBasicTest()
    {
      var addition1Op = new Operation(
        "+",
        1,
        2);
      var multiplication1Op = new Operation(
        "*",
        2,
        3);
      var multiplication2Op = new Operation(
        "*",
        3,
        4);
      var addition2Op = new Operation(
        "+",
        4,
        5);
      var testOps = new[]
      {
        addition1Op,
        multiplication1Op,
        multiplication2Op,
        addition2Op
      };
      var actualResult = TestSubject.Solve(testOps);
      var expectedResult = 30;
      Assert.Equal(expectedResult, actualResult);
    }
  }
}