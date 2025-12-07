using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Xunit.Abstractions;
using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq.UnitTests;

public class ExpressionHelperUnitTests(ITestOutputHelper output)
{
  [Fact]
  public void ParseExpressionLambda_SingleRule()
  {
    var tString = typeof(string);
    var obj = new PropertiesPublicGetSet();

    foreach (var prop in obj.GetType().GetProperties())
    {
      var (value, valueString) = GetPropertyValue(obj, prop);
      var expectedView = $"(Param_0.{prop.Name} == {valueString})";
      var jsonField = Options.Default.ConvertPropertyNameForJson(prop.Name);
      var jsonValue = JsonSerializer.Serialize(value);
      var conditionJson = CreateConditionJson("&&", "=", jsonField, jsonValue);

      var lambda = ParseExpressionLambda<PropertiesPublicGetSet>(conditionJson);

      Assert.Equal(expectedView, lambda.Body.ToString());
      Assert.True(lambda.Compile()(obj));

      output.WriteLine(expectedView);
    }
  }

  [Fact]
  public void ParseExpression_SingleRule()
  {
    var tString = typeof(string);
    var obj = new PropertiesPublicGetSet();

    foreach (var prop in obj.GetType().GetProperties())
    {
      var (value, valueString) = GetPropertyValue(obj, prop);
      var expectedView = $"(x.{prop.Name} == {valueString})";
      var jsonField = Options.Default.ConvertPropertyNameForJson(prop.Name);
      var jsonValue = JsonSerializer.Serialize(value);
      var conditionJson = CreateConditionJson("&&", "=", jsonField, jsonValue);

      var expr = ParseExpression<PropertiesPublicGetSet>(conditionJson);

      Assert.Equal(expectedView, expr.ToString());

      output.WriteLine(expectedView);
    }
  }

  [Theory]
  [InlineData(null, null, null)]
  [InlineData(null, true, null)]
  [InlineData(null, false, false)]
  [InlineData(true, null, null)]
  [InlineData(true, true, true)]
  [InlineData(true, false, false)]
  [InlineData(false, null, false)]
  [InlineData(false, true, false)]
  [InlineData(false, false, false)]
  public void CreateExpressionBind_Sql_AndAlso(bool? left, bool? right, bool? result)
  {
    CreateExpressionBindTest(
      ExpressionBindBuilders.Sql,
      "and",
      Expression.AndAlso,
      expr => Expression.Lambda<Func<bool?>>(expr),
      Expression.Constant(left, typeof(bool?)),
      Expression.Constant(right, typeof(bool?)),
      result);
  }

  [Theory]
  [InlineData(null, null, null)]
  [InlineData(null, true, true)]
  [InlineData(null, false, null)]
  [InlineData(true, null, true)]
  [InlineData(true, true, true)]
  [InlineData(true, false, true)]
  [InlineData(false, null, null)]
  [InlineData(false, true, true)]
  [InlineData(false, false, false)]
  public void CreateExpressionBind_Sql_OrElse(bool? left, bool? right, bool? result)
  {
    CreateExpressionBindTest(
      ExpressionBindBuilders.Sql,
      "or",
      Expression.OrElse,
      expr => Expression.Lambda<Func<bool?>>(expr),
      Expression.Constant(left, typeof(bool?)),
      Expression.Constant(right, typeof(bool?)),
      result);
  }

  [Theory]
  [InlineData(null, null, false)]
  [InlineData(null, true, false)]
  [InlineData(null, false, false)]
  [InlineData(true, null, false)]
  [InlineData(true, true, true)]
  [InlineData(true, false, false)]
  [InlineData(false, null, false)]
  [InlineData(false, true, false)]
  [InlineData(false, false, false)]
  public void CreateExpressionBind_NullAsFalse_AndAlso(bool? left, bool? right, bool? result)
  {
    CreateExpressionBindTest(
      ExpressionBindBuilders.NullAsFalse,
      "and",
      Expression.AndAlso,
      expr => Expression.Lambda<Func<bool>>(expr),
      Expression.Constant(left, typeof(bool?)),
      Expression.Constant(right, typeof(bool?)),
      result);
  }

  [Theory]
  [InlineData(null, null, false)]
  [InlineData(null, true, true)]
  [InlineData(null, false, false)]
  [InlineData(true, null, true)]
  [InlineData(true, true, true)]
  [InlineData(true, false, true)]
  [InlineData(false, null, false)]
  [InlineData(false, true, true)]
  [InlineData(false, false, false)]
  public void CreateExpressionBind_NullAsFalse_OrElse(bool? left, bool? right, bool? result)
  {
    CreateExpressionBindTest(
      ExpressionBindBuilders.NullAsFalse,
      "or",
      Expression.OrElse,
      expr => Expression.Lambda<Func<bool>>(expr),
      Expression.Constant(left, typeof(bool?)),
      Expression.Constant(right, typeof(bool?)),
      result);
  }

  [Theory]
  [InlineData(null, false)]
  [InlineData(false, false)]
  [InlineData(true, true)]
  public void CoalesceNullFalse(bool? input, bool output)
  {
    var inputExpression = Expression.Constant(input, typeof(bool?));

    var outputExpression = Expression.Constant(output, typeof(bool));
    var outputFunc = Expression.Lambda<Func<bool>>(outputExpression).Compile();

    Assert.Equal(output, outputFunc());
  }

