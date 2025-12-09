#pragma warning disable SA1512
using System.Text.Json;
using Neomaster.JsonToLinq.UnitTests;

namespace Neomaster.JsonToLinq.Demo;

internal class Menu
{
  private readonly int _curYMin;
  private readonly int _curYMax;
  private readonly MenuItem[] _menuItems;

  private int _curY;
  private int _selectedY;

  static Menu()
  {
    Console.CursorVisible = false;
    Console.OutputEncoding = System.Text.Encoding.UTF8;
  }

  public Menu()
  {
    _menuItems =
    [
      new("🧪 1. Filtering Users", FilteringUsers),
    ];

    _curYMin = 5;
    _curYMax = _curYMin + _menuItems.Length - 1;
    _curY = _curYMin;
  }

  public void Show()
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

  private void ShowCommands()
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

    foreach (var item in _menuItems)
    {
      var rowY = Console.GetCursorPosition().Top;
      var rowIsSelected = rowY == _selectedY;

      if (rowY == _curY)
      {
        if (rowIsSelected)
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
        if (rowIsSelected)
        {
          Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.DarkGray;
        }
      }

      var selectedMarker = rowIsSelected
        ? " 👀"
        : string.Empty;
      Console.WriteLine(item.Text + selectedMarker);
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
