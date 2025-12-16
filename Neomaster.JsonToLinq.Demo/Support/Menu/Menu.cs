#pragma warning disable SA1512
using System.Text;

namespace Neomaster.JsonToLinq.Demo;

internal class Menu
{
  private readonly int _curYMin;
  private readonly int _curYMax;
  private readonly Log _log = new();
  private readonly MenuItem[] _menuItems;

  private readonly DataService _dataService;
  private readonly UserDemoService _userDemoService;

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
    _dataService = dataService;
    _userDemoService = userDemoService;

    _menuItems =
    [
      new("📊 Prepare Data", PrepareData),
      new("📊 1", UserDemo1),
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

  public void PrepareData()
  {
    _dataService.Prepare(_log);
  }

  public void UserDemo1()
  {
    _userDemoService.Demo1(_log);
  }
}
