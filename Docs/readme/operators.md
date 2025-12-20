## 🛠️ Operators

👓 Operators and their handling logic are encapsulated in class `ExpressionOperatorMapper`.
The default operator mapping can be accessed via property `Pairs`.

### 📌 Default Operators

| Operator               | Base LINQ Expression             | Description                            |
|------------------------|----------------------------------|----------------------------------------|
| `&`                    | `Expression.And`                 | Bitwise AND                            |
| `&&`                   | `Expression.AndAlso`             | Logical AND                            |
| `\|`                   | `Expression.Or`                  | Bitwise OR                             |
| `\|\|`                 | `Expression.OrElse`              | Logical OR                             |
| `=`                    | `Expression.Equal`               | Equal                                  |
| `!=`                   | `Expression.NotEqual`            | Not equal                              |
| `>`                    | `Expression.GreaterThan`         | Greater than                           |
| `>=`                   | `Expression.GreaterThanOrEqual`  | Greater than or equal                  |
| `<`                    | `Expression.LessThan`            | Less than                              |
| `<=`                   | `Expression.LessThanOrEqual`     | Less than or equal                     |
| `in`                   | `ExpressionOperators.In`         | In collection                          |
| `as lower in`          | `ExpressionOperators.InStrings`  | Element lower-cased in collection      |
| `as upper in`          | `ExpressionOperators.InStrings`  | Element upper-cased in collection      |
| `starts with`          | `ExpressionOperators.StartsWith` | Starts with                            |
| `as lower starts with` | `ExpressionOperators.StartsWith` | Element lower-cased starts with        |
| `as upper starts with` | `ExpressionOperators.StartsWith` | Element upper-cased starts with        |
| `ends with`            | `ExpressionOperators.EndsWith`   | Ends with                              |
| `as lower ends with`   | `ExpressionOperators.EndsWith`   | Element lower-cased ends with          |
| `as upper ends with`   | `ExpressionOperators.EndsWith`   | Element upper-cased ends with          |
| `contains`             | `ExpressionOperators.Contains`   | Contains substring                     |
| `as lower contains`    | `ExpressionOperators.Contains`   | Element lower-cased contains substring |
| `as upper contains`    | `ExpressionOperators.Contains`   | Element upper-cased contains substring |

### 🔁 LINQ Translation

All default operators are designed to be translatable by LINQ providers
(e.g. Entity Framework Core) and do not rely on client-side evaluation.

### 🔠 Case Sensitivity and Normalization

1. String comparisons are case-sensitive, depending on the database collation, not on the operators.
1. The case of string values specified in the filter is never changed automatically.
1. Case normalization is the client’s responsibility.
1. Operators with `as lower` / `as upper` apply case transformation
   to the expression being evaluated, not to the filter value.
   The filter value is used exactly as provided.

### 🌟 Add Custom Operators

```csharp
JsonLinq.Configure(options =>
  {
    options.OperatorMapper
      .Add("lt", Expression.LessThan)
      .Add("gt", Expression.GreaterThan);
  });
```
