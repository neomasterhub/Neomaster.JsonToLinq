using System.Text;
using Neomaster.JsonToLinq.ReadmeBuilder;

var menuPath = Path.Combine(
  SolutionInfo.SolutionPath,
  "Neomaster.JsonToLinq.Demo",
  "Support",
  "Menu.cs");

string demoName = null;
var demo = new StringBuilder();
var demos = new Dictionary<string, string>();
var menuLineEnumerator = File.ReadLines(menuPath).GetEnumerator();

while (menuLineEnumerator.MoveNext())
{
  var line = menuLineEnumerator.Current;

  if (line.StartsWith("  /// <summary>"))
  {
    menuLineEnumerator.MoveNext();
    line = menuLineEnumerator.Current;
    demoName = line[6..^1];

    menuLineEnumerator.MoveNext(); // </summary>
    menuLineEnumerator.MoveNext(); // name
    menuLineEnumerator.MoveNext(); // {

    continue;
  }

  if (demoName != null)
  {
    if (line == "  }")
    {
      demos.Add(demoName, demo.ToString().Trim());
      demoName = null;
      continue;
    }

    if (line == "    Console.ReadKey();")
    {
      continue;
    }

    if (line == string.Empty)
    {
      demo.AppendLine();
    }
    else
    {
      demo.AppendLine(line[4..]);
    }
  }
}

var x = 1;
