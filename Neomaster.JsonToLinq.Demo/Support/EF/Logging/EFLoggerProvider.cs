using Microsoft.Extensions.Logging;

namespace Neomaster.JsonToLinq.Demo;

internal class EFLoggerProvider(Log logOutput)
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
