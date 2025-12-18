using Microsoft.Extensions.Logging;

namespace Neomaster.JsonToLinq.Demo;

internal readonly struct LogEvent(
  string text,
  LogLevel logLevel = LogLevel.Information,
  bool isIndexed = true,
  ConsoleColor textColor = ConsoleColor.White)
{
  public readonly LogLevel LogLevel = logLevel;
  public readonly string Text = text;
  public readonly bool IsIndexed = isIndexed;
  public readonly ConsoleColor TextColor = textColor;
}
