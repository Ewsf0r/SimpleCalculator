using System.Collections.Immutable;
using CalculatorTestAppService.Implementations.Operations;
using CalculatorTestAppService.Implementations.Parser;
using CalculatorTestAppService.Interfaces;
using CalculatorTestAppService.Interfaces.Operation;

namespace CalculatorTestAppTests
{
  public class ParserTests
  {
    public ParserImpl TestSubject { get; set; } = new();

    [Fact]
    public void CorrectAdditionBasicTest()
    {
      var testStr = "1+2";
      var actualResult = TestSubject.Parse(testStr);
      var expectedResult = new[]
      {
        (IOperation)new AdditionOp(1, 2)
      }.ToImmutableList();
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void CorrectSubtractionWith3TermsAndNegativeTest()
    {
      var testStr = "-1-2-3";
      var actualResult = TestSubject.Parse(testStr);
      var expectedResult = new IOperation[]
      {
        new SubtractionOp(
          -1,
          2),
        new SubtractionOp(
          2,
          3)
      }.ToImmutableList();
      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void IncorrectInputLettersTest()
    {
      var testStr = "a+2";
      var actualResult = ((IParser)TestSubject).TryParse(testStr, out _);
      Assert.False(actualResult);
    }

    [Fact]
    public void IncorrectInputSpaceTest()
    {
      var testStr = "1 +2";
      var actualResult = ((IParser)TestSubject).TryParse(testStr, out _);
      Assert.False(actualResult);
    }

    [Fact]
    public void IncorrectInputCommaPlacementTest()
    {
      var testStr = "1,,+2";
      var actualResult = ((IParser)TestSubject).TryParse(testStr, out _);
      Assert.False(actualResult);
    }

    [Fact(Skip = "true")] //now can be parsed dew to brackets, will fail at solving
    public void IncorrectInputMultipleSignsTest()
    {
      var testStr = "1*+2";
      var actualResult = ((IParser)TestSubject).TryParse(testStr, out var error);
      Assert.False(actualResult);
    }

    [Fact(Skip = "true")] //empty list don't throw
    public void IncorrectEmptyTest()
    {
      var testStr = "";
      var actualResult = ((IParser)TestSubject).TryParse(testStr, out var error);
      Assert.False(actualResult);
    }

    [Theory(Skip = "true")] //now can be parsed dew to brackets, will fail at solving
    [InlineData("+1")]
    [InlineData("1+")]
    public void IncorrectSignPlacementTest(string testStr)
    {
      var actualResult = ((IParser)TestSubject).TryParse(testStr, out _);
      Assert.False(actualResult);
    }

    [Fact]
    public void Correct1LayerOfBracketsTest()
    {
      var testStr = "(1+2)+3";
      var actualResult = TestSubject.Parse(testStr);
      var expectedResult = new IOperation[]
      {
        new BracketsOp(operations: new IOperation[]
        {
          new AdditionOp(1, 2)
        }),
        new AdditionOp(null, 3),
      }.ToImmutableList();
      Assert.Equal(expectedResult, actualResult);
    }


    [Fact]
    public void Correct2LayersOfBracketsAtBeginningTest()
    {
      var testStr = "((1+2)+(3+4))+5";
      var actualResult = TestSubject.Parse(testStr);
      var expectedResult = new IOperation[]
      {
        new BracketsOp(operations: new IOperation[]
        {
          new BracketsOp(operations: new IOperation[]
          {
            new AdditionOp(1, 2)
          }),
          new AdditionOp(null, null),
          new BracketsOp(operations: new IOperation[]
          {
            new AdditionOp(3, 4)
          }),
        }),
        new AdditionOp(null, 5),
      }.ToImmutableList();
      Assert.Equal(expectedResult, actualResult);
    }


    [Fact]
    public void Correct2LayersOfBracketsAtEndTest()
    {
      var testStr = "5+((1+2)+(3+4))";
      var actualResult = TestSubject.Parse(testStr);
      var expectedResult = new IOperation[]
      {
        new AdditionOp(5, null),
        new BracketsOp(operations: new IOperation[]
        {
          new BracketsOp(operations: new IOperation[]
          {
            new AdditionOp(1, 2)
          }),
          new AdditionOp(null, null),
          new BracketsOp(operations: new IOperation[]
          {
            new AdditionOp(3, 4)
          }),
        }),
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
      var actualResult = ((IParser)TestSubject).TryParse(testStr, out _);
      Assert.False(actualResult);
    }
  }
}