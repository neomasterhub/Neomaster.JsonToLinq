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

    Assert.Equal(source, mapper.Operators);
    Assert.NotStrictEqual(source, mapper.Operators);
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
    Assert.Equal(source.Operators, mapper.Operators);
    Assert.NotStrictEqual(source.Operators, mapper.Operators);
  }

  [Fact]
  public void Ctor_OnDefault()
  {
    var mapper = ExpressionOperatorMapper.OnDefault();

    Assert.NotEqual(ExpressionOperatorMappers.Default, mapper);
    Assert.Equal(ExpressionOperatorMappers.Default.Operators, mapper.Operators);
    Assert.NotStrictEqual(ExpressionOperatorMappers.Default.Operators, mapper.Operators);
  }

  [Fact]
  public void Ctor_Add()
  {
    var mapper = ExpressionOperatorMapper.OnDefault();
    var prevOperators = mapper.Clone().Operators;
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
      Assert.True(mapper.Operators.ContainsKey(newPair.Key));
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
}
