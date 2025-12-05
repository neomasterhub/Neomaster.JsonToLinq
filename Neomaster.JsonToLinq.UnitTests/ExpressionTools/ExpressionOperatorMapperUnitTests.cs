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
}
