using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq.UnitTests;

public class MapperUnitTests
{
  [Fact]
  public void Ctor_Source_Dictionary()
  {
    var source = new Dictionary<int, string>
    {
      [1] = "1",
      [2] = "2",
    };

    var mapper = new Mapper<int, string>(source);

    Assert.Equal(source, mapper.Pairs);
    Assert.NotStrictEqual(source, mapper.Pairs);
  }

  [Fact]
  public void Ctor_Source_Mapper()
  {
    var source = new Mapper<int, string>(
      new Dictionary<int, string>
      {
        [1] = "1",
        [2] = "2",
      });

    var mapper = new Mapper<int, string>(source);

    Assert.NotEqual(source, mapper);
    Assert.Equal(source.Pairs, mapper.Pairs);
    Assert.NotStrictEqual(source.Pairs, mapper.Pairs);
  }

  [Fact]
  public void Ctor_Add()
  {
    var mapper = new Mapper<int, string>(
      new Dictionary<int, string>
      {
        [1] = "1",
        [2] = "2",
      });

    var prevPairs = mapper.Pairs.ToDictionary();
    var newPairs = new Dictionary<int, string>
    {
      [3] = "3",
      [4] = "4",
    };

    foreach (var newPair in newPairs)
    {
      mapper.Add(newPair.Key, newPair.Value);
    }

    Assert.All(newPairs, newPair =>
    {
      Assert.False(prevPairs.ContainsKey(newPair.Key));
      Assert.True(mapper.Pairs.ContainsKey(newPair.Key));
      Assert.Equal(newPair.Value, mapper[newPair.Key]);
    });
  }

  [Fact]
  public void Ctor_Add_Duplicate()
  {
    var mapper = new Mapper<int, string>(
      new Dictionary<int, string>
      {
        [1] = "1",
        [2] = "2",
      });
    var expectedExMessage = string.Format(ErrorMessages.KeyRegistered, "1");

    var ex = Assert.Throws<InvalidOperationException>(() => mapper.Add(1, "3"));
    Assert.Equal(expectedExMessage, ex.Message);
  }

  [Fact]
  public void Ctor_Add_Duplicate_CustomErrorMessage()
  {
    var mapper = new Mapper<int, string>(
      new Dictionary<int, string>
      {
        [1] = "1",
        [2] = "2",
      });

    var ex = Assert.Throws<InvalidOperationException>(() => mapper.Add(1, "3", "*{0}*"));
    Assert.Equal("*1*", ex.Message);
  }
}
