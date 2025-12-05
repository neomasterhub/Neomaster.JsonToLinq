using System.Linq.Expressions;
using System.Text.Json;
using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq;

public static class ExpressionHelper
{
  public static Expression<Func<T, bool>> ParseExpressionLambda<T>(
    JsonDocument doc,
    ExpressionFieldMapper fieldMapper,
    ExpressionParsingOptions parsingOptions)
  {
    var par = Expression.Parameter(typeof(T));
    var condition = ParseExpression<T>(
      doc.RootElement,
      par,
      fieldMapper,
      parsingOptions);

    if (condition.CanReduce)
    {
      condition = condition.ReduceAndCheck();
    }

    var exp = Expression.Lambda<Func<T, bool>>(condition, par);

    return exp;
  }

  public static Expression ParseExpression<T>(
    JsonElement condition,
    ParameterExpression par,
    ExpressionFieldMapper fieldMapper,
    ExpressionParsingOptions parsingOptions)
  {
    var bind = CreateExpressionBind(
      condition,
      parsingOptions.LogicOperatorPropertyName,
      parsingOptions.OperatorMapper,
      parsingOptions.BindBuilder);

    var rules = EnumerateExpressionRules(
      condition,
      parsingOptions.RulesPropertyName);

    Expression left = null;

    foreach (var r in rules)
    {
      Expression right = null;

      if (r.TryGetProperty(parsingOptions.LogicOperatorPropertyName, out _))
      {
        right = ParseExpression<T>(
          r,
          par,
          fieldMapper,
          parsingOptions);

        left = bind(left, right);

        continue;
      }

      var rule = ExpressionRule.Parse(
        r,
        fieldMapper,
        parsingOptions.OperatorPropertyName,
        parsingOptions.FieldPropertyName,
        parsingOptions.ValuePropertyName);

      right = rule.CreateFilterExpression(par, parsingOptions.OperatorMapper);
      left = bind(left, right);
    }

    return left;
  }

  public static Func<Expression, Expression, Expression> CreateExpressionBind(
    JsonElement condition,
    string logicOperatorPropertyName,
    ExpressionOperatorMapper operatorMapper,
    Func<ExpressionBind, Expression, Expression, Expression> bindBuilder)
  {
    var logicOperator = condition.GetProperty(logicOperatorPropertyName).GetString();
    var logicBind = operatorMapper[logicOperator];

    return (left, right) => bindBuilder(logicBind, left, right);
  }

  public static Expression CoalesceNullFalse(Expression expr)
  {
    return expr.Type == typeof(bool?)
      ? Expression.Coalesce(expr, Expression.Constant(false, typeof(bool)))
      : expr;
  }

  public static IEnumerable<JsonElement> EnumerateExpressionRules(
    JsonElement condition,
    string rulesPropertyName)
  {
    if (!condition.TryGetProperty(rulesPropertyName, out var rulesJsonElement))
    {
      var exMessage = string.Format(ErrorMessages.JsonPropertyNotFound, rulesPropertyName);
      var ex = new KeyNotFoundException(exMessage);
      ex.Data[ErrorDataKeys.Json] = condition.GetRawText();
      ex.Data[ErrorDataKeys.Property] = rulesPropertyName;

      throw ex;
    }

    if (rulesJsonElement.ValueKind != JsonValueKind.Array)
    {
      var exMessage = string.Format(ErrorMessages.JsonPropertyNotType, rulesPropertyName, JsonValueKind.Array);
      var ex = new InvalidOperationException(exMessage);
      ex.Data[ErrorDataKeys.Json] = condition.GetRawText();
      ex.Data[ErrorDataKeys.Property] = rulesPropertyName;
      ex.Data[ErrorDataKeys.ExpectedType] = JsonValueKind.Array;
      ex.Data[ErrorDataKeys.CurrentType] = rulesJsonElement.ValueKind;

      throw ex;
    }

    var rules = rulesJsonElement.EnumerateArray();

    foreach (var r in rules)
    {
      yield return r;
    }
  }
}
