using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq;

public class ExpressionOperatorMapper
{
  private readonly Dictionary<string, ExpressionBind> _operators = [];

  public ExpressionBind this[string op] => _operators[op];

  public ExpressionOperatorMapper Add(string op, ExpressionBind bind)
  {
    if (_operators.ContainsKey(op))
    {
      throw new InvalidOperationException(string.Format(ErrorMessages.OperatorRegistered, op));
    }

    _operators.Add(op, bind);

    return this;
  }
}
