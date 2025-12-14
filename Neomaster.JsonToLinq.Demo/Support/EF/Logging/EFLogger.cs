using Microsoft.Extensions.Logging;

namespace Neomaster.JsonToLinq.Demo;

internal class EFLogger(EFLog output)
  : ILogger
{
  public IDisposable BeginScope<TState>(TState state)
    where TState : notnull
  {
    throw new NotImplementedException();
  }

  public bool IsEnabled(LogLevel logLevel)
  {
    return true;
  }

  public void Log<TState>(
    LogLevel logLevel,
    EventId eventId,
    TState state,
    Exception exception,
    Func<TState, Exception, string> formatter)
  {
    output.Add(new(logLevel, formatter(state, exception)));
  }
}
