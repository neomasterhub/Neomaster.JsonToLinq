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

    for (var i = 0; i < _events.Count; i++)
    {
      var e = _events[i];

      Console.Write($"#{i + 1} ");

      switch (e.LogLevel)
      {
        case Microsoft.Extensions.Logging.LogLevel.Trace:
          break;
        case Microsoft.Extensions.Logging.LogLevel.Debug:
          break;
        case Microsoft.Extensions.Logging.LogLevel.Information:
          Console.ForegroundColor = ConsoleColor.Cyan;
          break;
        case Microsoft.Extensions.Logging.LogLevel.Warning:
          Console.ForegroundColor = ConsoleColor.Yellow;
          break;
        case Microsoft.Extensions.Logging.LogLevel.Error:
          Console.ForegroundColor = ConsoleColor.Red;
          break;
        case Microsoft.Extensions.Logging.LogLevel.Critical:
          Console.ForegroundColor = ConsoleColor.Red;
          break;
        case Microsoft.Extensions.Logging.LogLevel.None:
          break;
        default: throw new ArgumentOutOfRangeException(nameof(e.LogLevel));
      }

      Console.Write($"[{e.LogLevel}] ");

      Console.ResetColor();

      Console.WriteLine(e.Text);
    }

    Console.ResetColor();
  }
}
