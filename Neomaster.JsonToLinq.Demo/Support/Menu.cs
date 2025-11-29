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
  }
}
