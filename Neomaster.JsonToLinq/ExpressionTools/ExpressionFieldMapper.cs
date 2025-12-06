using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq;

public class ExpressionFieldMapper
  : Mapper<string, ExpressionField>,
  IFluentAddPair<ExpressionFieldMapper, string, ExpressionField>,
  ICloneable<ExpressionFieldMapper>
{
  public ExpressionFieldMapper()
    : base()
  {
  }

  public ExpressionFieldMapper(ExpressionFieldMapper source)
    : base(source)
  {
  }

  public ExpressionFieldMapper(IDictionary<string, ExpressionField> sourcePairs)
    : base(sourcePairs)
  {
  }

  public static ExpressionFieldMapper OnDefault()
  {
    return new ExpressionFieldMapper();
  }

  public ExpressionFieldMapper Add(string key, ExpressionField value)
  {
    Add(key, value, ErrorMessages.SourceFieldRegistered);

    return this;
  }

  public ExpressionFieldMapper Clone()
  {
    var clone = new ExpressionFieldMapper();

    foreach (var p in Pairs)
    {
      clone.Add(p.Key, p.Value);
    }

    return clone;
  }
}
