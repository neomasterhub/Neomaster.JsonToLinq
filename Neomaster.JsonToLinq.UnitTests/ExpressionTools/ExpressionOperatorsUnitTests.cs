using System.Globalization;
using System.Linq.Expressions;
using Xunit.Abstractions;

namespace Neomaster.JsonToLinq.UnitTests;

public class ExpressionOperatorsUnitTests(ITestOutputHelper output)
{
  [Fact]
  public void Contains_Int()
  {
    var users = new User[]
    {
      new() { Id = 1 },
      new() { Id = 2 },
      new() { Id = 3 },
      new() { Id = 4 },
    };
    var collection = new int[] { -1, 1, 3 };
    var expected = users.Where(u => collection.Contains(u.Id));

    var actual = users.Where(CreateContainsFunc<User, int>(nameof(User.Id), collection)).ToList();

    Assert.Equal(expected, actual);

    actual.ForEach(u => output.WriteLine(u.Id.ToString()));
  }

  [Fact]
  public void Contains_DateTime()
  {
    var now = DateTime.Now;
    var dt = new DateTime[]
    {
      now.AddYears(1),
      now.AddYears(2),
      now.AddYears(3),
    };
    var users = new User[]
    {
      new() { LastVisitAt = null },
      new() { LastVisitAt = dt[0] },
      new() { LastVisitAt = dt[1] },
      new() { LastVisitAt = dt[2] },
    };
    var collection = new List<DateTime?> { now, null, dt[2] };
    var expected = users.Where(u => collection.Contains(u.LastVisitAt));

    var actual = users.Where(CreateContainsFunc<User, DateTime?>(nameof(User.LastVisitAt), collection)).ToList();

    Assert.Equal(expected, actual);

    actual.ForEach(u => output.WriteLine(u.LastVisitAt?.Year.ToString() ?? "null"));
  }

  [Fact]
  public void ContainsString_AsIs()
  {
    var collection = new string[] { "x", "Aa" };
    var users = new User[]
    {
      new() { FirstName = "aa" },
      new() { FirstName = "AA" },
      new() { FirstName = "Aa" },
    };
    var expected = users.Where(u => collection.Contains(u.FirstName));
    var func = CreateContainsStringFunc<User, string>(
      nameof(User.FirstName),
      collection,
      StringTransform.None);

    var actual = users.Where(func).ToList();

    Assert.Equal(expected, actual);

    actual.ForEach(u => output.WriteLine(u.FirstName));
  }

  [Fact]
  public void ContainsString_AsLower()
  {
    var collection = new string[] { "x", "aa" };
    var users = new User[]
    {
      new() { FirstName = "aa" },
      new() { FirstName = "AA" },
      new() { FirstName = "Aa" },
      new() { FirstName = "aA" },
      new() { FirstName = "xx" },
      new() { FirstName = "XX" },
      new() { FirstName = "Xx" },
      new() { FirstName = "xX" },
    };
    var expected = users.Where(u => collection.Contains(u.FirstName.ToLower()));
    var func = CreateContainsStringFunc<User, string>(
      nameof(User.FirstName),
      collection,
      StringTransform.Lower);

    var actual = users.Where(func).ToList();

    Assert.Equal(expected, actual);

    actual.ForEach(u => output.WriteLine(u.FirstName));
  }

  [Fact]
  public void ContainsString_AsUpper()
  {
    var collection = new string[] { "x", "AA" };
    var users = new User[]
    {
      new() { FirstName = "aa" },
      new() { FirstName = "AA" },
      new() { FirstName = "Aa" },
      new() { FirstName = "aA" },
      new() { FirstName = "xx" },
      new() { FirstName = "XX" },
      new() { FirstName = "Xx" },
      new() { FirstName = "xX" },
    };
    var expected = users.Where(u => collection.Contains(u.FirstName.ToUpper()));
    var func = CreateContainsStringFunc<User, string>(
      nameof(User.FirstName),
      collection,
      StringTransform.Upper);

    var actual = users.Where(func).ToList();

    Assert.Equal(expected, actual);

    actual.ForEach(u => output.WriteLine(u.FirstName));
  }

