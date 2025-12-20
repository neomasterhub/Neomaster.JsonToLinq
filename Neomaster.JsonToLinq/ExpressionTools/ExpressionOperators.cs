using System.Collections.Concurrent;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Neomaster.JsonToLinq;

public static class ExpressionOperators
{
  private static readonly MethodInfo _toLower;
  private static readonly MethodInfo _toUpper;
  private static readonly MethodInfo _toLowerCi;
  private static readonly MethodInfo _toUpperCi;
  private static readonly MethodInfo _startsWith;
  private static readonly MethodInfo _endsWith;
  private static readonly MethodInfo _contains;
  private static readonly ConcurrentDictionary<Type, MethodInfo> _containsMethodsCache = [];

  static ExpressionOperators()
  {
    _toLower = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes);
    _toUpper = typeof(string).GetMethod(nameof(string.ToUpper), Type.EmptyTypes);
    _toLowerCi = typeof(string).GetMethod(nameof(string.ToLower), [typeof(CultureInfo)]);
    _toUpperCi = typeof(string).GetMethod(nameof(string.ToUpper), [typeof(CultureInfo)]);
    _startsWith = typeof(string).GetMethod(nameof(string.StartsWith), [typeof(string)]);
    _endsWith = typeof(string).GetMethod(nameof(string.EndsWith), [typeof(string)]);
    _contains = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)]);
  }

  public static Expression InStrings(
    Expression element,
    Expression collection,
    StringTransform stringTransform = StringTransform.None,
    CultureInfo cultureInfo = null)
  {
    return StringsContains(collection, element, stringTransform, cultureInfo);
  }

  public static Expression In(Expression element, Expression collection)
  {
    return Contains(collection, element);
  }

  public static Expression StartsWith(
    Expression element,
    Expression prefix,
    StringTransform stringTransform = StringTransform.None,
    CultureInfo cultureInfo = null)
  {
    return Expression.Call(
      TransformStringCase(element, stringTransform, cultureInfo),
      _startsWith,
      prefix);
  }

  public static Expression EndsWith(
    Expression element,
    Expression postfix,
    StringTransform stringTransform = StringTransform.None,
    CultureInfo cultureInfo = null)
  {
    return Expression.Call(
      TransformStringCase(element, stringTransform, cultureInfo),
      _endsWith,
      postfix);
  }

  public static Expression Contains(
    Expression element,
    Expression infix,
    StringTransform stringTransform = StringTransform.None,
    CultureInfo cultureInfo = null)
  {
    return Expression.Call(
      TransformStringCase(element, stringTransform, cultureInfo),
      _contains,
      infix);
  }

  public static Expression StringsContains(
    Expression collection,
    Expression element,
    StringTransform stringTransform = StringTransform.None,
    CultureInfo cultureInfo = null)
  {
    return Contains(
      collection,
      TransformStringCase(element, stringTransform, cultureInfo));
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

  private static Expression TransformStringCase(
    Expression text,
    StringTransform stringTransform = StringTransform.None,
    CultureInfo cultureInfo = null)
  {
    return stringTransform switch
    {
      StringTransform.None => text,

      StringTransform.Lower => cultureInfo == null
        ? Expression.Call(text, _toLower)
        : Expression.Call(text, _toLowerCi, Expression.Constant(cultureInfo, typeof(CultureInfo))),

      StringTransform.Upper => cultureInfo == null
        ? Expression.Call(text, _toUpper)
        : Expression.Call(text, _toUpperCi, Expression.Constant(cultureInfo, typeof(CultureInfo))),

      _ => throw new ArgumentOutOfRangeException(nameof(stringTransform)),
    };
  }
}
