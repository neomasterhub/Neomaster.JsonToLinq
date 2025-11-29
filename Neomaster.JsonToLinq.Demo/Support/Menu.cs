namespace Neomaster.JsonToLinq.Demo;

internal class Menu
{
  static Menu()
  {
    Console.OutputEncoding = System.Text.Encoding.UTF8;
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
