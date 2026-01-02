## 🎛 Configuration<a name="configuration"/>

You can define your own settings via `JsonLinq.Configure()`.
To reset the configuration, call `JsonLinq.ResetConfiguration()`.
This is necessary for testing.

```csharp
JsonLinq.Configure(options =>
{
  // Property names in JSON filters
  options.LogicOperatorPropertyName = "🔗";
  options.RulesPropertyName = "⚖️";
  options.OperatorPropertyName = "⚡";
  options.FieldPropertyName = "🍁";
  options.ValuePropertyName = "🍬";

  // Operator definitions and aliases
  options.OperatorMapper = ExpressionOperatorMapper.OnDefault()
    .AddAlias("does not contain", "!contains");

  // Handling logic for expressions with null
  options.BindBuilder = ExpressionBindBuilders.NullAsFalse;
  
  // How C# property names appear in JSON
  options.ConvertPropertyNameForJson = JsonNamingPolicy.SnakeCaseUpper.ConvertName;
});
```

The JSON filter structure settings are relevant.
<br>For example, adding syntactic sugar:
<br>`"Logic": "&&", "Rules": [...]` → `"&&": [...]`
<br>Or specifying the property order to support [TONL][tonl] filters.
<br>This may be implemented in future versions.

[tonl]: https://tonl.dev
