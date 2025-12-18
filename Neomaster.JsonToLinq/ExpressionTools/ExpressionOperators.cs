using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Neomaster.JsonToLinq;

public static class ExpressionOperators
{
  private static readonly ConcurrentDictionary<Type, MethodInfo> _containsMethodsCache = [];

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
