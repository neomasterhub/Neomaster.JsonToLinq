using Neomaster.JsonToLinq.ReadmeBuilder;

var readmeTemplateFolder = Path.Combine(
  SolutionInfo.SolutionPath,
  "Docs",
  "readme");

var readme = File.ReadAllText(Path.Combine(readmeTemplateFolder, "template.md"))
  .InsertReadmeTemplate("logo")
  .InsertReadmeTemplate("shields")
  .InsertReadmeTemplate("title")
  .InsertReadmeTemplate("tldr")
  .InsertReadmeTemplate("quick-start")
  .InsertReadmeTemplate("use-cases")
  .InsertReadmeTemplate("operators")
  .InsertReadmeTemplate("demos");

var nugetReadme = File.ReadAllText(Path.Combine(readmeTemplateFolder, "template-nuget.md"))
  .InsertReadmeTemplate("title")
  .InsertReadmeTemplate("tldr")
  .InsertReadmeTemplate("quick-start")
  .InsertReadmeTemplate("use-cases")
  .InsertReadmeTemplate("operators")
  .InsertReadmeTemplate("demos")
  .Replace(@"`\|`", "`|`")
  .Replace(@"`\|\|`", "`||`");

File.WriteAllText(SolutionInfo.ReadmePath, readme);
File.WriteAllText(SolutionInfo.NugetReadmePath, nugetReadme);
