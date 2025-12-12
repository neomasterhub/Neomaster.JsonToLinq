using System.Text.Json;
using Xunit.Abstractions;

namespace Neomaster.JsonToLinq.UnitTests;

public class JsonLinqUnitTests(ITestOutputHelper output)
{
  private const string _filterJson =
    """
    {
      "Logic": "&&",
      "Rules": [
        {
          "Field": "balance",
          "Operator": ">",
          "Value": 0
        },
        {
          "Field": "balance",
          "Operator": "<",
          "Value": 100
        }
      ]
    }
    """;

  private static readonly IReadOnlyList<User> _users;
  private static readonly Func<User, bool> _filterFunc;
  private static readonly JsonDocument _filterJsonDocument;

  static JsonLinqUnitTests()
  {
    _users =
    [
      new() { Balance = 0 },
      new() { Balance = 10 },
      new() { Balance = 20 },
      new() { Balance = 30 },
      new() { Balance = 100 },
    ];

    _filterFunc = u => u.Balance > 0 && u.Balance < 100;
    _filterJsonDocument = JsonDocument.Parse(_filterJson);
  }

  [Fact]
  public void Where()
  {
    var expected = _users.Where(_filterFunc);

    var actual1 = _users.Where(_filterJson);
    var actual2 = _users.Where(_filterJsonDocument);

    Assert.Equal(expected, actual1);
    Assert.Equal(expected, actual2);

    foreach (var a in actual1)
    {
      output.WriteLine($"{a.Balance}");
    }
  }
}
