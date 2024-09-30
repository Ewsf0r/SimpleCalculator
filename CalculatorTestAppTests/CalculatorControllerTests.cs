using System.Text;
using CalculatorTestAppService.Controllers;
using CalculatorTestAppService.Implementations.Parser;
using CalculatorTestAppService.Implementations.Solver;
using Xunit.Abstractions;

namespace CalculatorTestAppTests
{
  public class CalculatorControllerTests
  {
    public CalculatorControllerTests(ITestOutputHelper outputHelper)
    {
      var logger = new TestLogger<CalculatorController>(outputHelper);
       TestSubject = new CalculatorController(
         new ParserImpl(),
         new SolverImpl(),
         logger);
    }
    public CalculatorController TestSubject { get; set; }
    [Fact]
    public void CorrectAdditionBasicTest()
    {
      var testStr = "1+2";
      var actualResult = TestSubject.Get(testStr);
      var expectedResult = 3d;
      Assert.Equal(expectedResult, actualResult);
    }
    [Fact]
    public void CorrectSubtractionWith3TermsAndNegativeTest()
    {
      var testStr = "-1-2-3";
      var actualResult = TestSubject.Get(testStr);
      var expectedResult = -6d;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void IncorrectInputLettersTest()
    {
      var testStr = "a+2";
      var actualResult = TestSubject.Get(testStr);
      var expectedResult = Double.NaN;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void CorrectEveryOperationTest()
    {
      var testStr = "1*1-1/1";
      var actualResult = TestSubject.Get(testStr);
      var expectedResult = 0d;
      Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1_000)]
    [InlineData(10_000)]
    public void StressTest(int testLen)
    {
      var testStrBuilder = new StringBuilder();
      testStrBuilder.Append("1");
      for (var i = 0; i < testLen; i++)
      {
        testStrBuilder.Append("*1-1/1+1");
      }

      var actualResult = TestSubject.Get(testStrBuilder.ToString());
      var expectedResult = 1d;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void CorrectBasicBracketsAtEndTest()
    {
      var testStr = "3*(2+3)";
      var actualResult = TestSubject.Get(testStr);
      var expectedResult = 15;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void CorrectBasicBracketsAtBeginningTest()
    {
      var testStr = "(2+3)*3";
      var actualResult = TestSubject.Get(testStr);
      var expectedResult = 15;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void Correct2LayersOfBracketsAtBeginningTest()
    {
      var testStr = "((1+2)*(3+4))+5";
      var actualResult = TestSubject.Get(testStr);
      var expectedResult = 26;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void Correct2LayersOfBracketsAtEndTest()
    {
      var testStr = "5+((1+2)*(3+4))";
      var actualResult = TestSubject.Get(testStr);
      var expectedResult = 26;
      Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData("")]
    [InlineData("()")]
    public void IncorrectEmptyTest(string testStr)
    {
      var actualResult = TestSubject.Get(testStr);
      Assert.True(double.IsNaN(actualResult));
    }

    [Fact]
    public void BorderOperationOrderTest()
    {
      var testStr = "2*2*2*2-1";
      var actualResult = TestSubject.Get(testStr);
      var expectedResult = 15;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void FloatingPointTest()
    {
      var testStr = "2.1+1.2";
      var actualResult = TestSubject.Get(testStr);
      var expectedResult = 3.3d;
      Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData("+1")]
    [InlineData("1+")]
    public void IncorrectInputTest(string testStr)
    {
      var actualResult = TestSubject.Get(testStr);
      Assert.True(double.IsNaN(actualResult));
    }

    [Theory]
    [InlineData("avg(1,2,3)", 2d)]
    [InlineData("avg(1,2,3)+4", 6d)]
    [InlineData("4+avg(1,1+1,(1+1)+1)", 6d)]
    public void AverageBaseTests(string testStr, double expectedResult)
    {
      var actualResult = TestSubject.Get(testStr);
      Assert.Equal(actualResult, expectedResult);
    }
  }
}