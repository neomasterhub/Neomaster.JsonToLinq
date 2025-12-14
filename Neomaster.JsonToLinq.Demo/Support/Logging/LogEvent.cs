using Microsoft.Extensions.Logging;

namespace Neomaster.JsonToLinq.Demo;

internal readonly struct LogEvent(LogLevel logLevel, string text)
{
  public readonly LogLevel LogLevel = logLevel;
  public readonly string Text = text;
}
