using System.Text;
using CalculatorTestAppService.Controllers;
using CalculatorTestAppService.Implementations.ExpressionOrganizerImpl;
using CalculatorTestAppService.Implementations.ParserImpl;
using Xunit.Abstractions;

namespace CalculatorTestAppTests
{
  public class CalculatorControllerTests
  {
    public CalculatorControllerTests(ITestOutputHelper outputHelper)
    {
      TestSubject = new CalculatorController(new ParserImpl(), new ExpressionOrganizerImpl(),
        new CalculatorControllerTestLogger(outputHelper));
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

      if (testLen > 1_000)
        Assert.Throws(typeof(StackOverflowException), () => TestSubject.Get(testStrBuilder.ToString()));
      else
      {
        var actualResult = TestSubject.Get(testStrBuilder.ToString());
        var expectedResult = 1d;
        Assert.Equal(expectedResult, actualResult);
      }
    }
  }
}