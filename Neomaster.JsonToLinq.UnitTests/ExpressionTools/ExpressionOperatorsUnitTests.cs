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

  private static Func<TEntity, bool> CreateContainsFunc<TEntity, TProp>(
    string propName,
    IEnumerable<TProp> collection)
  {
    var par = Expression.Parameter(typeof(TEntity));
    var left = Expression.Constant(collection);
    var right = Expression.Property(par, propName);
    var body = ExpressionOperators.Contains(left, right);
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
    var col = Expression.Constant(collection);
    var element = Expression.Property(par, propName);
    var body = ExpressionOperators.ContainsString(col, element, stringTransform, cultureInfo);
    var lambda = Expression.Lambda<Func<TEntity, bool>>(body, par);
    var func = lambda.Compile();

    return func;
  }
}
