## 🛠️ Operators

### 📌 Default Operators

```csharp
public IReadOnlyDictionary<TKey, TValue> ExpressionOperatorMapper.Pairs { get; }
```

| JSON   | LINQ Expression                 | Description           |
|--------|---------------------------------|-----------------------|
| `&`    | `Expression.And`                | Bitwise AND           |
| `&&`   | `Expression.AndAlso`            | Logical AND           |
| `\|`   | `Expression.Or`                 | Bitwise OR            |
| `\|\|` | `Expression.OrElse`             | Logical OR            |
| `=`    | `Expression.Equal`              | Equal                 |
| `!=`   | `Expression.NotEqual`           | Not equal             |
| `>`    | `Expression.GreaterThan`        | Greater than          |
| `>=`   | `Expression.GreaterThanOrEqual` | Greater than or equal |
| `<`    | `Expression.LessThan`           | Less than             |
| `<=`   | `Expression.LessThanOrEqual`    | Less than or equal    |

### 🌟 Add Custom Operators

```csharp
JsonLinq.Configure(options =>
  {
    options.OperatorMapper
      .Add("lt", Expression.LessThan)
      .Add("gt", Expression.GreaterThan);
  });
```
