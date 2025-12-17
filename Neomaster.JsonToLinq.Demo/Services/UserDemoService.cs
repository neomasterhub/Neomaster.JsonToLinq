#pragma warning disable CA1822, SA1611
using Microsoft.Extensions.Logging;
using Neomaster.JsonToLinq.UnitTests;
using Xunit;

namespace Neomaster.JsonToLinq.Demo;

internal class UserDemoService
{
  /// <summary>
  /// <![CDATA[&&[r]]]>
  /// </summary>
  public void And_1_(Log log)
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

  /// <summary>
  /// <![CDATA[&&[||[r1,r2]]]]>
  /// </summary>
  public void And_Or_2__(Log log)
  {
    using var dbContext = new AppDbContext();

    var filterJson =
      """
      {
        "Logic": "&&",
        "Rules": [
          {
            "Logic": "||",
            "Rules": [
              {
                "Field": "lastVisitAt",
                "Operator": "=",
                "Value": null
              },
              {
                "Field": "lastVisitAt",
                "Operator": "<",
                "Value": "2025-01-01T00:00:00Z"
              }
            ]
          }
        ]
      }
      """;

    try
    {
      var expectedCount = dbContext.Users.Count(u =>
        u.LastVisitAt == null
        || u.LastVisitAt < new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));

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
