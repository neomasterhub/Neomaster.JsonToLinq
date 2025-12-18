#pragma warning disable CA1822, SA1611
using System.Linq.Expressions;
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
  /// <![CDATA[&&[r1,r2]]]>
  /// </summary>
  public void And_2_(Log log)
  {
    using var dbContext = new AppDbContext();

    var filterJson =
      """
      {
        "Logic": "&&",
        "Rules": [
          {
            "Field": "balance",
            "Operator": ">=",
            "Value": 0
          },
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
      var expectedCount = dbContext.Users.Count(u =>
        u.Balance >= 0
        && u.LastVisitAt == null);

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

  /// <summary>
  /// <![CDATA[&&[r1,||[r2,r3]]]]>
  /// </summary>
  public void And_1Or_2__(Log log)
  {
    using var dbContext = new AppDbContext();

    var filterJson =
      """
      {
        "Logic": "&&",
        "Rules": [
          {
            "Field": "balance",
            "Operator": "<=",
            "Value": 0
          },
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
        u.Balance <= 0
        && (
          u.LastVisitAt == null
          || u.LastVisitAt < new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)));

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

  public void In(Log log)
  {
    using var dbContext = new AppDbContext();

    var filterJson =
      """
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "id",
            "Operator": "in",
            "Value": [-1, 1]
          }
        ]
      }
      """;

    try
    {
      var expectedCount = dbContext.Users.Count(u => new[] { -1, 1 }.Contains(u.Id));

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
  /// Custom operators:
  /// <list type="number">
  /// <item>
  /// <term><c>lt</c></term>
  /// <description><see cref="Expression.LessThan"/></description>
  /// </item>
  /// <item>
  /// <term><c>gt</c></term>
  /// <description><see cref="Expression.GreaterThan"/></description>
  /// </item>
  /// </list>
  /// </summary>
  public void CustomOp(Log log)
  {
    JsonLinq.Configure(options =>
    {
      options.OperatorMapper
        .Add("lt", Expression.LessThan)
        .Add("gt", Expression.GreaterThan);
    });

    using var dbContext = new AppDbContext();

    var filterJson =
      """
      {
        "Logic": "&&",
        "Rules": [
          {
            "Field": "balance",
            "Operator": "gt",
            "Value": 100
          },
          {
            "Field": "balance",
            "Operator": "lt",
            "Value": 10000
          }
        ]
      }
      """;

    try
    {
      var expectedCount = dbContext.Users.Count(u =>
        u.Balance > 100
        && u.Balance < 10000);

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
    finally
    {
      JsonLinq.RestoreDefaultOptions();
    }
  }
}
