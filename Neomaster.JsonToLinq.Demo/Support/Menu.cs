using System.Text.Json;
using Neomaster.JsonToLinq.UnitTests;

namespace Neomaster.JsonToLinq.Demo;

internal class Menu
{
  static Menu()
  {
    Console.OutputEncoding = System.Text.Encoding.UTF8;
  }

  public void Show()
  {
    ShowCommands();

    var key = Console.ReadKey().Key;

    while (key != ConsoleKey.Escape)
    {
      Console.WriteLine(); // Wrap after key output.

      switch (key)
      {
        case ConsoleKey.D1:
          FilteringUsers();
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
    Console.WriteLine("🔗 JsonToLinq Demos 🔗");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(
      """
      Esc   - Exit
      1     - Filtering Users
      Other - Clear

      """);
    Console.ResetColor();
  }

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
  }
}
