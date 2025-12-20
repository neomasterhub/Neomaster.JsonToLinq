using System.Text;

namespace Neomaster.JsonToLinq.Demo;

internal class Menu
{
  private readonly int _curYMin;
  private readonly int _curYMax;
  private readonly Log _log = new();
  private readonly MenuItem[] _menuItems;

  private int _curY;
  private int _selectedY;
  private bool _runDemo;

  static Menu()
  {
    Console.CursorVisible = false;
    Console.OutputEncoding = Encoding.UTF8;
  }

  public Menu(
    DataService dataService,
    UserDemoService userDemoService)
  {
    _menuItems =
    [
      new("📊 Prepare data", () => dataService.Prepare(_log)),
      new("🧪 &&[r]", () => userDemoService.And_1_(_log)),
      new("🧪 &&[r1,r2]", () => userDemoService.And_2_(_log)),
      new("🧪 &&[||[r1,r2]]", () => userDemoService.And_Or_2__(_log)),
      new("🧪 &&[r1,||[r2,r3]]", () => userDemoService.And_1Or_2__(_log)),
      new("🧪 in", () => userDemoService.In(_log)),
      new("🧪 as lower in", () => userDemoService.AsLowerIn(_log)),
      new("🧪 as upper in", () => userDemoService.AsUpperIn(_log)),
      new("🧪 starts with", () => userDemoService.StartsWith(_log)),
      new("🧪 as lower starts with", () => userDemoService.AsLowerStartsWith(_log)),
      new("🧪 as upper starts with", () => userDemoService.AsUpperStartsWith(_log)),
      new("🧪 ends with", () => userDemoService.EndsWith(_log)),
      new("🧪 as lower ends with", () => userDemoService.AsLowerEndsWith(_log)),
      new("🧪 as upper ends with", () => userDemoService.AsUpperEndsWith(_log)),
      new("🧪 contains", () => userDemoService.Contains(_log)),
      new("🧪 Custom operators: lt, gt", () => userDemoService.CustomOp(_log)),
    ];

    _curYMin = 8;
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
          _log.Clear();
          _runDemo = true;
          break;

        case ConsoleKey.Spacebar:
          _log.Clear();
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
    Console.WriteLine(
      """
      ░▀▀█░█▀▀░█▀█░█▀█░░░▀█▀░█▀█░░░█░░░▀█▀░█▀█░█▀█░
      ░░░█░▀▀█░█░█░█░█░░░░█░░█░█░░░█░░░░█░░█░█░█░█░
      ░▀▀░░▀▀▀░▀▀▀░▀░▀░░░░▀░░▀▀▀░░░▀▀▀░▀▀▀░▀░▀░▀▀█░
      """);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(
      """

      Esc   - Exit
      Enter - Select
      Space - Clear

      """);

    Action runDemo = null;
    string selectedMarker = null;
    foreach (var item in _menuItems)
    {
      var rowY = Console.GetCursorPosition().Top;
      var rowIsSelected = rowY == _selectedY;

      if (rowIsSelected)
      {
        runDemo = item.action;

        if (item.ShowSelectedMarker)
        {
          selectedMarker = " 👀";
        }
      }
      else
      {
        selectedMarker = null;
      }

      if (rowY == _curY)
      {
        if (rowIsSelected)
        {
          Console.ForegroundColor = item.SelectedHoveredColor;
        }
        else
        {
          Console.ForegroundColor = item.NormalHoveredColor;
        }
      }
      else
      {
        if (rowIsSelected)
        {
          Console.ForegroundColor = item.SelectedColor;
        }
        else
        {
          Console.ForegroundColor = item.NormalColor;
        }
      }

      Console.WriteLine(item.Text + selectedMarker);
    }

    if (_runDemo)
    {
      runDemo();
      _runDemo = false;
    }

    Console.ResetColor();
    Console.WriteLine();

    _log.Print();

    Console.SetCursorPosition(0, _curY);
  }
}
