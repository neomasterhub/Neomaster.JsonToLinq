using System.Linq.Expressions;
using System.Text.Json;

namespace Neomaster.JsonToLinq;

/// <summary>
/// Provides methods to convert JSON-based query definitions into LINQ expressions.
/// </summary>
public static partial class JsonLinq
{
  /// <summary>
  /// Returns the first element in a sequence that satisfies a specified JSON filter.
  /// </summary>
  /// <typeparam name="TElement">The type of the elements of source.</typeparam>
  /// <param name="elements">An System.Collections.Generic.IEnumerable`1 to return an element from.</param>
  /// <param name="filter">JSON filter definition.</param>
  /// <param name="fieldMapper">Maps JSON field names to <see cref="ExpressionField"/> definitions.</param>
  /// <param name="options">Optional parser settings. Uses defaults if null.</param>
  /// <returns>The first element in the sequence that matches the JSON filter.</returns>
  /// <exception cref="ArgumentNullException">
  /// The source sequence or the JSON filter is null.
  /// </exception>
  /// <exception cref="InvalidOperationException">
  /// No element satisfies the JSON filter,
  /// or the source sequence is empty.
  /// </exception>
  public static TElement First<TElement>(
    this IEnumerable<TElement> elements,
    JsonDocument filter,
    ExpressionFieldMapper fieldMapper = null,
    ExpressionParsingOptions options = null)
  {
    if (filter == null)
    {
      throw new ArgumentNullException(nameof(filter));
    }

    return elements.First(ParseToFilterExpression<TElement>(filter, fieldMapper, options).Compile());
  }

  /// <summary>
  /// Filters a sequence of values based on a JSON filter definition.
  /// </summary>
  /// <typeparam name="TElement">Type of elements to filter.</typeparam>
  /// <param name="elements">An System.Collections.Generic.IEnumerable`1 to filter.</param>
  /// <param name="filterJson">JSON filter definition.</param>
  /// <param name="fieldMapper">Maps JSON field names to <see cref="ExpressionField"/> definitions.</param>
  /// <param name="options">Optional parser settings. Uses defaults if null.</param>
  /// <returns>
  /// An System.Collections.Generic.IEnumerable`1
  /// that contains elements from the input sequence that satisfy the condition.
  /// </returns>
  /// <exception cref="ArgumentNullException">
  /// The source sequence or the JSON filter is null.
  /// </exception>
  public static IEnumerable<TElement> Where<TElement>(
    this IEnumerable<TElement> elements,
    string filterJson,
    ExpressionFieldMapper fieldMapper = null,
    ExpressionParsingOptions options = null)
  {
    if (string.IsNullOrWhiteSpace(filterJson))
    {
      throw new ArgumentNullException(nameof(filterJson));
    }

    return elements.Where(JsonDocument.Parse(filterJson), fieldMapper, options);
  }

  /// <summary>
  /// Filters a sequence of values based on a JSON filter definition.
  /// </summary>
  /// <typeparam name="TElement">Type of elements to filter.</typeparam>
  /// <param name="elements">An System.Collections.Generic.IEnumerable`1 to filter.</param>
  /// <param name="filter">JSON filter definition.</param>
  /// <param name="fieldMapper">Maps JSON field names to <see cref="ExpressionField"/> definitions.</param>
  /// <param name="options">Optional parser settings. Uses defaults if null.</param>
  /// <returns>
  /// An System.Collections.Generic.IEnumerable`1
  /// that contains elements from the input sequence that satisfy the condition.
  /// </returns>
  /// <exception cref="ArgumentNullException">
  /// The source sequence or the JSON filter is null.
  /// </exception>
  public static IEnumerable<TElement> Where<TElement>(
    this IEnumerable<TElement> elements,
    JsonDocument filter,
    ExpressionFieldMapper fieldMapper = null,
    ExpressionParsingOptions options = null)
  {
    if (filter == null)
    {
      throw new ArgumentNullException(nameof(filter));
    }

    return elements.Where(ParseToFilterExpression<TElement>(filter, fieldMapper, options).Compile());
  }

  /// <summary>
  /// Parses a JSON document into a LINQ expression filter for <typeparamref name="T"/>.
  /// </summary>
  /// <typeparam name="T">Type of objects to filter.</typeparam>
  /// <param name="filter">JSON filter definition.</param>
  /// <param name="fieldMapper">Maps JSON field names to <see cref="ExpressionField"/> definitions.</param>
  /// <param name="options">Optional parser settings. Uses defaults if null.</param>
  /// <returns>An <see cref="Expression{Func}"/> representing the filter for <typeparamref name="T"/>.</returns>
  public static Expression<Func<T, bool>> ParseToFilterExpression<T>(
    JsonDocument filter,
    ExpressionFieldMapper fieldMapper = null,
    ExpressionParsingOptions options = null)
  {
    options ??= Consts.Options.Default;
    fieldMapper ??= ExpressionFieldMapper.OnDefault<T>();

    return ExpressionHelper.ParseExpressionLambda<T>(
      filter,
      fieldMapper,
      options);
  }
}
