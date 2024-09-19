using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace CalculatorTestAppTests
{
  public class TestLogger<T>: ILogger<T>
  {
    private readonly ITestOutputHelper _outputHelper;
    public TestLogger(ITestOutputHelper outputHelper)
    {
      _outputHelper = outputHelper;
    }
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
      throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
      throw new NotImplementedException();
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
      _outputHelper.WriteLine($"{logLevel}: {eventId}, {state}");
      _outputHelper.WriteLine(exception?.ToString() ?? "no ex");
    }
  }
}
