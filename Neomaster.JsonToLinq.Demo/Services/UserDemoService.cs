using Microsoft.Extensions.Logging;
using Neomaster.JsonToLinq.UnitTests;
using Xunit;

namespace Neomaster.JsonToLinq.Demo;

internal class UserDemoService
{
  public void Demo1(Log log)
  {
    using var dbContext = new AppDbContext();

    var filterJson =
      """
      {
        "Logic": "&&",
        "Rules": [
          {
            "Field": "lastVisitAt",
            "Operator": "=",
            "Value": null
          }
        ]
      }
      """;

    try
    {
      var expectedCount = dbContext.Users.Count(u => u.LastVisitAt == null);

      var actualCount = dbContext.Users.Count(JsonLinq.ParseToFilterExpression<User>(filterJson));

      Assert.Equal(expectedCount, actualCount);

      log.Add($"Filter:\n{filterJson}");
      log.AddSep();
      log.Add($"Count: {actualCount}");
    }
    catch (Exception ex)
    {
      log.Add(ex.Message, LogLevel.Error);
    }
  }
}
