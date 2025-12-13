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
  private static readonly IReadOnlyList<User> _usersNull = null;
  private static readonly IReadOnlyList<User> _usersEmpty = [];
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

  [Fact]
  public void Where_ArgumentExceptions()
  {
    TestArgumentNullException("elements", () => _usersNull.Where(_filterJson));
    TestArgumentNullException("elements", () => _usersNull.Where(_filterJsonDocument));

    TestArgumentNullException("filter", () => _users.Where((JsonDocument)null));
    TestArgumentNullException("filterJson", () => _users.Where((string)null));
    TestArgumentNullException("filterJson", () => _users.Where(string.Empty));
    TestArgumentNullException("filterJson", () => _users.Where(" "));
  }

  [Fact]
  public void First()
  {
    var expected = _users.First(_filterFunc);

    var actual1 = _users.First(_filterJson);
    var actual2 = _users.First(_filterJsonDocument);

    Assert.Equal(expected, actual1);
    Assert.Equal(expected, actual2);

    output.WriteLine($"{actual1.Balance}");
  }

  [Fact]
  public void First_ArgumentExceptions()
  {
    TestArgumentNullException("elements", () => _usersNull.First(_filterJson));
    TestArgumentNullException("elements", () => _usersNull.First(_filterJsonDocument));

    TestArgumentNullException("filter", () => _users.First((JsonDocument)null));
    TestArgumentNullException("filterJson", () => _users.First((string)null));
    TestArgumentNullException("filterJson", () => _users.First(string.Empty));
    TestArgumentNullException("filterJson", () => _users.First(" "));
  }

  [Fact]
  public void FirstOrDefault_ArgumentExceptions()
  {
    TestArgumentNullException("elements", () => _usersNull.FirstOrDefault(_filterJson));
    TestArgumentNullException("elements", () => _usersNull.FirstOrDefault(_filterJsonDocument));

    TestArgumentNullException("filter", () => _users.FirstOrDefault((JsonDocument)null));
    TestArgumentNullException("filterJson", () => _users.FirstOrDefault((string)null));
    TestArgumentNullException("filterJson", () => _users.FirstOrDefault(string.Empty));
    TestArgumentNullException("filterJson", () => _users.FirstOrDefault(" "));
  }

  [Fact]
  public void Last_ArgumentExceptions()
  {
    TestArgumentNullException("elements", () => _usersNull.Last(_filterJson));
    TestArgumentNullException("elements", () => _usersNull.Last(_filterJsonDocument));

    TestArgumentNullException("filter", () => _users.Last((JsonDocument)null));
    TestArgumentNullException("filterJson", () => _users.Last((string)null));
    TestArgumentNullException("filterJson", () => _users.Last(string.Empty));
    TestArgumentNullException("filterJson", () => _users.Last(" "));
  }

  [Fact]
  public void LastOrDefault_ArgumentExceptions()
  {
    TestArgumentNullException("elements", () => _usersNull.LastOrDefault(_filterJson));
    TestArgumentNullException("elements", () => _usersNull.LastOrDefault(_filterJsonDocument));

    TestArgumentNullException("filter", () => _users.LastOrDefault((JsonDocument)null));
    TestArgumentNullException("filterJson", () => _users.LastOrDefault((string)null));
    TestArgumentNullException("filterJson", () => _users.LastOrDefault(string.Empty));
    TestArgumentNullException("filterJson", () => _users.LastOrDefault(" "));
  }

  [Fact]
  public void Any_ArgumentExceptions()
  {
    TestArgumentNullException("elements", () => _usersNull.Any(_filterJson));
    TestArgumentNullException("elements", () => _usersNull.Any(_filterJsonDocument));

    TestArgumentNullException("filter", () => _users.Any((JsonDocument)null));
    TestArgumentNullException("filterJson", () => _users.Any((string)null));
    TestArgumentNullException("filterJson", () => _users.Any(string.Empty));
    TestArgumentNullException("filterJson", () => _users.Any(" "));
  }

  [Fact]
  public void All_ArgumentExceptions()
  {
    TestArgumentNullException("elements", () => _usersNull.All(_filterJson));
    TestArgumentNullException("elements", () => _usersNull.All(_filterJsonDocument));

    TestArgumentNullException("filter", () => _users.All((JsonDocument)null));
    TestArgumentNullException("filterJson", () => _users.All((string)null));
    TestArgumentNullException("filterJson", () => _users.All(string.Empty));
    TestArgumentNullException("filterJson", () => _users.All(" "));
  }

  private static void TestArgumentNullException(string expectedParamName, Action action)
  {
    var ex = Assert.Throws<ArgumentNullException>(() => action());
    Assert.Equal(expectedParamName, ex.ParamName);
  }
}
