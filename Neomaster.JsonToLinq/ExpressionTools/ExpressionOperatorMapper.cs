using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq;

public class ExpressionOperatorMapper : ICloneable<ExpressionOperatorMapper>
{
  private readonly Dictionary<string, ExpressionBind> _operators = [];

  public ExpressionOperatorMapper()
  {
  }

  public ExpressionOperatorMapper(ExpressionOperatorMapper source)
    : this(source._operators)
  {
  }

  public ExpressionOperatorMapper(IDictionary<string, ExpressionBind> sourceOperators)
  {
    foreach (var op in sourceOperators)
    {
      _operators.Add(op.Key, op.Value);
    }
  }

  public ExpressionBind this[string op] => _operators[op];

  public static ExpressionOperatorMapper OnDefault()
  {
    return new ExpressionOperatorMapper(ExpressionOperatorMappers.Default);
  }

  public ExpressionOperatorMapper Add(string op, ExpressionBind bind)
  {
    if (_operators.ContainsKey(op))
    {
      throw new InvalidOperationException(string.Format(ErrorMessages.OperatorRegistered, op));
    }

    _operators.Add(op, bind);

    return this;
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
