using static Neomaster.JsonToLinq.JsonLinqConsts;

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

  public static ExpressionFieldMapper OnDefault<T>()
  {
    return ExpressionFieldMapperFactory.CreateForPublicProperties<T>(
      Options.Default.ConvertPropertyNameForJson);
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
