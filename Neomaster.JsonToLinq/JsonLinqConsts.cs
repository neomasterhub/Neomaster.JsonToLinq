using System.Linq.Expressions;

namespace Neomaster.JsonToLinq;

public static class JsonLinqConsts
{
  public delegate Expression ExpressionBind(Expression left, Expression right);

  public static class Options
  {
    private static readonly ExpressionParsingOptions _default = new();
    private static ExpressionParsingOptions _custom;
    public static ExpressionParsingOptions Default
    {
      get => _custom ?? _default;
      set => _custom = value;
    }
  }

  public static class ExpressionOperatorMappers
  {
    public static readonly ExpressionOperatorMapper Default = new ExpressionOperatorMapper()
      .Add("&", Expression.And)
      .Add("&&", Expression.AndAlso)
      .Add("|", Expression.Or)
      .Add("||", Expression.OrElse)
      .Add("=", Expression.Equal)
      .Add("!=", Expression.NotEqual)
      .Add(">", Expression.GreaterThan)
      .Add(">=", Expression.GreaterThanOrEqual)
      .Add("<", Expression.LessThan)
      .Add("<=", Expression.LessThanOrEqual)
      .Add("in", ExpressionOperators.In).WithNot()
      .Add("as lower in", (element, collection) => ExpressionOperators.InStrings(element, collection, StringTransform.Lower)).WithNot()
      .Add("as upper in", (element, collection) => ExpressionOperators.InStrings(element, collection, StringTransform.Upper)).WithNot()
      .Add("starts with", (element, prefix) => ExpressionOperators.StartsWith(element, prefix)).WithNot()
      .Add("as lower starts with", (element, prefix) => ExpressionOperators.StartsWith(element, prefix, StringTransform.Lower)).WithNot()
      .Add("as upper starts with", (element, prefix) => ExpressionOperators.StartsWith(element, prefix, StringTransform.Upper)).WithNot()
      .Add("ends with", (element, postfix) => ExpressionOperators.EndsWith(element, postfix)).WithNot()
      .Add("as lower ends with", (element, postfix) => ExpressionOperators.EndsWith(element, postfix, StringTransform.Lower)).WithNot()
      .Add("as upper ends with", (element, postfix) => ExpressionOperators.EndsWith(element, postfix, StringTransform.Upper)).WithNot()
      .Add("contains", (element, infix) => ExpressionOperators.Contains(element, infix, StringTransform.None)).WithNot()
      .Add("as lower contains", (element, infix) => ExpressionOperators.Contains(element, infix, StringTransform.Lower)).WithNot()
      .Add("as upper contains", (element, infix) => ExpressionOperators.Contains(element, infix, StringTransform.Upper)).WithNot();
  }

  public static class ExpressionBindBuilders
  {
    public static readonly Func<ExpressionBind, Expression, Expression, Expression> Sql = (bind, left, right) =>
      left == null
      ? right
      : bind(left, right);

    public static readonly Func<ExpressionBind, Expression, Expression, Expression> NullAsFalse = (bind, left, right) =>
      left == null
      ? ExpressionHelper.CoalesceNullFalse(right)
      : bind(ExpressionHelper.CoalesceNullFalse(left), ExpressionHelper.CoalesceNullFalse(right));
  }

  public static class ErrorMessages
  {
    public const string JsonPropertyNotFound = "Json property \"{0}\" not found.";
    public const string JsonPropertyNotType = "Json property \"{0}\" must be of type \"{1}\".";
    public const string JsonPropertyEmpty = "Json property \"{0}\" is empty.";
    public const string KeyRegistered = "Key \"{0}\" is already registered.";
    public const string OperatorRegistered = "Operator \"{0}\" is already registered.";
    public const string SourceFieldRegistered = "Source field \"{0}\" is already registered.";
  }

  public static class ErrorDataKeys
  {
    public const string Json = nameof(Json);
    public const string Property = nameof(Property);
    public const string ExpectedType = nameof(ExpectedType);
    public const string CurrentType = nameof(CurrentType);
  }

  public static class JsonLinqOptionsPropertyNames
  {
    public const string LogicOperator = "Logic";
    public const string Rules = nameof(Rules);
    public const string Operator = nameof(ExpressionRule.Operator);
    public const string Field = nameof(ExpressionRule.Field);
    public const string Value = nameof(ExpressionRule.Value);
  }
}
