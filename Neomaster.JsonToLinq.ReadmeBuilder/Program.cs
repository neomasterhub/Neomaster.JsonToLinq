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
  .InsertReadmeTemplate("table-of-content")
  .InsertReadmeTemplate("advantages")
  .InsertReadmeTemplate("quick-start")
  .InsertReadmeTemplate("operators")
  .InsertReadmeTemplate("configuration")
  .InsertReadmeTemplate("testing")
  .InsertReadmeTemplate("demos")
  .InsertReadmeTemplate("limitations")
  .InsertReadmeTemplate("potential")
  ;

var nugetReadme = File.ReadAllText(Path.Combine(readmeTemplateFolder, "template-nuget.md"))
  .InsertReadmeTemplate("title")
  .InsertReadmeTemplate("tldr")
  .InsertReadmeTemplate("table-of-content")
  .InsertReadmeTemplate("advantages")
  .InsertReadmeTemplate("quick-start")
  .InsertReadmeTemplate("operators")
  .InsertReadmeTemplate("configuration")
  .InsertReadmeTemplate("testing")
  .InsertReadmeTemplate("demos")
  .InsertReadmeTemplate("limitations")
  .InsertReadmeTemplate("potential")
  .Replace(@"`\|`", "`|`")
  .Replace(@"`\|\|`", "`||`")
  ;

File.WriteAllText(SolutionInfo.ReadmePath, readme);
File.WriteAllText(SolutionInfo.NugetReadmePath, nugetReadme);
