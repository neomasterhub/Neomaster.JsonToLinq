using System.Linq.Expressions;
using System.Text.Json;
using static Neomaster.JsonToLinq.Consts;

namespace Neomaster.JsonToLinq.UnitTests;

public class JsonLinqConfigUnitTests
  : UnitTestsBase
{
  public JsonLinqConfigUnitTests()
  {
    JsonLinq.RestoreDefaultOptions();
  }

  [Fact]
  public void RestoreDefaultOptions()
  {
    var prevDefaultOptions = Options.Default;
    JsonLinq.Configure(_ => { });

    JsonLinq.RestoreDefaultOptions();

    Assert.StrictEqual(prevDefaultOptions, Options.Default);
  }

  [Fact]
  public void Configure_AllDifferent()
  {
    var prevDefaultOptions = Options.Default;

    JsonLinq.Configure(options =>
    {
      options.LogicOperatorPropertyName = "L";
      options.RulesPropertyName = "R";
      options.OperatorPropertyName = "O";
      options.FieldPropertyName = "F";
      options.ValuePropertyName = "V";
      options.OperatorMapper = new ExpressionOperatorMapper()
        .Add("OR", Expression.OrElse)
        .Add("EQ", Expression.Equal)
        .Add("LT", Expression.LessThan);
      options.BindBuilder = ExpressionBindBuilders.NullAsFalse;
      options.ConvertPropertyNameForJson = JsonNamingPolicy.SnakeCaseUpper.ConvertName;
    });

    var json =
      """
      {
        "L": "OR",
        "R": [
          {
            "F": "LAST_VISIT_AT",
            "O": "EQ",
            "V": null
          },
          {
            "F": "LAST_VISIT_AT",
            "O": "LT",
            "V": "2025-01-01T00:00:00Z"
          }
        ]
      }
      """;

    var ex = Record.Exception(() => JsonLinq.ParseToFilterExpression<User>(JsonDocument.Parse(json)));

    Assert.NotEqual(prevDefaultOptions, Options.Default);
    Assert.Null(ex);

    var props = typeof(ExpressionParsingOptions).GetProperties();
    foreach (var prop in props)
    {
      Assert.NotEqual(prop.GetValue(prevDefaultOptions), prop.GetValue(Options.Default));
    }
  }
}
