using System.Text.Json;
using Xunit.Abstractions;

namespace Neomaster.JsonToLinq.UnitTests;

public class ExpressionFieldMapperFactoryUnitTests(ITestOutputHelper output)
  : UnitTestsBase
{
  [Fact]
  public void CreateForPublicProperties_Mapping()
  {
    var props = typeof(PropertiesPublicGetSet)
      .GetProperties()
      .ToArray();

    var mapper = ExpressionFieldMapperFactory
      .CreateForPublicProperties<PropertiesPublicGetSet>();

    Assert.Equal(props.Length, mapper.Pairs.Count);
    Assert.All(mapper.Pairs, (f, i) =>
    {
      var expectedName = props[i].Name;
      Assert.Equal(expectedName, f.Key);
      Assert.Equal(expectedName, f.Value.Name);
      output.WriteLine(f.Key);
    });
  }

  [Fact]
  public void CreateForPublicProperties_GettingValue_Element()
  {
    var obj = new PropertiesPublicGetSet();
    var propValues = typeof(PropertiesPublicGetSet)
      .GetProperties()
      .Select(p => p.GetValue(obj))
      .ToArray();
    var jsonElements = propValues
      .Select(v => JsonSerializer.SerializeToElement(v))
      .ToArray();

    var mapperValues = ExpressionFieldMapperFactory
      .CreateForPublicProperties<PropertiesPublicGetSet>().Pairs.Values
      .Select((v, i) => v.GetValue(jsonElements[i]).Value)
      .ToArray();

    Assert.Equal(propValues.Length, mapperValues.Length);
    Assert.All(mapperValues, (actual, i) =>
    {
      var expected = propValues[i];
      Assert.Equal(expected, actual);
      output.WriteLine(actual?.ToString() ?? "Null");
    });
  }

  [Fact]
  public void CreateForPublicProperties_GettingValue_Array()
  {
    var obj = new PropertiesPublicGetSet();
    var propValues = typeof(PropertiesPublicGetSet)
      .GetProperties()
      .Select(p => p.GetValue(obj))
      .ToArray();
    var jsonElements = propValues
      .Select(v => JsonSerializer.SerializeToElement(new[] { v }))
      .ToArray();

    var mapperValues = ExpressionFieldMapperFactory
      .CreateForPublicProperties<PropertiesPublicGetSet>().Pairs.Values
      .Select((v, i) => v.GetValue(jsonElements[i]).Value)
      .ToArray();

    Assert.Equal(propValues.Length, mapperValues.Length);
    Assert.All(mapperValues, (actual, i) =>
    {
      var propValue = propValues[i];
      var expected = new[] { propValue };
      Assert.Equal(expected, actual);
      output.WriteLine($"[{propValue ?? "Null"}]");
    });
  }
}
