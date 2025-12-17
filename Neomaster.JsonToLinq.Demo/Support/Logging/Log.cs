using Microsoft.Extensions.Logging;

namespace Neomaster.JsonToLinq.Demo;

internal class Log
{
  private readonly List<LogEvent> _events = [];

  public IReadOnlyList<LogEvent> Events => _events;

  public void Clear()
  {
    _events.Clear();
  }

  public void Add(LogEvent e)
  {
    _events.Add(e);
  }

  public void Add(
    string text,
    LogLevel logLevel = LogLevel.Information,
    bool isIndexed = true)
  {
    _events.Add(new LogEvent(text, logLevel, isIndexed));
  }

  public void AddSep()
  {
    Add("────────────────────────────", LogLevel.None, false);
  }

  public void AddRange(IEnumerable<LogEvent> events)
  {
    _events.AddRange(events);
  }

  public void CopyTo(Log log)
  {
    log.AddRange(Events);
  }

  public void CutTo(Log log)
  {
    CopyTo(log);
    Clear();
  }

  public void Print()
  {
    Console.ResetColor();

    var i = 1;
    foreach (var e in _events)
    {
      if (e.IsIndexed)
      {
        Console.Write($"#{i++} ");
      }

      switch (e.LogLevel)
      {
        case LogLevel.Trace:
          break;
        case LogLevel.Debug:
          break;
        case LogLevel.Information:
          Console.ForegroundColor = ConsoleColor.Cyan;
          break;
        case LogLevel.Warning:
          Console.ForegroundColor = ConsoleColor.Yellow;
          break;
        case LogLevel.Error:
          Console.ForegroundColor = ConsoleColor.Red;
          break;
        case LogLevel.Critical:
          Console.ForegroundColor = ConsoleColor.Red;
          break;
        case LogLevel.None:
          break;
        default: throw new ArgumentOutOfRangeException(nameof(e.LogLevel));
      }

      if (e.LogLevel != LogLevel.None)
      {
        Console.Write($"[{e.LogLevel}] ");
      }

      Console.ResetColor();

      Console.WriteLine(e.Text);
    }

    Console.ResetColor();
  }
}
