# JsonToLinq

**JsonToLinq** - lightweight C# library that converts JSON-based query definitions into LINQ expressions. Ideal for building dynamic filters, predicates, and queries.

## 🎯 Use Cases

**JsonToLinq** can be applied in various scenarios where dynamic, runtime-defined queries are needed. Examples include:

- **Server-side filtering:** Apply JSON-defined filters received from front-end applications to collections or database queries.
- **Dynamic reporting:** Build complex filters and predicates for reports without hardcoding logic.
- **Custom dashboards:** Let users define queries for dashboards dynamically and translate them to LINQ expressions.
- **EF Core / Entity Framework queries:** Map JSON filters directly to LINQ queries executed on the database.
- **Audit & logging filters:** Dynamically select subsets of data based on JSON rules for auditing or logging purposes.

## 🚀 Quick Start

🗄️ With **Entity Framework**, you can create LINQ predicates 
directly from JSON using `JsonLinq.ParseFilterExpression()`.

```csharp
using Neomaster.JsonToLinq;

var users = source.Where(
  """
  {
    "Logic": "&&",
    "Rules": [
      { "Field": "balance", "Operator": "=", "Value": 0 },
      { "Field": "country", "Operator": "as lower contains", "Value": "islands" },
      { "Field": "email", "Operator": "!in", "Value": [ "admin@org.com", "su@org.com" ] },
      {
        "Logic": "||",
        "Rules": [
          { "Field": "lastVisitAt", "Operator": "=", "Value": null },
          { "Field": "lastVisitAt", "Operator": "<=", "Value": "2026-01-01T00:00:00Z" }
        ]
      }
    ]
  }
  """);
```

## 🛠️ Operators

👓 Operators and their handling logic are encapsulated in class `ExpressionOperatorMapper`.
The default operator mapping can be accessed via property `Pairs`.

### 📌 Default Operators

| Operator               | Description                            |
|------------------------|----------------------------------------|
| `&`                    | Bitwise AND                            |
| `&&`                   | Logical AND                            |
| `|`                   | Bitwise OR                             |
| `||`                 | Logical OR                             |
| `=`                    | Equal                                  |
| `!=`                   | Not equal                              |
| `>`                    | Greater than                           |
| `>=`                   | Greater than or equal                  |
| `<`                    | Less than                              |
| `<=`                   | Less than or equal                     |
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

- `!in`                   
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

## 🔬 Demos and Experiments

This repository contains a project with **working examples**.
You are welcome to submit a PR with your own examples, bug reports, or new features.
**To describe a filter, use the following notation:**

### 🔤 Filter Notation

#### Syntax
```
expr = logic[expr(, expr)*]
```
- `expr` - a single rule or a combination of rules
- `logic` - an operator used to combine multiple rules, e.g. `&&`, `&`, `||`, `|`, or a custom one

#### Examples
1. `&&[x = null]`
1. `&&[a < 0, b > 0]`
1. `&&[x = null, ||[a < 0, b > 0]]`

### 💻 Demo Project

This project provides examples of working with **EF Core** and a **PostgreSQL database**.

🧪 A real database is used instead of in-memory storage, ensuring clean and realistic experiments.

➕ You can add your own examples with other databases or ORMs via a PR.

▶️ Before running the demos, select the first menu item, **Prepare Data**, to apply migrations and populate the tables with test data.

