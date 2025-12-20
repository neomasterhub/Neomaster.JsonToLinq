using System.Linq.Expressions;
using static Neomaster.JsonToLinq.JsonLinqConsts;

namespace Neomaster.JsonToLinq;

public class ExpressionOperatorMapper
  : Mapper<string, ExpressionBind>,
  IFluentAddPair<ExpressionOperatorMapper, string, ExpressionBind>,
  ICloneable<ExpressionOperatorMapper>
{
  public ExpressionOperatorMapper()
    : base()
  {
  }

  public ExpressionOperatorMapper(ExpressionOperatorMapper source)
    : base(source)
  {
  }

  public ExpressionOperatorMapper(IDictionary<string, ExpressionBind> sourcePairs)
    : base(sourcePairs)
  {
  }

  public static ExpressionOperatorMapper OnDefault()
  {
    return new ExpressionOperatorMapper(ExpressionOperatorMappers.Default);
  }

  public ExpressionOperatorMapper Add(string key, ExpressionBind value)
  {
    Add(key, value, ErrorMessages.OperatorRegistered);

    return this;
  }

  public ExpressionOperatorMapper Clone()
  {
    var clone = new ExpressionOperatorMapper();

    foreach (var p in Pairs)
    {
      clone.Add(p.Key, p.Value);
    }

    return clone;
  }

  public ExpressionOperatorMapper AddAlias(string alias, string key)
  {
    return Add(alias, this[key]);
  }

  public ExpressionOperatorMapper WithAliases(params string[] aliases)
  {
    var last = Pairs.Last();

    foreach (var alias in aliases)
    {
      Add(alias, last.Value);
    }

    return this;
  }

  public ExpressionOperatorMapper AddNot(string notOpKey, string key)
  {
    return Add(notOpKey, (left, right) => Expression.Not(this[key](left, right)));
  }

  public ExpressionOperatorMapper WithNot(string notOpKey = null)
  {
    var lastKey = Pairs.Last().Key;

    return AddNot(notOpKey ?? (lastKey.Contains(" ") ? "! " : "!"), lastKey);
  }
}
