#pragma warning disable SA1601
using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq;

public static partial class JsonLinq
{
  public static void RestoreDefaultOptions()
  {
    Options.Default = null;
  }

  public static void Configure(Action<ExpressionParsingOptions> setOptions)
  {
    Options.Default = new();
    setOptions(Options.Default);
  }
}
