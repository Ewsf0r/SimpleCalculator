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
          1,
          2)
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
          -1,
          2),
        new Operation(
          "-",
          2,
          3)
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
    public void IncorrectInputCommaPlacementTest()
    {
      var testStr = "1,,+2";
      var actualResult = ParserImpl.TryParse(testStr, out var opsResult);
      var expectedResult = false;
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void IncorrectInputMultipleSignsTest()
    {
      var testStr = "1*+2";
      var actualResult = ParserImpl.TryParse(testStr, out var opsResult);
      var expectedResult = false;
      Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData(".")]
    [InlineData(",")]
    public void CorrectFloatingPointNumberWithCommaTest(string pointStr)
    {
      var testStr = $"1{pointStr}2+2";
      var actualResult = ParserImpl.Parse(testStr);

      var expectedResult = new[]
      {
        new Operation(
          "+",
          1.2,
          2)
      }.ToImmutableList();
      Assert.Equal(expectedResult, actualResult);
    }
  }
}