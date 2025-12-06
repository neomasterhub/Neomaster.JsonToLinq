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
}
