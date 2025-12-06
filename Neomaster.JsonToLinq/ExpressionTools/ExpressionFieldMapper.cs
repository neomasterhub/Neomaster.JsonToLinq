namespace Neomaster.JsonToLinq;

public class ExpressionFieldMapper
{
  public readonly Dictionary<string, ExpressionField> _fields = [];

  public IReadOnlyDictionary<string, ExpressionField> Fields => _fields;

  public ExpressionFieldMapper Add(string srcFieldName, ExpressionField dstField)
  {
    _fields.Add(srcFieldName, dstField);

    return this;
  }
}
