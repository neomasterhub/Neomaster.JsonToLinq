using System.Collections.Concurrent;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Neomaster.JsonToLinq;

public static class ExpressionOperators
{
  private static readonly ConcurrentDictionary<Type, MethodInfo> _containsMethodsCache = [];
  private static readonly MethodInfo _toLower;
  private static readonly MethodInfo _toUpper;

  static ExpressionOperators()
  {
    _toLower = typeof(string).GetMethod(nameof(string.ToLower), [typeof(CultureInfo)]);
    _toUpper = typeof(string).GetMethod(nameof(string.ToUpper), [typeof(CultureInfo)]);
  }

  public static Expression In(Expression element, Expression collection)
  {
    return Contains(collection, element);
  }

  public static Expression ContainsString(
    Expression collection,
    Expression element,
    StringTransform stringTransform = StringTransform.None,
    CultureInfo cultureInfo = null)
  {
    var ci = Expression.Constant(cultureInfo ?? CultureInfo.InvariantCulture);

    element = stringTransform switch
    {
      StringTransform.None => element,
      StringTransform.Lower => Expression.Call(_toLower, element, ci),
      StringTransform.Upper => Expression.Call(_toUpper, element, ci),
      _ => throw new ArgumentOutOfRangeException(nameof(stringTransform)),
    };

    return Contains(collection, element);
  }

  public static Expression Contains(Expression collection, Expression element)
  {
    var collectionElementType = collection.Type.IsArray
      ? collection.Type.GetElementType()
      : collection.Type.GetGenericArguments()[0];

    if (element.Type != collectionElementType)
    {
      element = Expression.Convert(element, collectionElementType);
    }

    return Expression.Call(GetContainsMethod(collectionElementType), collection, element);
  }

  private static MethodInfo GetContainsMethod(Type type)
  {
    return _containsMethodsCache.GetOrAdd(type, t => typeof(Enumerable)
      .GetMethods()
      .Single(m =>
        m.Name == nameof(Enumerable.Contains)
        && m.GetParameters().Length == 2)
      .MakeGenericMethod(t));
  }
}
