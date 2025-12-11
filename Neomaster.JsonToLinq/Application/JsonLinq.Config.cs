#pragma warning disable SA1601
using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq;

public static partial class JsonLinq
{
  public static void Configure(Action<ExpressionParsingOptions> options)
  {
    options(Options.Default);
  }
}
