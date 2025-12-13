namespace Neomaster.JsonToLinq.Demo;

internal record MenuItem(string text, Action action)
{
  public string Text => text;
  public Action Action => action;
}
