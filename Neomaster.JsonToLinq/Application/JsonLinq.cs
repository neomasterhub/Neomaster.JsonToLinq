using System.Linq.Expressions;
using System.Text.Json;

namespace Neomaster.JsonToLinq;

/// <summary>
/// Provides methods to convert JSON-based query definitions into LINQ expressions.
/// </summary>
public static partial class JsonLinq
{
  /// <summary>
  /// Filters a sequence of values based on a JSON filter definition.
  /// </summary>
  /// <typeparam name="TElement">Type of elements to filter.</typeparam>
  /// <param name="elements">An System.Collections.Generic.IEnumerable`1 to filter.</param>
  /// <param name="json">JSON filter definition.</param>
  /// <param name="fieldMapper">Maps JSON field names to <see cref="ExpressionField"/> definitions.</param>
  /// <param name="options">Optional parser settings. Uses defaults if null.</param>
  /// <returns>
  /// An System.Collections.Generic.IEnumerable`1
  /// that contains elements from the input sequence that satisfy the condition.
  /// </returns>
  public static IEnumerable<TElement> Where<TElement>(
    this IEnumerable<TElement> elements,
    string json,
    ExpressionFieldMapper fieldMapper = null,
    ExpressionParsingOptions options = null)
  {
    return elements.Where(JsonDocument.Parse(json), fieldMapper, options);
  }

  /// <summary>
  /// Filters a sequence of values based on a JSON filter definition.
  /// </summary>
  /// <typeparam name="TElement">Type of elements to filter.</typeparam>
  /// <param name="elements">An System.Collections.Generic.IEnumerable`1 to filter.</param>
  /// <param name="doc">JSON filter definition.</param>
  /// <param name="fieldMapper">Maps JSON field names to <see cref="ExpressionField"/> definitions.</param>
  /// <param name="options">Optional parser settings. Uses defaults if null.</param>
  /// <returns>
  /// An System.Collections.Generic.IEnumerable`1
  /// that contains elements from the input sequence that satisfy the condition.
  /// </returns>
  public static IEnumerable<TElement> Where<TElement>(
    this IEnumerable<TElement> elements,
    JsonDocument doc,
    ExpressionFieldMapper fieldMapper = null,
    ExpressionParsingOptions options = null)
  {
    return elements.Where(ParseToFilterExpression<TElement>(doc, fieldMapper, options).Compile());
  }

  /// <summary>
  /// Parses a JSON document into a LINQ expression filter for <typeparamref name="T"/>.
  /// </summary>
  /// <typeparam name="T">Type of objects to filter.</typeparam>
  /// <param name="doc">JSON filter definition.</param>
  /// <param name="fieldMapper">Maps JSON field names to <see cref="ExpressionField"/> definitions.</param>
  /// <param name="options">Optional parser settings. Uses defaults if null.</param>
  /// <returns>An <see cref="Expression{Func}"/> representing the filter for <typeparamref name="T"/>.</returns>
  public static Expression<Func<T, bool>> ParseToFilterExpression<T>(
    JsonDocument doc,
    ExpressionFieldMapper fieldMapper = null,
    ExpressionParsingOptions options = null)
  {
    options ??= Consts.Options.Default;
    fieldMapper ??= ExpressionFieldMapper.OnDefault<T>();

    return ExpressionHelper.ParseExpressionLambda<T>(
      doc,
      fieldMapper,
      options);
  }
}
