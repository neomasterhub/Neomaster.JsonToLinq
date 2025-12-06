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
}
