using System.Linq.Expressions;
using static Neomaster.JsonToLinq.JsonLinqConsts;

namespace Neomaster.JsonToLinq.UnitTests;

public class ExpressionOperatorMapperUnitTests
  : UnitTestsBase
{
  [Fact]
  public void OnDefault()
  {
    var mapper = ExpressionOperatorMapper.OnDefault();

    Assert.NotEqual(ExpressionOperatorMappers.Default, mapper);
    Assert.Equal(ExpressionOperatorMappers.Default.Pairs, mapper.Pairs);
    Assert.NotStrictEqual(ExpressionOperatorMappers.Default.Pairs, mapper.Pairs);
  }

  [Fact]
  public void Clone()
  {
    var m1 = new ExpressionOperatorMapper();

    var m2 = m1.Clone();

    Assert.NotEqual(m1, m2);
    Assert.NotStrictEqual(m1.Pairs, m2.Pairs);
  }

  [Fact]
  public void FluentAdd()
  {
    var m1 = new ExpressionOperatorMapper();
    var expectedPairs = new Dictionary<string, ExpressionBind>
    {
      ["&"] = Expression.And,
      ["|"] = Expression.Or,
    };

    var m2 = m1
      .Add("&", Expression.And)
      .Add("|", Expression.Or);

    Assert.Equal(m1, m2);
    Assert.Equal(expectedPairs, m1.Pairs);
  }

  [Fact]
  public void FluentAdd_Duplicate()
  {
    const string key = "op";
    var mapper = new ExpressionOperatorMapper().Add(key, default);
    var expectedExMessage = string.Format(ErrorMessages.OperatorRegistered, key);

    var ex = Assert.Throws<InvalidOperationException>(() => mapper.Add(key, default));
    Assert.Equal(expectedExMessage, ex.Message);
  }

  [Fact]
  public void AddAlias()
  {
    var mapper = ExpressionOperatorMapper.OnDefault()
      .AddAlias("1", "=")
      .AddAlias("2", "=");

    var bind = mapper["="];
    var bind1 = mapper["1"];
    var bind2 = mapper["2"];

    Assert.Equal(bind, bind1);
    Assert.Equal(bind, bind2);
  }
}
