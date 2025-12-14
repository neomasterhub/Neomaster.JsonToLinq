using Microsoft.Extensions.Logging;

namespace Neomaster.JsonToLinq.Demo;

internal class EFLoggerProvider(EFLog logOutput)
  : ILoggerProvider
{
  public ILogger CreateLogger(string categoryName = null)
  {
    return new EFLogger(logOutput);
  }

  public void Dispose()
  {
  }
}
