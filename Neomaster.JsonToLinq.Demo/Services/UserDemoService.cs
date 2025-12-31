#pragma warning disable CA1822, SA1611
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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

      log.Add("With dates:");
      foreach (var u in actual.Where(u => u.LastVisitAt != null))
      {
        log.Add($"Id: {u.Id}, LastVisitAt: {u.LastVisitAt.Value.ToString(PgIsoFormat)}");
      }

      log.Add("With specified decimal values:");
      foreach (var u in actual.Where(u => filterDecimalValues.Contains(u.Balance)))
      {
        log.Add($"Id: {u.Id}, Balance: {u.Balance}");
      }

      log.Add("With specified emails:");
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
  /// Operator <c>as upper in</c>.
  /// </summary>
  public void AsUpperIn(Log log)
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
            "Operator": "as upper in",
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
        .Where(u => firstNames.Contains(u.FirstName.ToUpper()))
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
  /// Operator <c>starts with</c>.
  /// </summary>
  public void StartsWith(Log log)
  {
    using var dbContext = new AppDbContext();

    const string prefix = "Josi";
    var filterJson =
      $$"""
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "firstName",
            "Operator": "starts with",
            "Value": "{{prefix}}"
          }
        ]
      }
      """;

    try
    {
      var expected = dbContext.Users
        .Where(u => u.FirstName.StartsWith(prefix))
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
  /// Operator <c>as lower starts with</c>.
  /// </summary>
  public void AsLowerStartsWith(Log log)
  {
    using var dbContext = new AppDbContext();

    const string prefix = "josi";
    var filterJson =
      $$"""
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "firstName",
            "Operator": "as lower starts with",
            "Value": "{{prefix}}"
          }
        ]
      }
      """;

    try
    {
      var expected = dbContext.Users
        .Where(u => u.FirstName.ToLower().StartsWith(prefix))
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
  /// Operator <c>as upper starts with</c>.
  /// </summary>
  public void AsUpperStartsWith(Log log)
  {
    using var dbContext = new AppDbContext();

    const string prefix = "JOSI";
    var filterJson =
      $$"""
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "firstName",
            "Operator": "as upper starts with",
            "Value": "{{prefix}}"
          }
        ]
      }
      """;

    try
    {
      var expected = dbContext.Users
        .Where(u => u.FirstName.ToUpper().StartsWith(prefix))
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
  /// Operator <c>ends with</c>.
  /// </summary>
  public void EndsWith(Log log)
  {
    using var dbContext = new AppDbContext();

    const string postfix = "Korea";
    var filterJson =
      $$"""
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "country",
            "Operator": "ends with",
            "Value": "{{postfix}}"
          }
        ]
      }
      """;

    try
    {
      var expected = dbContext.Users
        .Where(u => u.Country.EndsWith(postfix))
        .ToArray();

      var actual = dbContext.Users
        .Where(JsonLinq.ParseToFilterExpression<User>(filterJson))
        .ToArray();

      Assert.True(expected.Length > 0);
      Assert.Equal(expected.Length, actual.Length);
      Assert.Equal(expected.Select(u => u.Id), actual.Select(u => u.Id));

      log.Add($"Filter:\n{filterJson}");
      log.AddSep();

      log.Add("Found countries:");
      foreach (var u in actual.DistinctBy(u => u.Country))
      {
        log.Add(u.Country);
      }

      log.Add(NoteString, textColor: ConsoleColor.DarkCyan);
    }
    catch (Exception ex)
    {
      log.Add(ex.Message, LogLevel.Error);
    }
  }

  /// <summary>
  /// Operator <c>as lower ends with</c>.
  /// </summary>
  public void AsLowerEndsWith(Log log)
  {
    using var dbContext = new AppDbContext();

    const string postfix = "korea";
    var filterJson =
      $$"""
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "country",
            "Operator": "as lower ends with",
            "Value": "{{postfix}}"
          }
        ]
      }
      """;

    try
    {
      var expected = dbContext.Users
        .Where(u => u.Country.ToLower().EndsWith(postfix))
        .ToArray();

      var actual = dbContext.Users
        .Where(JsonLinq.ParseToFilterExpression<User>(filterJson))
        .ToArray();

      Assert.True(expected.Length > 0);
      Assert.Equal(expected.Length, actual.Length);
      Assert.Equal(expected.Select(u => u.Id), actual.Select(u => u.Id));

      log.Add($"Filter:\n{filterJson}");
      log.AddSep();

      log.Add("Found countries:");
      foreach (var u in actual.DistinctBy(u => u.Country))
      {
        log.Add(u.Country);
      }

      log.Add(NoteString, textColor: ConsoleColor.DarkCyan);
    }
    catch (Exception ex)
    {
      log.Add(ex.Message, LogLevel.Error);
    }
  }

  /// <summary>
  /// Operator <c>as upper ends with</c>.
  /// </summary>
  public void AsUpperEndsWith(Log log)
  {
    using var dbContext = new AppDbContext();

    const string postfix = "KOREA";
    var filterJson =
      $$"""
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "country",
            "Operator": "as upper ends with",
            "Value": "{{postfix}}"
          }
        ]
      }
      """;

    try
    {
      var expected = dbContext.Users
        .Where(u => u.Country.ToUpper().EndsWith(postfix))
        .ToArray();

      var actual = dbContext.Users
        .Where(JsonLinq.ParseToFilterExpression<User>(filterJson))
        .ToArray();

      Assert.True(expected.Length > 0);
      Assert.Equal(expected.Length, actual.Length);
      Assert.Equal(expected.Select(u => u.Id), actual.Select(u => u.Id));

      log.Add($"Filter:\n{filterJson}");
      log.AddSep();

      log.Add("Found countries:");
      foreach (var u in actual.DistinctBy(u => u.Country))
      {
        log.Add(u.Country);
      }

      log.Add(NoteString, textColor: ConsoleColor.DarkCyan);
    }
    catch (Exception ex)
    {
      log.Add(ex.Message, LogLevel.Error);
    }
  }

  /// <summary>
  /// Operator <c>contains</c>.
  /// </summary>
  public void Contains(Log log)
  {
    using var dbContext = new AppDbContext();

    const string infix = "State";
    var filterJson =
      $$"""
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "country",
            "Operator": "contains",
            "Value": "{{infix}}"
          }
        ]
      }
      """;

    try
    {
      var expected = dbContext.Users
        .Where(u => u.Country.Contains(infix))
        .ToArray();

      var actual = dbContext.Users
        .Where(JsonLinq.ParseToFilterExpression<User>(filterJson))
        .ToArray();

      Assert.True(expected.Length > 0);
      Assert.Equal(expected.Length, actual.Length);
      Assert.Equal(expected.Select(u => u.Id), actual.Select(u => u.Id));

      log.Add($"Filter:\n{filterJson}");
      log.AddSep();

      log.Add("Found countries:");
      foreach (var u in actual.DistinctBy(u => u.Country))
      {
        log.Add(u.Country);
      }

      log.Add(NoteString, textColor: ConsoleColor.DarkCyan);
    }
    catch (Exception ex)
    {
      log.Add(ex.Message, LogLevel.Error);
    }
  }

  /// <summary>
  /// Operator <c>as lower contains</c>.
  /// </summary>
  public void AsLowerContains(Log log)
  {
    using var dbContext = new AppDbContext();

    const string infix = "state";
    var filterJson =
      $$"""
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "country",
            "Operator": "as lower contains",
            "Value": "{{infix}}"
          }
        ]
      }
      """;

    try
    {
      var expected = dbContext.Users
        .Where(u => u.Country.ToLower().Contains(infix))
        .ToArray();

      var actual = dbContext.Users
        .Where(JsonLinq.ParseToFilterExpression<User>(filterJson))
        .ToArray();

      Assert.True(expected.Length > 0);
      Assert.Equal(expected.Length, actual.Length);
      Assert.Equal(expected.Select(u => u.Id), actual.Select(u => u.Id));

      log.Add($"Filter:\n{filterJson}");
      log.AddSep();

      log.Add("Found countries:");
      foreach (var u in actual.DistinctBy(u => u.Country))
      {
        log.Add(u.Country);
      }

      log.Add(NoteString, textColor: ConsoleColor.DarkCyan);
    }
    catch (Exception ex)
    {
      log.Add(ex.Message, LogLevel.Error);
    }
  }

  /// <summary>
  /// Operator <c>as upper contains</c>.
  /// </summary>
  public void AsUpperContains(Log log)
  {
    using var dbContext = new AppDbContext();

    const string infix = "STATE";
    var filterJson =
      $$"""
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "country",
            "Operator": "as upper contains",
            "Value": "{{infix}}"
          }
        ]
      }
      """;

    try
    {
      var expected = dbContext.Users
        .Where(u => u.Country.ToUpper().Contains(infix))
        .ToArray();

      var actual = dbContext.Users
        .Where(JsonLinq.ParseToFilterExpression<User>(filterJson))
        .ToArray();

      Assert.True(expected.Length > 0);
      Assert.Equal(expected.Length, actual.Length);
      Assert.Equal(expected.Select(u => u.Id), actual.Select(u => u.Id));

      log.Add($"Filter:\n{filterJson}");
      log.AddSep();

      log.Add("Found countries:");
      foreach (var u in actual.DistinctBy(u => u.Country))
      {
        log.Add(u.Country);
      }

      log.Add(NoteString, textColor: ConsoleColor.DarkCyan);
    }
    catch (Exception ex)
    {
      log.Add(ex.Message, LogLevel.Error);
    }
  }

  /// <summary>
  /// Operator <c>!in</c>.
  /// </summary>
  public void NotIn(Log log)
  {
    using var dbContext = new AppDbContext();

    var filterJson =
      """
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "id",
            "Operator": "not in",
            "Value": [1, 2, 3]
          }
        ]
      }
      """;

    try
    {
      var query = dbContext.Users
        .Where(JsonLinq.ParseToFilterExpression<User>(filterJson))
        .OrderBy(u => u.Id);

      var total = dbContext.Users.Count();
      var found = query.Count();
      var firstId = query.First().Id;

      Assert.Equal(total - 3, found);
      Assert.Equal(4, firstId);

      log.Add($"Filter:\n{filterJson}");
      log.AddSep();
      log.Add($"Total: {total}");
      log.Add($"Found: {found}");
      log.Add($"First found id: {firstId}");
    }
    catch (Exception ex)
    {
      log.Add(ex.Message, LogLevel.Error);
    }
  }

  /// <summary>
  /// Operator <c>!contains</c>.
  /// </summary>
  public void NotContains(Log log)
  {
    using var dbContext = new AppDbContext();

    const string infix = "Islands";
    var filterJson =
      $$"""
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "country",
            "Operator": "!contains",
            "Value": "{{infix}}"
          }
        ]
      }
      """;

    try
    {
      var total = dbContext.Users.Count();
      var expected = dbContext.Users.Count(u => !u.Country.Contains(infix));
      var actual = dbContext.Users.Count(JsonLinq.ParseToFilterExpression<User>(filterJson));

      log.Add($"Filter:\n{filterJson}");
      log.AddSep();
      log.Add($"Total: {total}");
      log.Add($"Found: {actual}");
      log.Add(NoteString, textColor: ConsoleColor.DarkCyan);
    }
    catch (Exception ex)
    {
      log.Add(ex.Message, LogLevel.Error);
    }
  }

  /// <summary>
  /// Operator <c>like</c> - <see cref="DbFunctionsExtensions.Like"/>.
  /// </summary>
  public void Like(Log log)
  {
    JsonLinq.Configure(options =>
    {
      options.OperatorMapper = ExpressionOperatorMapper.OnDefault()
        .Add("like", (element, pattern) =>
          Expression.Call(
            typeof(DbFunctionsExtensions).GetMethod(
              nameof(DbFunctionsExtensions.Like),
              [typeof(DbFunctions), typeof(string), typeof(string)]),
            Expression.Constant(EF.Functions),
            element,
            pattern));
    });

    using var dbContext = new AppDbContext();

    var filterJson1 =
      """
      {
        "Logic": "&&",
        "Rules": [
          {
            "Field": "email",
            "Operator": "like",
            "Value": "%.com"
          }
        ]
      }
      """;
    var filterJson2 =
      """
      {
        "Logic": "&&",
        "Rules": [
          {
            "Field": "email",
            "Operator": "like",
            "Value": "%.COM"
          }
        ]
      }
      """;

    try
    {
      var expectedCount1 = dbContext.Users.Count(u => EF.Functions.Like(u.Email, "%.com"));
      var expectedCount2 = dbContext.Users.Count(u => EF.Functions.Like(u.Email, "%.COM"));

      var actualCount1 = dbContext.Users.Count(JsonLinq.ParseToFilterExpression<User>(filterJson1));
      var actualCount2 = dbContext.Users.Count(JsonLinq.ParseToFilterExpression<User>(filterJson2));

      Assert.Equal(expectedCount1, actualCount1);
      Assert.Equal(expectedCount2, actualCount2);

      log.Add($"Filter 1:\n{filterJson1}");
      log.Add($"Filter 2:\n{filterJson2}");

      log.AddSep();

      log.Add("Count:");
      log.Add($"\"%.com\": {actualCount1}");
      log.Add($"\"%.COM\": {actualCount2}");
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

  /// <summary>
  /// Operator <c>ilike</c> - <see cref="NpgsqlDbFunctionsExtensions.ILike"/>.
  /// </summary>
  public void ILike(Log log)
  {
    JsonLinq.Configure(options =>
    {
      options.OperatorMapper = ExpressionOperatorMapper.OnDefault()
        .Add("ilike", (element, pattern) =>
          Expression.Call(
            typeof(NpgsqlDbFunctionsExtensions).GetMethod(
              nameof(NpgsqlDbFunctionsExtensions.ILike),
              [typeof(DbFunctions), typeof(string), typeof(string)]),
            Expression.Constant(EF.Functions),
            element,
            pattern));
    });

    using var dbContext = new AppDbContext();

    var filterJson1 =
      """
      {
        "Logic": "&&",
        "Rules": [
          {
            "Field": "email",
            "Operator": "ilike",
            "Value": "%.com"
          }
        ]
      }
      """;
    var filterJson2 =
      """
      {
        "Logic": "&&",
        "Rules": [
          {
            "Field": "email",
            "Operator": "ilike",
            "Value": "%.COM"
          }
        ]
      }
      """;

    try
    {
      var expectedCount1 = dbContext.Users.Count(u => EF.Functions.ILike(u.Email, "%.com"));
      var expectedCount2 = dbContext.Users.Count(u => EF.Functions.ILike(u.Email, "%.COM"));

      var actualCount1 = dbContext.Users.Count(JsonLinq.ParseToFilterExpression<User>(filterJson1));
      var actualCount2 = dbContext.Users.Count(JsonLinq.ParseToFilterExpression<User>(filterJson2));

      Assert.Equal(expectedCount1, actualCount1);
      Assert.Equal(expectedCount2, actualCount2);

      log.Add($"Filter 1:\n{filterJson1}");
      log.Add($"Filter 2:\n{filterJson2}");

      log.AddSep();

      log.Add("Count:");
      log.Add($"\"%.com\": {actualCount1}");
      log.Add($"\"%.COM\": {actualCount2}");
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

  /// <summary>
  /// Emoji fields.
  /// </summary>
  public void EmojiFields(Log log)
  {
    using var dbContext = new AppDbContext();

    JsonLinq.Configure(options =>
    {
      options.LogicOperatorPropertyName = "🔗";
      options.RulesPropertyName = "⚖️";
      options.OperatorPropertyName = "⚡";
      options.FieldPropertyName = "🍁";
      options.ValuePropertyName = "🍬";
    });

    var filterJson =
      """
      {
        "🔗": "&&",
        "⚖️": [
          {
            "🍁": "lastVisitAt",
            "⚡": "=",
            "🍬": null
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
