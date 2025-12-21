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
    var mapper = new ExpressionOperatorMapper()
      .Add("=", Expression.Equal)
      .AddAlias("eq", "=")
      .AddAlias("EQ", "=");

    Assert.Contains("eq", mapper.Pairs.Keys);
    Assert.Contains("EQ", mapper.Pairs.Keys);
    Assert.Equal(mapper["="], mapper["eq"]);
    Assert.Equal(mapper["="], mapper["EQ"]);
  }

  [Fact]
  public void WithAliases()
  {
    var mapper = new ExpressionOperatorMapper()
      .Add("=", Expression.Equal)
      .WithAliases("eq", "EQ")
      .Add("!=", Expression.NotEqual);

    Assert.Contains("eq", mapper.Pairs.Keys);
    Assert.Contains("EQ", mapper.Pairs.Keys);
    Assert.Equal(mapper["="], mapper["eq"]);
    Assert.Equal(mapper["="], mapper["EQ"]);
    Assert.Single(mapper.Pairs, kv => kv.Value == Expression.NotEqual);
  }

  [Fact]
  public void AddNot()
  {
    var mapper = new ExpressionOperatorMapper()
      .Add("!=", Expression.NotEqual)
      .AddNot("=", "!=");
    var par = Expression.Parameter(typeof(int));
    var func = Expression.Lambda<Func<int, bool>>(
      mapper["="](par, Expression.Constant(1)),
      par)
      .Compile();

    Assert.Contains("=", mapper.Pairs.Keys);
    Assert.NotEqual(mapper["!="], mapper["="]);
    Assert.True(func(1));
  }

  [Fact]
  public void AddNot_AutoPrefix()
  {
    var mapper = new ExpressionOperatorMapper()
      .Add("a", null)
      .Add("b c", null)
      .AddNot("a")
      .AddNot("b c");

    Assert.Contains("!a", mapper.Pairs.Keys);
    Assert.Contains("! b c", mapper.Pairs.Keys);
  }

  [Fact]
  public void WithNot()
  {
    var mapper = new ExpressionOperatorMapper()
      .Add("!=", Expression.NotEqual)
      .WithNot("=")
      .Add("&", Expression.And);
    var par = Expression.Parameter(typeof(int));
    var func = Expression.Lambda<Func<int, bool>>(
      mapper["="](par, Expression.Constant(1)),
      par)
      .Compile();

    Assert.Contains("=", mapper.Pairs.Keys);
    Assert.NotEqual(mapper["!="], mapper["="]);
    Assert.True(func(1));
    Assert.Single(mapper.Pairs, kv => kv.Value == Expression.NotEqual);
    Assert.Single(mapper.Pairs, kv => kv.Value == Expression.And);
  }

  [Fact]
  public void WithNot_AutoPrefix()
  {
    var mapper = new ExpressionOperatorMapper()
      .Add("a", null)
      .WithNot()
      .Add("b c", null)
      .WithNot();

    Assert.Contains("!a", mapper.Pairs.Keys);
    Assert.Contains("! b c", mapper.Pairs.Keys);
  }
}
