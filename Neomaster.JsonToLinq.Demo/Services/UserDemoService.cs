using System.Text.Json;
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
    var expectedCount = dbContext.Users.Count(u => u.LastVisitAt == null);

    var actualCount = dbContext.Users.Count(JsonLinq.ParseToFilterExpression<User>(JsonDocument.Parse(filterJson)));

    Assert.Equal(expectedCount, actualCount);

    log.Add(filterJson);
  }
}
