using System.Text.Json;
using Xunit.Abstractions;

namespace Neomaster.JsonToLinq.UnitTests;

public class JsonLinqUnitTests : UnitTestsBase
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

  private readonly ITestOutputHelper _output;
  private readonly IReadOnlyList<User> _users;
  private readonly IReadOnlyList<User> _usersNull = null;
  private readonly IReadOnlyList<User> _usersEmpty = [];
  private readonly Func<User, bool> _filterFunc;
  private readonly JsonDocument _filterJsonDocument;

  public JsonLinqUnitTests(ITestOutputHelper output)
  {
    _output = output;

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
      _output.WriteLine($"{a.Balance}");
    }
  }

  [Fact]
  public void Where_ArgumentExceptions()
  {
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.Where(_filterJson));
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.Where(_filterJsonDocument));

    TestArgumentException<ArgumentNullException>("filter", () => _users.Where((JsonDocument)null));
    TestArgumentException<ArgumentNullException>("filterJson", () => _users.Where((string)null));

    TestArgumentException<ArgumentException>("filterJson", () => _users.Where(string.Empty));
    TestArgumentException<ArgumentException>("filterJson", () => _users.Where(" "));
  }

  [Fact]
  public void First()
  {
    var expected = _users.First(_filterFunc);

    var actual1 = _users.First(_filterJson);
    var actual2 = _users.First(_filterJsonDocument);

    Assert.Equal(expected, actual1);
    Assert.Equal(expected, actual2);

    _output.WriteLine($"{actual1.Balance}");
  }

  [Fact]
  public void First_ArgumentExceptions()
  {
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.First(_filterJson));
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.First(_filterJsonDocument));

    TestArgumentException<ArgumentNullException>("filter", () => _users.First((JsonDocument)null));
    TestArgumentException<ArgumentNullException>("filterJson", () => _users.First((string)null));

    TestArgumentException<ArgumentException>("filterJson", () => _users.First(string.Empty));
    TestArgumentException<ArgumentException>("filterJson", () => _users.First(" "));
  }

  [Fact]
  public void FirstOrDefault()
  {
    foreach (var user in _users)
    {
      user.Balance += 1000;
    }

    Assert.Null(_users.FirstOrDefault(_filterJson));
    Assert.Null(_users.FirstOrDefault(_filterJsonDocument));
    Assert.Null(_usersEmpty.FirstOrDefault(_filterJson));
    Assert.Null(_usersEmpty.FirstOrDefault(_filterJsonDocument));
  }

  [Fact]
  public void FirstOrDefault_ArgumentExceptions()
  {
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.FirstOrDefault(_filterJson));
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.FirstOrDefault(_filterJsonDocument));

    TestArgumentException<ArgumentNullException>("filter", () => _users.FirstOrDefault((JsonDocument)null));
    TestArgumentException<ArgumentNullException>("filterJson", () => _users.FirstOrDefault((string)null));

    TestArgumentException<ArgumentException>("filterJson", () => _users.FirstOrDefault(string.Empty));
    TestArgumentException<ArgumentException>("filterJson", () => _users.FirstOrDefault(" "));
  }

  [Fact]
  public void Last()
  {
    var expected = _users.Last(_filterFunc);

    var actual1 = _users.Last(_filterJson);
    var actual2 = _users.Last(_filterJsonDocument);

    Assert.Equal(expected, actual1);
    Assert.Equal(expected, actual2);

    _output.WriteLine($"{actual1.Balance}");
  }

  [Fact]
  public void Last_ArgumentExceptions()
  {
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.Last(_filterJson));
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.Last(_filterJsonDocument));

    TestArgumentException<ArgumentNullException>("filter", () => _users.Last((JsonDocument)null));
    TestArgumentException<ArgumentNullException>("filterJson", () => _users.Last((string)null));

    TestArgumentException<ArgumentException>("filterJson", () => _users.Last(string.Empty));
    TestArgumentException<ArgumentException>("filterJson", () => _users.Last(" "));
  }

  [Fact]
  public void LastOrDefault()
  {
    foreach (var user in _users)
    {
      user.Balance += 1000;
    }

    Assert.Null(_users.LastOrDefault(_filterJson));
    Assert.Null(_users.LastOrDefault(_filterJsonDocument));
    Assert.Null(_usersEmpty.LastOrDefault(_filterJson));
    Assert.Null(_usersEmpty.LastOrDefault(_filterJsonDocument));
  }

  [Fact]
  public void LastOrDefault_ArgumentExceptions()
  {
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.LastOrDefault(_filterJson));
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.LastOrDefault(_filterJsonDocument));

    TestArgumentException<ArgumentNullException>("filter", () => _users.LastOrDefault((JsonDocument)null));
    TestArgumentException<ArgumentNullException>("filterJson", () => _users.LastOrDefault((string)null));

    TestArgumentException<ArgumentException>("filterJson", () => _users.LastOrDefault(string.Empty));
    TestArgumentException<ArgumentException>("filterJson", () => _users.LastOrDefault(" "));
  }

  [Fact]
  public void Any_ArgumentExceptions()
  {
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.Any(_filterJson));
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.Any(_filterJsonDocument));

    TestArgumentException<ArgumentNullException>("filter", () => _users.Any((JsonDocument)null));
    TestArgumentException<ArgumentNullException>("filterJson", () => _users.Any((string)null));

    TestArgumentException<ArgumentException>("filterJson", () => _users.Any(string.Empty));
    TestArgumentException<ArgumentException>("filterJson", () => _users.Any(" "));
  }

  [Fact]
  public void All_ArgumentExceptions()
  {
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.All(_filterJson));
    TestArgumentException<ArgumentNullException>("elements", () => _usersNull.All(_filterJsonDocument));

    TestArgumentException<ArgumentNullException>("filter", () => _users.All((JsonDocument)null));
    TestArgumentException<ArgumentNullException>("filterJson", () => _users.All((string)null));

    TestArgumentException<ArgumentException>("filterJson", () => _users.All(string.Empty));
    TestArgumentException<ArgumentException>("filterJson", () => _users.All(" "));
  }

  private static void TestArgumentException<TArgumentException>(string expectedParamName, Action action)
    where TArgumentException : ArgumentException
  {
    var ex = Assert.Throws<TArgumentException>(() => action());
    Assert.Equal(expectedParamName, ex.ParamName);
  }
}
