namespace Neomaster.JsonToLinq;

public class ExpressionFieldMapper
{
  private readonly Dictionary<string, ExpressionField> _fields = [];

  public IReadOnlyDictionary<string, ExpressionField> Fields => _fields;

  public ExpressionField this[string op] => _fields[op];

  public ExpressionFieldMapper Add(string srcFieldName, ExpressionField dstField)
  {
    _fields.Add(srcFieldName, dstField);

    return this;
  }
}
