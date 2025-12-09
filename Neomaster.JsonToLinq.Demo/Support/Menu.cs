#pragma warning disable SA1512
using System.Text.Json;
using Neomaster.JsonToLinq.UnitTests;

namespace Neomaster.JsonToLinq.Demo;

internal class Menu
{
  private static readonly int _curYMin;
  private static readonly int _curYMax;
  private static readonly MenuRow[] _menuRows =
  [
    new() { Text = "🧪 1. Filtering Users" },
    new() { Text = "🧪 2." },
    new() { Text = "🧪 3." },
  ];

  private static int _curY;
  private static int _selectedY;

  static Menu()
  {
    Console.CursorVisible = false;
    Console.OutputEncoding = System.Text.Encoding.UTF8;

    _curYMin = 5;
    _curYMax = _curYMin + _menuRows.Length - 1;
    _curY = _curYMin;
  }

  public static void Show()
  {
    ShowCommands();

    var key = Console.ReadKey().Key;

    while (key != ConsoleKey.Escape)
    {
      switch (key)
      {
        case ConsoleKey.UpArrow:
        case ConsoleKey.W:

          if (_curY > _curYMin)
          {
            _curY--;
          }
          else
          {
            _curY = _curYMax;
          }

          break;

        case ConsoleKey.DownArrow:
        case ConsoleKey.S:

          if (_curY < _curYMax)
          {
            _curY++;
          }
          else
          {
            _curY = _curYMin;
          }

          break;

        case ConsoleKey.Enter:
          _selectedY = _curY;
          break;
      }

      ShowCommands();

      key = Console.ReadKey().Key;
    }
  }

  private static void ShowCommands()
  {
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("🔗 JsonToLinq Demos 🔗\n");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(
      """
      Esc   - Exit
      Enter - Select

      """);

    foreach (var row in _menuRows)
    {
      var rowY = Console.GetCursorPosition().Top;

      if (rowY == _curY)
      {
        if (rowY == _selectedY)
        {
          Console.ForegroundColor = ConsoleColor.Green;
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.Gray;
        }
      }
      else
      {
        if (rowY == _selectedY)
        {
          Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.DarkGray;
        }
      }

      Console.WriteLine(row.Text);
    }

    Console.SetCursorPosition(0, _curY);
  }

  /// <summary>
  /// Filtering Users.
  /// </summary>
  private void FilteringUsers()
  {
    // 1. Source data.
    var users = new List<User>
    {
      new() { Id = 1, Balance = 0, LastVisitAt = null },
      new() { Id = 2, Balance = 0, LastVisitAt = DateTime.UtcNow },
      new() { Id = 3, Balance = 0, LastVisitAt = DateTime.UtcNow.AddYears(-10) },
      new() { Id = 4, Balance = 100, LastVisitAt = null },
      new() { Id = 5, Balance = 100, LastVisitAt = DateTime.UtcNow },
      new() { Id = 6, Balance = 100, LastVisitAt = DateTime.UtcNow.AddYears(-10) },
    };

    // 2. JSON filter definition (simulates front-end request).
    var filterJson = JsonDocument.Parse(
      """
      {
        "Logic": "&&",
        "Rules": [
          {
            "Field": "balance",
            "Operator": "=",
            "Value": 0
          },
          {
            "Logic": "||",
            "Rules": [
              {
                "Field": "lastVisitAt",
                "Operator": "=",
                "Value": null
              },
              {
                "Field": "lastVisitAt",
                "Operator": "<=",
                "Value": "2025-01-01T00:00:00Z"
              }
            ]
          }
        ]
      }
      """);

    // 3. Parse JSON to LINQ expression and compile.
    var filterExpr = JsonLinq.ParseToFilterExpression<User>(filterJson);
    var filterLambda = filterExpr.Compile();

    // 4. Apply filter.
    var filteredUsers = users.Where(filterLambda);

    // 5. Output results.
    foreach (var fu in filteredUsers)
    {
      Console.WriteLine($"Id: {fu.Id}");
    }

    // Id: 1
    // Id: 3

    Console.ReadKey();
  }
}
