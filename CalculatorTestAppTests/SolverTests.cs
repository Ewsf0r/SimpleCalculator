using CalculatorTestAppService.Implementations.Operations;
using CalculatorTestAppService.Implementations.Solver;
using CalculatorTestAppService.Interfaces;
using CalculatorTestAppService.Interfaces.Operation;

namespace CalculatorTestAppTests
{
  public class SolverTests
  {
    public SolverImpl TestSubject { get; set; } = new();

    [Fact]
    public void CorrectBasicTest()
    {
      var testOp = new AdditionOp(1, 2);
      var testOps = new[] { testOp };
      var actualResult = TestSubject.Solve(testOps);
      var expectedResult = 3;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void CorrectMultiplicationAndAdditionBasicTest()
    {
      var testOps = new IOperation[]
      {
        new AdditionOp(1, 2),
        new MultiplicationOp(2, 3),
        new MultiplicationOp(3, 4),
        new AdditionOp(4, 5)
      };
      var actualResult = TestSubject.Solve(testOps);
      var expectedResult = 30;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void IncorrectBasicTest()
    {
      var testOp = new AdditionOp(1, null);
      var testOps = new[] { testOp };
      var actualResult = ((ISolver)TestSubject).TrySolve(testOps, out _);
      Assert.False(actualResult);
    }

    [Fact]
    public void BorderOperationOrderTest()
    {
      var testOps = new IOperation[]
      {
        new MultiplicationOp(2, 2),
        new MultiplicationOp(2, 2),
        new MultiplicationOp(2, 2),
        new SubtractionOp(2, 1)
      };
      var actualResult = TestSubject.Solve(testOps);
      var expectedResult = 15;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void BasePowerTest()
    {
      var testOps = new IOperation[]
      {
        new PowerOp(2, 3),
      };
      var actualResult = TestSubject.Solve(testOps);
      var expectedResult = 8;
      Assert.Equal(expectedResult, actualResult);
    }
  }
}