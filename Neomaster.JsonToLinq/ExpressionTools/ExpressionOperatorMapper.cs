using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq;

public class ExpressionOperatorMapper
  : Mapper<string, ExpressionBind>,
  IFluentAddPair<ExpressionOperatorMapper, string, ExpressionBind>,
  ICloneable<ExpressionOperatorMapper>
{
  static ExpressionOperatorMapper()
  {
    Default = ExpressionOperatorMappers.Default;
  }

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
}
