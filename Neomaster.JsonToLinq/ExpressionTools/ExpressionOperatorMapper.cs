using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq;

public class ExpressionOperatorMapper : Mapper<string, ExpressionBind>, ICloneable<ExpressionOperatorMapper>
{
  static ExpressionOperatorMapper()
  {
    Default = ExpressionOperatorMappers.Default;
  }

  public override Mapper<string, ExpressionBind> Add(
    string key,
    ExpressionBind value,
    string errorMessage = ErrorMessages.OperatorRegistered)
  {
    return base.Add(key, value, errorMessage);
  }

  public ExpressionOperatorMapper Clone()
  {
    var clone = new ExpressionOperatorMapper();

    foreach (var op in _operators)
    {
      clone.Add(op.Key, op.Value);
    }

    return clone;
  }
}
