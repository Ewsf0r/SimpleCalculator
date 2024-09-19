using CalculatorTestAppService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorTestAppService.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class CalculatorController : ControllerBase
  {
    private readonly IParser _parser;
    private readonly ISolver p_solver;
    private readonly ILogger<CalculatorController> _logger;

    public CalculatorController(
      IParser parser,
      ISolver _solver,
      ILogger<CalculatorController> logger)
    {
      _parser = parser;
      p_solver = _solver;
      _logger = logger;
    }

    [HttpGet(Name = "GetCalculation")]
    public double Get(string expressionStr)
    {
      _logger.Log(LogLevel.Information, "Parsing");
      if (!_parser.TryParse(expressionStr, out var opsList)) return double.NaN;
      _logger.Log(LogLevel.Information, "Calculating");
      if (!p_solver.TrySolve(opsList!, out var result)) return double.NaN;
      return result ?? double.NaN;
    }
  }
}
