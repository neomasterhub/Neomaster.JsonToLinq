using System.Linq.Expressions;

namespace Neomaster.JsonToLinq.UnitTests;

public class ExpressionOperatorsUnitTests
{
  [Fact]
  public void Contains()
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
    var param = Expression.Parameter(typeof(User));
    var left = Expression.Constant(collection);
    var right = Expression.Property(param, nameof(User.Id));
    var body = ExpressionOperators.Contains(left, right);
    var lambda = Expression.Lambda<Func<User, bool>>(body, param);
    var func = lambda.Compile();

    var actual = users.Where(func).ToArray();

    Assert.Equal(expected, actual);
  }
}