  [Theory]
  [InlineData("i", false, 1)]
  [InlineData("i", true, 0)]
  [InlineData("ı", false, 0)]
  [InlineData("ı", true, 1)]
  public void ContainsString_AsLower_Culture(
    string collectionItem,
    bool withCi,
    int found)
  {
    var collection = new string[] { collectionItem };
    var users = new User[] { new() { FirstName = "I" } };
    var func = CreateContainsStringFunc<User, string>(
      nameof(User.FirstName),
      collection,
      StringTransform.Lower,
      withCi ? new CultureInfo("tr-TR") : null);

    var actual = users.Count(func);

    Assert.Equal(found, actual);
  }

  [Theory]
  [InlineData("I", false, 1)]
  [InlineData("I", true, 0)]
  [InlineData("İ", false, 0)]
  [InlineData("İ", true, 1)]
  public void ContainsString_AsUpper_Culture(
    string collectionItem,
    bool withCi,
    int found)
  {
    var collection = new string[] { collectionItem };
    var users = new User[] { new() { FirstName = "i" } };
    var func = CreateContainsStringFunc<User, string>(
      nameof(User.FirstName),
      collection,
      StringTransform.Upper,
      withCi ? new CultureInfo("tr-TR") : null);

    var actual = users.Count(func);

    Assert.Equal(found, actual);
  }

  [Fact]
  public void StartsWith_AsIs()
  {
    const string prefix = "a";
    var users = new User[]
    {
      new() { FirstName = "a-" },
      new() { FirstName = "A-" },
    };
    var expected = users.Where(u => u.FirstName.StartsWith(prefix));
    var func = CreateStartsWithFunc<User, string>(
      nameof(User.FirstName),
      prefix,
      StringTransform.None);

    var actual = users.Where(func).ToList();

    Assert.Equal(expected, actual);

    actual.ForEach(u => output.WriteLine(u.FirstName));
  }

  [Fact]
  public void StartsWith_AsLower()
  {
    const string prefix = "a";
    var users = new User[]
    {
      new() { FirstName = "a-" },
      new() { FirstName = "A-" },
    };
    var expected = users.Where(u => u.FirstName.ToLower().StartsWith(prefix));
    var func = CreateStartsWithFunc<User, string>(
      nameof(User.FirstName),
      prefix,
      StringTransform.Lower);

    var actual = users.Where(func).ToList();

    Assert.Equal(expected, actual);

    actual.ForEach(u => output.WriteLine(u.FirstName));
  }

  [Fact]
  public void StartsWith_AsUpper()
  {
    const string prefix = "A";
    var users = new User[]
    {
      new() { FirstName = "a-" },
      new() { FirstName = "A-" },
    };
    var expected = users.Where(u => u.FirstName.ToUpper().StartsWith(prefix));
    var func = CreateStartsWithFunc<User, string>(
      nameof(User.FirstName),
      prefix,
      StringTransform.Upper);

    var actual = users.Where(func).ToList();

    Assert.Equal(expected, actual);

    actual.ForEach(u => output.WriteLine(u.FirstName));
  }

  private static Func<TEntity, bool> CreateStartsWithFunc<TEntity, TProp>(
    string propName,
    string prefix,
    StringTransform stringTransform,
    CultureInfo cultureInfo = null)
  {
    var par = Expression.Parameter(typeof(TEntity));
    var body = ExpressionOperators.StartsWith(
      Expression.Property(par, propName),
      Expression.Constant(prefix),
      stringTransform,
      cultureInfo);
    var lambda = Expression.Lambda<Func<TEntity, bool>>(body, par);
    var func = lambda.Compile();

    return func;
  }

  private static Func<TEntity, bool> CreateContainsFunc<TEntity, TProp>(
    string propName,
    IEnumerable<TProp> collection)
  {
    var par = Expression.Parameter(typeof(TEntity));
    var body = ExpressionOperators.Contains(
      Expression.Constant(collection),
      Expression.Property(par, propName));
    var lambda = Expression.Lambda<Func<TEntity, bool>>(body, par);
    var func = lambda.Compile();

    return func;
  }

  private static Func<TEntity, bool> CreateContainsStringFunc<TEntity, TProp>(
    string propName,
    IEnumerable<TProp> collection,
    StringTransform stringTransform,
    CultureInfo cultureInfo = null)
  {
    var par = Expression.Parameter(typeof(TEntity));
    var body = ExpressionOperators.ContainsString(
      Expression.Constant(collection),
      Expression.Property(par, propName),
      stringTransform,
      cultureInfo);
    var lambda = Expression.Lambda<Func<TEntity, bool>>(body, par);
    var func = lambda.Compile();

    return func;
  }
}
