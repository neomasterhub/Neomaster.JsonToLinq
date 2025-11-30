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

var demosText = string.Join(
  "\n",
  demos.Select(kv =>
    $"""
    ### {kv.Key}

    ```csharp
    {kv.Value}
    ```
    """));

var readmeTemplateFolder = Path.Combine(
  SolutionInfo.SolutionPath,
  "Docs",
  "readme");

var readme = File.ReadAllText(Path.Combine(readmeTemplateFolder, "template.md"))
  .InsertReadmeTemplate("logo")
  .InsertReadmeTemplate("shields")
  .InsertReadmeTemplate("title")
  .InsertReadmeTemplate("use-cases")
  .InsertReadmeTemplate("operators")
  .Replace("{demos}", $"## 🧪 Demos\n{demosText}");

var nugetReadme = File.ReadAllText(Path.Combine(readmeTemplateFolder, "template-nuget.md"))
  .InsertReadmeTemplate("logo")
  .InsertReadmeTemplate("title")
  .InsertReadmeTemplate("use-cases")
  .InsertReadmeTemplate("operators")
  .Replace(@"`\|`", "`|`")
  .Replace(@"`\|\|`", "`||`")
  .Replace("{demos}", $"## 🧪 Demos\n{demosText}");

File.WriteAllText(SolutionInfo.ReadmePath, readme);
File.WriteAllText(SolutionInfo.NugetReadmePath, nugetReadme);
