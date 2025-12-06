namespace Neomaster.JsonToLinq;

public interface IFluentAddPair<TPairs, TKey, TValue>
{
  TPairs Add(TKey key, TValue value);
}
