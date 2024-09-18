using System.Collections.Immutable;
using CalculatorTestAppService.Data;
using CalculatorTestAppService.Implementations.ParserImpl;

namespace CalculatorTestAppTests
{
  public class ParserTests
  {
    [Fact]
    public void CorrectAdditionBasicTest()
    {
      var testStr = "1+2";
      var actualResult = ParserImpl.Parse(testStr);
      var expectedResult = new[]
      {
        new Operation(
          "+",
          OperationExtensions.FromResult(1),
          OperationExtensions.FromResult(2))
      }.ToImmutableList();
      Assert.Equal(expectedResult, actualResult);
    }
    [Fact]
    public void CorrectSubtractionWith3TermsAndNegativeTest()
    {
      var testStr = "-1-2-3";
      var actualResult = ParserImpl.Parse(testStr);
      var expectedResult = new[]
      {
        new Operation(
          "-",
          OperationExtensions.FromResult(-1),
          OperationExtensions.FromResult(2)),
        new Operation(
          "-",
          OperationExtensions.FromResult(2),
          OperationExtensions.FromResult(3))
      }.ToImmutableList();
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void IncorrectInputLettersTest()
    {
      var testStr = "a+2";
      var actualResult = ParserImpl.TryParse(testStr,out _);
      var expectedResult = false;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void IncorrectInputSpaceTest()
    {
      var testStr = "1 +2";
      var actualResult = ParserImpl.TryParse(testStr, out _);
      var expectedResult = false;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void IncorrectInputSignTest()
    {
      var testStr = "1,+2";
      var actualResult = ParserImpl.TryParse(testStr, out _);
      var expectedResult = false;
      Assert.Equal(expectedResult, actualResult);
    }
  }
}