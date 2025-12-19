#pragma warning disable CA1822, SA1611
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Neomaster.JsonToLinq.UnitTests;
using Xunit;
using static Neomaster.JsonToLinq.Demo.DemoConsts;

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

  /// <summary>
  /// Operator <c>in</c>.
  /// </summary>
  public void In(Log log)
  {
    using var dbContext = new AppDbContext();

    var filterDecimalValues = dbContext.Users
      .Skip(100)
      .Take(2)
      .Select(u => u.Balance)
      .ToArray();

    var filterDates = dbContext.Users
      .Where(u => u.LastVisitAt != null)
      .Take(2)
      .Select(u => u.LastVisitAt)
      .ToArray();

    var filterDateIsoValues = filterDates
      .Select(d => d.Value.ToString(PgIsoFormat))
      .ToArray();

    var filterEmails = dbContext.Users
      .Skip(200)
      .Take(2)
      .Select(u => u.Email)
      .ToList();
    filterEmails[1] = filterEmails[1].ToUpper();

    int[] intCol = [-1, 1];
    string[] strCol = ["1", .. filterEmails];
    decimal[] decCol = [-1, .. filterDecimalValues];
    DateTime?[] dtCol = [null, .. filterDates];

    var filterJson =
      $$"""
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "id",
            "Operator": "in",
            "Value": []
          },
          {
            "Field": "id",
            "Operator": "in",
            "Value": [-1, 1]
          },
          {
            "Field": "balance",
            "Operator": "in",
            "Value": [-1, {{filterDecimalValues[0]}}, {{filterDecimalValues[1]}}]
          },
          {
            "Field": "lastVisitAt",
            "Operator": "in",
            "Value": [null, "{{filterDateIsoValues[0]}}", "{{filterDateIsoValues[1]}}"]
          },
          {
            "Field": "email",
            "Operator": "in",
            "Value": ["1", "{{filterEmails[0]}}", "{{filterEmails[1]}}"]
          }
        ]
      }
      """;

    try
    {
      var expected = dbContext.Users
        .Where(u =>
          intCol.Contains(u.Id)
          || strCol.Contains(u.Email)
          || decCol.Contains(u.Balance)
          || dtCol.Contains(u.LastVisitAt))
        .OrderBy(u => u.Id)
        .ToArray();

      var actual = dbContext.Users
        .Where(JsonLinq.ParseToFilterExpression<User>(filterJson))
        .OrderBy(u => u.Id)
        .ToArray();

      Assert.True(expected.Length > 0);
      Assert.Equal(expected.Length, actual.Length);
      Assert.Equal(expected.Select(u => u.Id), actual.Select(u => u.Id));

      log.Add($"Filter:\n{filterJson}");
      log.AddSep();
      log.Add($"Count: {actual.Length}");

      log.Add("With date:");
      foreach (var u in actual.Where(u => u.LastVisitAt != null))
      {
        log.Add($"Id: {u.Id}, LastVisitAt: {u.LastVisitAt.Value.ToString(PgIsoFormat)}");
      }

      log.Add("With email:");
      foreach (var u in actual.Where(u => filterEmails.Contains(u.Email, StringComparer.OrdinalIgnoreCase)))
      {
        log.Add($"Id: {u.Id}, Email: {u.Email}");
      }

      log.Add(NoteString, textColor: ConsoleColor.DarkCyan);
    }
    catch (Exception ex)
    {
      log.Add(ex.Message, LogLevel.Error);
    }
  }

  /// <summary>
  /// Operator <c>as lower in</c>.
  /// </summary>
  public void AsLowerIn(Log log)
  {
    using var dbContext = new AppDbContext();

    var firstNames = dbContext.Users
      .Take(4)
      .Select(u => u.FirstName)
      .AsEnumerable()
      .Select((e, i) => i < 2
        ? e.ToLower()
        : e.ToUpper())
      .ToList();

    var firstNamesString = string.Join(",\n        ", firstNames.Select(e => $"\"{e}\""));

    var filterJson =
      $$"""
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "firstName",
            "Operator": "as lower in",
            "Value": [
              {{firstNamesString}}
            ]
          }
        ]
      }
      """;

    try
    {
      var expected = dbContext.Users
        .Where(u => firstNames.Contains(u.FirstName.ToLower()))
        .ToArray();

      var actual = dbContext.Users
        .Where(JsonLinq.ParseToFilterExpression<User>(filterJson))
        .ToArray();

      Assert.True(expected.Length > 0);
      Assert.Equal(expected.Length, actual.Length);
      Assert.Equal(expected.Select(u => u.Id), actual.Select(u => u.Id));

      log.Add($"Filter:\n{filterJson}");
      log.AddSep();

      log.Add("Found first names:");
      foreach (var u in actual.DistinctBy(u => u.FirstName))
      {
        log.Add(u.FirstName);
      }

      log.Add(NoteString, textColor: ConsoleColor.DarkCyan);
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
