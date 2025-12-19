## 🛠️ Operators

👓 Operators and their handling logic are encapsulated in class `ExpressionOperatorMapper`.
The default operator mapping can be accessed via property `Pairs`.

🔠 String comparisons are case-sensitive, depending on the database collation, not on the operators.

### 📌 Default Operators

| JSON          | LINQ Expression                 | Description                       |
|---------------|---------------------------------|-----------------------------------|
| `&`           | `Expression.And`                | Bitwise AND                       |
| `&&`          | `Expression.AndAlso`            | Logical AND                       |
| `\|`          | `Expression.Or`                 | Bitwise OR                        |
| `\|\|`        | `Expression.OrElse`             | Logical OR                        |
| `=`           | `Expression.Equal`              | Equal                             |
| `!=`          | `Expression.NotEqual`           | Not equal                         |
| `>`           | `Expression.GreaterThan`        | Greater than                      |
| `>=`          | `Expression.GreaterThanOrEqual` | Greater than or equal             |
| `<`           | `Expression.LessThan`           | Less than                         |
| `<=`          | `Expression.LessThanOrEqual`    | Less than or equal                |
| `in`          | `ExpressionOperators.In`        | In collection                     |
| `as lower in` | `ExpressionOperators.InStrings` | Element lower-cased in collection |
| `as upper in` | `ExpressionOperators.InStrings` | Element upper-cased in collection |

### 🌟 Add Custom Operators

```csharp
JsonLinq.Configure(options =>
  {
    options.OperatorMapper
      .Add("lt", Expression.LessThan)
      .Add("gt", Expression.GreaterThan);
  });
```
