using static Neomaster.JsonToLinq.JsonLinqConsts;

namespace Neomaster.JsonToLinq;

public class Mapper<TKey, TValue>
{
  private readonly Dictionary<TKey, TValue> _pairs = [];

  public Mapper()
  {
  }

  public Mapper(Mapper<TKey, TValue> source)
    : this(source._pairs)
  {
  }

  public Mapper(IDictionary<TKey, TValue> sourcePairs)
  {
    foreach (var p in sourcePairs)
    {
      _pairs.Add(p.Key, p.Value);
    }
  }

  public IReadOnlyDictionary<TKey, TValue> Pairs => _pairs;

  public TValue this[TKey key] => _pairs[key];

  public virtual Mapper<TKey, TValue> Add(
    TKey key,
    TValue value,
    string errorMessage = ErrorMessages.KeyRegistered)
  {
    if (_pairs.ContainsKey(key))
    {
      throw new InvalidOperationException(string.Format(errorMessage, key));
    }

    _pairs.Add(key, value);

    return this;
  }

  public virtual bool TryGet(TKey key, out TValue value)
  {
    return _pairs.TryGetValue(key, out value);
  }
}
