## 🛠️ Operators

👓 Operators and their handling logic are encapsulated in class `ExpressionOperatorMapper`.
The default operator mapping can be accessed via property `Pairs`.

### 📌 Default Operators

| Operator               | Description                            |
|------------------------|----------------------------------------|
| `&`                    | Bitwise AND                            |
| `&&`                   | Logical AND                            |
| `\|`                   | Bitwise OR                             |
| `\|\|`                 | Logical OR                             |
| `=`                    | Equal                                  |
| `!=`                   | Not equal                              |
| `>`                    | Greater than                           |
| `>=`                   | Greater than or equal                  |
| `<`                    | Less than                              |
| `<=`                   | Less than or equal                     |
| `in`                   | In collection                          |
| `as lower in`          | Element lower-cased in collection      |
| `as upper in`          | Element upper-cased in collection      |
| `starts with`          | Starts with                            |
| `as lower starts with` | Element lower-cased starts with        |
| `as upper starts with` | Element upper-cased starts with        |
| `ends with`            | Ends with                              |
| `as lower ends with`   | Element lower-cased ends with          |
| `as upper ends with`   | Element upper-cased ends with          |
| `contains`             | Contains substring                     |
| `as lower contains`    | Element lower-cased contains substring |
| `as upper contains`    | Element upper-cased contains substring |

#### Negated Operators

- `!in`                   
- `! as lower in`         
- `! as upper in`         
- `! starts with`         
- `! as lower starts with`
- `! as upper starts with`
- `! ends with`           
- `! as lower ends with`  
- `! as upper ends with`  
- `! contains`            
- `! as lower contains`   
- `! as upper contains`   

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

Negated operators can be created without explicitly providing a key.
In this case, the key is automatically generated using the `NegateKeyProvider`.
This provider can be set via `SetNegateKeyProvider()`.

```csharp
new ExpressionOperatorMapper()

// Default negate key provider
.Add("a", ...).WithNot()   // "!a"
.Add("b c", ...).WithNot() // "! b c"

// Custom negate key provider
.SetNegateKeyProvider(key => (key.Contains(' ') ? "~ " : "~") + key)
.Add("x", ...).WithNot()   // "~x"
.Add("y z", ...).WithNot() // "~ y z"
```
