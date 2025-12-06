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
}
