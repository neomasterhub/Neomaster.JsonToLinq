#pragma warning disable SA1601
using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq;

public static partial class JsonLinq
{
  /// <summary>
  /// Restores the default options.
  /// </summary>
  public static void RestoreDefaultOptions()
  {
    Options.Default = null;
  }

  /// <summary>
  /// Applies custom options as the default.
  /// </summary>
  /// <param name="setOptions">Sets custom options as the default.</param>
  public static void Configure(Action<ExpressionParsingOptions> setOptions)
  {
    Options.Default = new();
    setOptions(Options.Default);
  }
}
