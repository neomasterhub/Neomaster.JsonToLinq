using System.Collections.Concurrent;
using System.Reflection;

namespace Neomaster.JsonToLinq;

public static class ExpressionOperators
{
  private static readonly ConcurrentDictionary<Type, MethodInfo> _containsMethodsCache = [];

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
