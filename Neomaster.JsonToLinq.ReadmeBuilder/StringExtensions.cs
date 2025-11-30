namespace Neomaster.JsonToLinq.ReadmeBuilder;

public static class StringExtensions
{
  private static readonly string _readmeTemplateFolder = Path.Combine(
    SolutionInfo.SolutionPath,
    "Docs",
    "readme");

  public static string InsertReadmeTemplate(this string readme, string templateName)
  {
    return readme.Replace(
      $"{{{templateName}}}",
      File.ReadAllText(Path.Combine(_readmeTemplateFolder, $"{templateName}.md")));
  }
}
