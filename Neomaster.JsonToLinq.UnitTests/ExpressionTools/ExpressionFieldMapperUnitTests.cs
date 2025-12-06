using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq.UnitTests;

public class ExpressionFieldMapperUnitTests
{
  [Fact]
  public void Clone()
  {
    var m1 = new ExpressionFieldMapper();

    var m2 = m1.Clone();

    Assert.NotEqual(m1, m2);
    Assert.NotStrictEqual(m1.Pairs, m2.Pairs);
  }

  [Fact]
  public void FluentAdd()
  {
    var m1 = new ExpressionFieldMapper();
    var f1 = new ExpressionField();
    var f2 = new ExpressionField();
    var expectedPairs = new Dictionary<string, ExpressionField>
    {
      ["f1"] = f1,
      ["f2"] = f2,
    };

    var m2 = m1
      .Add("f1", f1)
      .Add("f2", f2);

    Assert.Equal(m1, m2);
    Assert.Equal(expectedPairs, m1.Pairs);
  }

  [Fact]
  public void FluentAdd_Duplicate()
  {
    const string key = "f";
    var mapper = new ExpressionFieldMapper().Add(key, default);
    var expectedExMessage = string.Format(ErrorMessages.SourceFieldRegistered, key);

    var ex = Assert.Throws<InvalidOperationException>(() => mapper.Add(key, default));
    Assert.Equal(expectedExMessage, ex.Message);
  }
}
