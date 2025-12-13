namespace Neomaster.JsonToLinq.Demo;

internal record MenuItem(
  string text,
  Action action,
  bool showSelectedMarker = true,
  ConsoleColor normalColor = ConsoleColor.DarkGray,
  ConsoleColor normalHoveredColor = ConsoleColor.DarkGreen,
  ConsoleColor selectedColor = ConsoleColor.Gray,
  ConsoleColor selectedHoveredColor = ConsoleColor.Green)
{
  public string Text => text;
  public Action Action => action;
  public ConsoleColor NormalColor => normalColor;
  public ConsoleColor NormalHoveredColor => normalHoveredColor;
  public ConsoleColor SelectedColor => selectedColor;
  public ConsoleColor SelectedHoveredColor => selectedHoveredColor;
  public bool ShowSelectedMarker => showSelectedMarker;
}
