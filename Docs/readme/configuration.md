## 🎛 Configuration

You can define your own settings via `JsonLinq.Configure()`.
To reset the configuration - necessary for testing - call `JsonLinq.ResetConfiguration()`.

```csharp
JsonLinq.Configure(options =>
{
  // Property names in JSON filters
  options.LogicOperatorPropertyName = "🔗";
  options.RulesPropertyName = "⚖️";
  options.OperatorPropertyName = "⚡";
  options.FieldPropertyName = "🍁";
  options.ValuePropertyName = "🍬";

  // Operator definitions and synonyms
  options.OperatorMapper = ExpressionOperatorMapper.OnDefault()
    .AddAlias("does not contain", "!contains");

  // Handling logic for expressions with null
  options.BindBuilder = ExpressionBindBuilders.NullAsFalse;
  
  // How C# property names appear in JSON
  options.ConvertPropertyNameForJson = JsonNamingPolicy.SnakeCaseUpper.ConvertName;
});
```

The JSON filter structure settings seem interesting.
<br>For example, adding syntactic sugar:
<br>`"Logic": "&&", "Rules": [...]` -> `"&&": [...]`
<br>Or fixing the property order to support [TONL][tonl] filters.

This may be implemented in future versions.

[tonl]: https://tonl.dev
