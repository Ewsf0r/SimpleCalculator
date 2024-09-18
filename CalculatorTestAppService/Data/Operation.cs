namespace CalculatorTestAppService.Data
{
  public record Operation(
    string? OpKey = "",
    double? LeftOp = null,
    double? RightOp = null);
}
