using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq;

public class ExpressionOperatorMapper
  : Mapper<string, ExpressionBind>,
  IFluentOnDefault<ExpressionOperatorMapper>,
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

  public ExpressionOperatorMapper Add(string key, ExpressionBind value)
  {
    Add(key, value, ErrorMessages.OperatorRegistered);

    return this;
  }

  public ExpressionOperatorMapper OnDefault()
  {
    return new ExpressionOperatorMapper(ExpressionOperatorMappers.Default);
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
