#pragma warning disable SA1512
using System.Text;

namespace Neomaster.JsonToLinq.Demo;

internal class Menu
{
  private readonly int _curYMin;
  private readonly int _curYMax;
  private readonly MenuItem[] _menuItems;
  private readonly StringBuilder _demoOutput = new();

  private readonly DataService _dataService;

  private int _curY;
  private int _selectedY;
  private bool _runDemo;

  static Menu()
  {
    Console.CursorVisible = false;
    Console.OutputEncoding = Encoding.UTF8;
  }

  public Menu(
    DataService dataService)
  {
    _dataService = dataService;

    _menuItems =
    [
      new("📊 1. Prepare Data", PrepareData),
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
          _demoOutput.Clear();
          _runDemo = true;
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

    Action runDemo = null;
    string selectedMarker = null;
    foreach (var item in _menuItems)
    {
      var rowY = Console.GetCursorPosition().Top;
      var rowIsSelected = rowY == _selectedY;

      if (rowIsSelected)
      {
        runDemo = item.action;
        selectedMarker = " 👀";
      }

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

      Console.WriteLine(item.Text + selectedMarker);
    }

    if (_runDemo)
    {
      runDemo();
      _runDemo = false;
    }

    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine(_demoOutput);

    Console.SetCursorPosition(0, _curY);
  }

  public void PrepareData()
  {
    _dataService.Prepare();
  }
}
