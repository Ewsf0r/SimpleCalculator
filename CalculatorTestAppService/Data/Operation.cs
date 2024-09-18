namespace CalculatorTestAppService.Data
{
  public record Operation(
    string? OpKey = "",
    Operation? LeftOp = null,
    Operation? RightOp = null,
    double? OpResult = null);
}
