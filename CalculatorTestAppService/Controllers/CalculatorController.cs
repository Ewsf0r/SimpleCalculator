using CalculatorTestAppService.Data;
using CalculatorTestAppService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorTestAppService.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class CalculatorController : ControllerBase
  {
    private readonly IParser _parser;
    private readonly IExpressionOrganizer _organizer;
    private readonly ILogger<CalculatorController> _logger;

    public CalculatorController(
      IParser parser,
      IExpressionOrganizer organizer,
      ILogger<CalculatorController> logger)
    {
      _parser = parser;
      _organizer = organizer;
      _logger = logger;
    }

    [HttpGet(Name = "GetCalculation")]
    public double Get(string expressionStr)
    {
      _logger.Log(LogLevel.Information, "Parsing");
      if (!_parser.TryParse(expressionStr, out var opsList)) return double.NaN;
      _logger.Log(LogLevel.Information, "Organizing");
      if (!_organizer.TryOrganize(opsList!, out var organizedOps)) return double.NaN;
      _logger.Log(LogLevel.Information, "Calculating");
      return organizedOps!.GetResult();
    }
  }
}
