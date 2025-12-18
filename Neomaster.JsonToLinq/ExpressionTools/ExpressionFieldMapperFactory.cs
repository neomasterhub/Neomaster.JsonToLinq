using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace Neomaster.JsonToLinq;

public class ExpressionFieldMapperFactory
{
  public static ExpressionFieldMapper CreateForPublicProperties<T>(
    Func<string, string> convertPropertyNameForJson = null)
  {
    convertPropertyNameForJson ??= _ => _;
    var mapper = new ExpressionFieldMapper();
    var type = typeof(T);
    var props = type
      .GetProperties(BindingFlags.Public | BindingFlags.Instance)
      .Where(p => p.CanRead && p.CanWrite);

    foreach (var prop in props)
    {
      mapper.Add(convertPropertyNameForJson(prop.Name), new ExpressionField
      {
        Name = prop.Name,
        GetValue = je => GetJsonElementValue(je, prop.PropertyType),
      });
    }

    return mapper;
  }

  private static ConstantExpression GetJsonElementValue(JsonElement? je, Type propType)
  {
    if (je?.ValueKind == JsonValueKind.Array)
    {
      propType = propType.MakeArrayType();
    }

    return Expression.Constant(je?.Deserialize(propType), propType);
  }
}
