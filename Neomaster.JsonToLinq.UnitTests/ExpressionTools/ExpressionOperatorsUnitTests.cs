using System.Linq.Expressions;

namespace Neomaster.JsonToLinq.UnitTests;

public class ExpressionOperatorsUnitTests
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

    var actual = users.Where(CreateContainsFunc<User, int>(nameof(User.Id), collection)).ToArray();

    Assert.Equal(expected, actual);
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

    var actual = users.Where(CreateContainsFunc<User, DateTime?>(nameof(User.LastVisitAt), collection)).ToArray();

    Assert.Equal(expected, actual);
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
}
