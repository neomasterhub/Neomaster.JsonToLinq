## 🛠️ Operators

👓 Operators and their handling logic are encapsulated in class `ExpressionOperatorMapper`.
The default operator mapping can be accessed via property `Pairs`.

🔠 String comparisons are case-sensitive, depending on the database collation, not on the operators.

🤝 The case of string values specified in the filter is never changed automatically.
Case normalization is the client’s responsibility.

### 📌 Default Operators

| JSON                   | Base LINQ Expression             | Description                       |
|------------------------|----------------------------------|-----------------------------------|
| `&`                    | `Expression.And`                 | Bitwise AND                       |
| `&&`                   | `Expression.AndAlso`             | Logical AND                       |
| `\|`                   | `Expression.Or`                  | Bitwise OR                        |
| `\|\|`                 | `Expression.OrElse`              | Logical OR                        |
| `=`                    | `Expression.Equal`               | Equal                             |
| `!=`                   | `Expression.NotEqual`            | Not equal                         |
| `>`                    | `Expression.GreaterThan`         | Greater than                      |
| `>=`                   | `Expression.GreaterThanOrEqual`  | Greater than or equal             |
| `<`                    | `Expression.LessThan`            | Less than                         |
| `<=`                   | `Expression.LessThanOrEqual`     | Less than or equal                |
| `in`                   | `ExpressionOperators.In`         | In collection                     |
| `as lower in`          | `ExpressionOperators.InStrings`  | Element lower-cased in collection |
| `as upper in`          | `ExpressionOperators.InStrings`  | Element upper-cased in collection |
| `starts with`          | `ExpressionOperators.StartsWith` | Starts with                       |
| `as lower starts with` | `ExpressionOperators.StartsWith` | Element lower-cased starts with   |
| `as upper starts with` | `ExpressionOperators.StartsWith` | Element upper-cased starts with   |
| `ends with`            | `ExpressionOperators.EndsWith`   | Ends with                         |
| `as lower ends with`   | `ExpressionOperators.EndsWith`   | Element lower-cased ends with     |
| `as upper ends with`   | `ExpressionOperators.EndsWith`   | Element upper-cased ends with     |

### 🌟 Add Custom Operators

```csharp
JsonLinq.Configure(options =>
  {
    options.OperatorMapper
      .Add("lt", Expression.LessThan)
      .Add("gt", Expression.GreaterThan);
  });
```
