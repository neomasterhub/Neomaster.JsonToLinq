using System.Linq.Expressions;
using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq.UnitTests;

public class ExpressionOperatorMapperUnitTests
{
  [Fact]
  public void Ctor_Source_Dictionary()
  {
    var source = new Dictionary<string, ExpressionBind>
    {
      ["&"] = Expression.And,
      ["|"] = Expression.Or,
    };

    var mapper = new ExpressionOperatorMapper(source);

    Assert.Equal(source, mapper.Pairs);
    Assert.NotStrictEqual(source, mapper.Pairs);
  }

  [Fact]
  public void Ctor_Source_Mapper()
  {
    var source = new ExpressionOperatorMapper(
      new Dictionary<string, ExpressionBind>
      {
        ["&"] = Expression.And,
        ["|"] = Expression.Or,
      });

    var mapper = new ExpressionOperatorMapper(source);

    Assert.NotEqual(source, mapper);
    Assert.Equal(source.Pairs, mapper.Pairs);
    Assert.NotStrictEqual(source.Pairs, mapper.Pairs);
  }

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
  public void Ctor_Add()
  {
    var mapper = ExpressionOperatorMapper.OnDefault();
    var prevOperators = mapper.Clone().Pairs;
    var newPairs = new Dictionary<string, ExpressionBind>
    {
      { "k1", Expression.Equal },
      { "k2", Expression.NotEqual },
    };

    foreach (var newPair in newPairs)
    {
      mapper.Add(newPair.Key, newPair.Value);
    }

    Assert.All(newPairs, newPair =>
    {
      Assert.False(prevOperators.ContainsKey(newPair.Key));
      Assert.True(mapper.Pairs.ContainsKey(newPair.Key));
      Assert.Equal(newPair.Value, mapper[newPair.Key]);
    });
  }

  [Fact]
  public void Ctor_Add_Duplicate()
  {
    const string key = nameof(key);
    var mapper = new ExpressionOperatorMapper().Add(key, default);
    var expectedExMessage = string.Format(ErrorMessages.OperatorRegistered, key);

    var ex = Assert.Throws<InvalidOperationException>(() => mapper.Add(key, default));
    Assert.Equal(expectedExMessage, ex.Message);
  }

  [Fact]
  public void TryGet()
  {
    var mapper = new ExpressionOperatorMapper().Add("&", Expression.And);

    var andSuccess = mapper.TryGet("&", out var andExpr);
    var orSuccess = mapper.TryGet("|", out var orExpr);

    Assert.True(andSuccess);
    Assert.Equal(Expression.And, andExpr);
    Assert.False(orSuccess);
    Assert.Null(orExpr);
  }
}