  [Fact]
  public void EnumerateExpressionRules_ShouldEnumerateRootRules()
  {
    var rules = Enumerable
      .Range(1, 3)
      .Select(n => new
      {
        X = $"1.{n}",
        Rules = new[] { new { X = $"1.{n}.1" } },
      })
      .ToArray();
    var tree = new
    {
      X = "1",
      Rules = rules,
    };
    var treeJsonElement = JsonSerializer.SerializeToElement(tree);

    var enumeratedRules = ExpressionHelper.EnumerateExpressionRules(treeJsonElement, nameof(tree.Rules)).ToArray();

    Assert.Equal(rules.Length, enumeratedRules.Length);
    Assert.Equal(JsonSerializer.Serialize(rules), JsonSerializer.Serialize(enumeratedRules));
  }

  [Fact]
  public void EnumerateExpressionRules_ShouldIgnoreEmptyRules()
  {
    var tree = new
    {
      X = "1",
      Rules = new object[0],
    };
    var treeJsonElement = JsonSerializer.SerializeToElement(tree);

    var enumeratedRules = ExpressionHelper.EnumerateExpressionRules(treeJsonElement, nameof(tree.Rules)).ToArray();

    Assert.Empty(enumeratedRules);
  }

  [Fact]
  public void EnumerateExpressionRules_ShouldThrowKeyNotFoundException_MissingKey()
  {
    const string key = "Rules";
    var tree = new { X = "1" };
    var treeJson = JsonSerializer.Serialize(tree);
    var treeJsonElement = JsonSerializer.SerializeToElement(tree);
    var expectedExMessage = string.Format(ErrorMessages.JsonPropertyNotFound, key);

    var ex = Assert.Throws<KeyNotFoundException>(
      () => ExpressionHelper.EnumerateExpressionRules(treeJsonElement, key).ToArray());

    Assert.Equal(expectedExMessage, ex.Message);
    Assert.Equal(key, ex.Data[ErrorDataKeys.Property]);
    Assert.Equal(treeJson, ex.Data[ErrorDataKeys.Json]);
  }

  [Fact]
  public void EnumerateExpressionRules_ShouldThrowInvalidOperationException_NotArray()
  {
    var tree = new { X = "1", Rules = "2" };
    var treeJson = JsonSerializer.Serialize(tree);
    var treeJsonElement = JsonSerializer.SerializeToElement(tree);
    var key = nameof(tree.Rules);
    var expectedExMessage = string.Format(ErrorMessages.JsonPropertyNotType, key, JsonValueKind.Array);

    var ex = Assert.Throws<InvalidOperationException>(
      () => ExpressionHelper.EnumerateExpressionRules(treeJsonElement, key).ToArray());

    Assert.Equal(expectedExMessage, ex.Message);
    Assert.Equal(key, ex.Data[ErrorDataKeys.Property]);
    Assert.Equal(treeJson, ex.Data[ErrorDataKeys.Json]);
    Assert.Equal(JsonValueKind.Array, ex.Data[ErrorDataKeys.ExpectedType]);
    Assert.Equal(JsonValueKind.String, ex.Data[ErrorDataKeys.CurrentType]);
  }

  private static (object value, object valueString) GetPropertyValue<T>(T obj, PropertyInfo prop)
  {
    var value = prop.GetValue(obj);
    var valueString = value ?? "null";
    if (value != null && prop.PropertyType == typeof(string))
    {
      valueString = $"\"{valueString}\"";
    }

    return (value, valueString);
  }

  private static void CreateExpressionBindTest<TResult>(
    Func<ExpressionBind, Expression, Expression, Expression> buildBind,
    string logicOperator,
    ExpressionBind logicOperatorExpression,
    Func<Expression, LambdaExpression> buildLambda,
    Expression leftExpression,
    Expression rightExpression,
    TResult expectedLambdaResult)
  {
    var condition = new { logic = logicOperator };
    var logicOperatorPropertyName = nameof(condition.logic);
    var operatorMapper = new ExpressionOperatorMapper().Add(logicOperator, logicOperatorExpression);
    var conditionJsonElement = JsonSerializer.SerializeToElement(condition);
    var bind = ExpressionHelper.CreateExpressionBind(
      conditionJsonElement,
      logicOperatorPropertyName,
      operatorMapper,
      buildBind);

    var bound = bind(leftExpression, rightExpression);
    var lambda = buildLambda(bound).Compile();

    Assert.Equal(expectedLambdaResult, lambda.DynamicInvoke());
  }

  private static Expression<Func<TItem, bool>> ParseExpressionLambda<TItem>(string conditionJson)
  {
    var condition = JsonDocument.Parse(conditionJson);

    var fieldMapper = ExpressionFieldMapperFactory
      .CreateForPublicProperties<TItem>(
        Options.Default.ConvertPropertyNameForJson);

    var lambda = ExpressionHelper.ParseExpressionLambda<TItem>(
      condition,
      fieldMapper,
      Options.Default);

    return lambda;
  }

  private static Expression ParseExpression<TItem>(string conditionJson)
  {
    var condition = JsonDocument.Parse(conditionJson).RootElement;

    var fieldMapper = ExpressionFieldMapperFactory
      .CreateForPublicProperties<TItem>(
        Options.Default.ConvertPropertyNameForJson);

    var expr = ExpressionHelper.ParseExpression<TItem>(
      condition,
      Expression.Parameter(typeof(TItem), "x"),
      fieldMapper,
      Options.Default);

    return expr;
  }

  private static string CreateConditionJson(string logic, string op, string field, string value)
  {
    var condition =
      $$"""
      {
        "Logic": "{{logic}}",
        "Rules": [
          {
            "Operator": "{{op}}",
            "Field": "{{field}}",
            "Value": {{value}}
          }
        ]
      }
      """;

    return condition;
  }
}
