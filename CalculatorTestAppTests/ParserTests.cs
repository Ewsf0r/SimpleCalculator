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
      Assert.False(actualResult);
    }

    [Fact]
    public void IncorrectInputSpaceTest()
    {
      var testStr = "1 +2";
      var actualResult = ParserImpl.TryParse(testStr, out _);
      Assert.False(actualResult);
    }

    [Fact]
    public void IncorrectInputCommaPlacementTest()
    {
      var testStr = "1,,+2";
      var actualResult = ParserImpl.TryParse(testStr, out _);
      Assert.False(actualResult);
    }

    [Fact]
    public void IncorrectInputMultipleSignsTest()
    {
      var testStr = "1*+2";
      var actualResult = ParserImpl.TryParse(testStr, out _);
      Assert.False(actualResult);
    }

    [Fact(Skip = "true")]
    public void IncorrectEmptyTest()
    {
      var testStr = "";
      var actualResult = ParserImpl.TryParse(testStr, out _);
      Assert.False(actualResult);
    }

    [Theory]
    [InlineData("+1")]
    [InlineData("1+")]
    public void IncorrectSignPlacementTest(string testStr)
    {
      var actualResult = ParserImpl.TryParse(testStr, out _);
      Assert.False(actualResult);
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

    [Fact]
    public void Correct1LayerOfBracketsTest()
    {
      var testStr = "(1+2)+3";
      var actualResult = ParserImpl.Parse(testStr);
      var expectedResult = new Operation[]
      {
        new ("(", null, 1),
        new ("+", 1, 2),
        new (")"),
        new ("+", null, 3),
      }.ToImmutableList();
      Assert.Equal(expectedResult, actualResult);
    }


    [Fact]
    public void Correct2LayersOfBracketsAtBeginningTest()
    {
      var testStr = "((1+2)+(3+4))+5";
      var actualResult = ParserImpl.Parse(testStr);
      var expectedResult = new Operation[]
      {
        new ("("),
        new ("(", null, 1),
        new ("+", 1, 2),
        new (")"),
        new ("+"),
        new ("(", null, 3),
        new ("+", 3, 4),
        new (")"),
        new (")"),
        new ("+", null, 5),
      }.ToImmutableList();
      Assert.Equal(expectedResult, actualResult);
    }


    [Fact]
    public void Correct2LayersOfBracketsAtEndTest()
    {
      var testStr = "5+((1+2)+(3+4))";
      var actualResult = ParserImpl.Parse(testStr);
      var expectedResult = new Operation[]
      {
        new ("+", 5),
        new ("("),
        new ("(", null, 1),
        new ("+", 1, 2),
        new (")"),
        new ("+"),
        new ("(", null, 3),
        new ("+", 3, 4),
        new (")"),
        new (")"),
      }.ToImmutableList();
      Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData("()", Skip = "true")]
    [InlineData(")(")]
    [InlineData("(")]
    [InlineData(")")]
    [InlineData("(1+2))")]
    [InlineData("((1+2)")]
    public void IncorrectBracketsPlacementTest(string testStr)
    {
      var actualResult = ParserImpl.TryParse(testStr, out _);
      Assert.False(actualResult);
    }
  }
}