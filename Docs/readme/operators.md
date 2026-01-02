## 🛠️ Operators

1. Built-in operators and their handling logic are encapsulated in the `ExpressionOperatorMapper` class.
1. The mapping between operators and their processing methods is available via the `ExpressionOperatorMapper.Pairs` property.
1. Operator keys are case-sensitive.

### 📌 Built-in Operators

| Operator               | Description                            |
|------------------------|----------------------------------------|
| `&`                    | Bitwise AND                            |
| `\|`                   | Bitwise OR                             |
| `&&` / `and`           | Logical AND                            |
| `\|\|` / `or`          | Logical OR                             |
| `=` / `eq`             | Equal                                  |
| `!=` / `neq`           | Not equal                              |
| `>` / `gt`             | Greater than                           |
| `>=` / `gte`           | Greater than or equal                  |
| `<` / `lt`             | Less than                              |
| `<=` / `lte`           | Less than or equal                     |
| `in`                   | In collection                          |
| `as lower in`          | Element lower-cased in collection      |
| `as upper in`          | Element upper-cased in collection      |
| `contains`             | Contains substring                     |
| `as lower contains`    | Element lower-cased contains substring |
| `as upper contains`    | Element upper-cased contains substring |
| `starts with`          | Starts with                            |
| `as lower starts with` | Element lower-cased starts with        |
| `as upper starts with` | Element upper-cased starts with        |
| `ends with`            | Ends with                              |
| `as lower ends with`   | Element lower-cased ends with          |
| `as upper ends with`   | Element upper-cased ends with          |

#### Negated Operators

- `!in` / `not in`                   
- `! as lower in`         
- `! as upper in`         
- `!contains`            
- `! as lower contains`   
- `! as upper contains`   
- `! starts with`         
- `! as lower starts with`
- `! as upper starts with`
- `! ends with`           
- `! as lower ends with`  
- `! as upper ends with`  

#### The word `not`

The word `not` is a synonym for `!`.

It improves readability but is not suitable for all operators:

1. Operators expressed as words become even longer.
1. Using `not` is not always grammatically correct.
   - ❌ *not contains*
   - ✅ *does not contain*

### 🔁 SQL Translation

All built-in operators are designed to be translatable by LINQ providers
(e.g. Entity Framework Core) and do not rely on client-side evaluation.

### 🔠 Case Sensitivity and Normalization

1. String comparisons are case-sensitive, depending on the database collation, not on the operators.
1. The case of string values specified in the filter is never changed automatically.
1. Case normalization is the client’s responsibility.
1. Operators with `as lower` / `as upper` apply case transformation
   to the expression being evaluated, not to the filter value.
   The filter value is used exactly as provided.

### 📦 Filter Collections

1. A filter collection can be empty, but it is never `null`.
1. A filter collection and its elements are never automatically changed.
1. Operators with `as lower` / `as upper` apply string transformations
   to each element in the source collection before evaluation,
   leaving the filter collection elements unchanged.

### 🌟 Add Custom Operators

#### Fully Custom Operators

```csharp
JsonLinq.Configure(options =>
{
  options.OperatorMapper = new ExpressionOperatorMapper()
    .Add("=", Expression.Equal)
      .WithAliases("eq", "EQ")
    .AddNot("!=", "=")
      .WithAliases("neq", "NEQ")
    .AddAlias("==", "=")
      .WithNot("<>");
});
```

#### Extend Default Operators

```csharp
JsonLinq.Configure(options =>
{
  options.OperatorMapper = ExpressionOperatorMapper.OnDefault()
    .Add(...
});
```

#### Negated Operators

Negated operators can be defined without explicitly providing a key.
In this case, the key is automatically generated using the `NegatedKeyProvider`.
This provider can be set via `SetNegatedKeyProvider()`.

```csharp
new ExpressionOperatorMapper()

// Default provider
.Add("a", ...).WithNot()   // "!a"
.Add("b c", ...).WithNot() // "! b c"

// Custom provider
.SetNegatedKeyProvider(key => (key.Contains(' ') ? "~ " : "~") + key)
.Add("x", ...).WithNot()   // "~x"
.Add("y z", ...).WithNot() // "~ y z"
```

#### SQL operators

The library is built on `netstandard2.1` and **does not depend on EF** or any other ORM.
To perform case-insensitive string comparisons, 
use the built-in operators with `as lower` or `as upper`.
They differ only in the preferred case for filter values.

```csharp
JsonLinq.Configure(options =>
{
  options.OperatorMapper = ExpressionOperatorMapper.OnDefault()
    .Add("like", (element, pattern) =>
      Expression.Call(
        typeof(DbFunctionsExtensions).GetMethod(
          nameof(DbFunctionsExtensions.Like),
          [typeof(DbFunctions), typeof(string), typeof(string)]),
        Expression.Constant(EF.Functions),
        element,
        pattern))
    .Add("ilike", (element, pattern) =>
      Expression.Call(
        typeof(NpgsqlDbFunctionsExtensions).GetMethod(
          nameof(NpgsqlDbFunctionsExtensions.ILike),
          [typeof(DbFunctions), typeof(string), typeof(string)]),
        Expression.Constant(EF.Functions),
        element,
        pattern));
});
```
