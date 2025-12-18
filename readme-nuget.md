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

```csharp
using Neomaster.JsonToLinq;

var users = source.Where(
  """
  {
    "Logic": "&&",
    "Rules": [
      {
        "Field": "balance",
        "Operator": "=",
        "Value": 0
      },
      {
        "Logic": "||",
        "Rules": [
          {
            "Field": "lastVisitAt",
            "Operator": "=",
            "Value": null
          },
          {
            "Field": "lastVisitAt",
            "Operator": "<=",
            "Value": "2025-01-01T00:00:00Z"
          }
        ]
      }
    ]
  }
  """);
```

## 🛠️ Operators

👓 Operators and their handling logic are encapsulated in class `ExpressionOperatorMapper`.
The default operator mapping can be accessed via property `Pairs`.

🔠 String comparisons are case-sensitive, depending on the database collation, not on the operators.

### 📌 Default Operators

| JSON   | LINQ Expression                 | Description           |
|--------|---------------------------------|-----------------------|
| `&`    | `Expression.And`                | Bitwise AND           |
| `&&`   | `Expression.AndAlso`            | Logical AND           |
| `|`   | `Expression.Or`                 | Bitwise OR            |
| `||` | `Expression.OrElse`             | Logical OR            |
| `=`    | `Expression.Equal`              | Equal                 |
| `!=`   | `Expression.NotEqual`           | Not equal             |
| `>`    | `Expression.GreaterThan`        | Greater than          |
| `>=`   | `Expression.GreaterThanOrEqual` | Greater than or equal |
| `<`    | `Expression.LessThan`           | Less than             |
| `<=`   | `Expression.LessThanOrEqual`    | Less than or equal    |
| `in`   | `ExpressionOperators.In`        | In collection         |

### 🌟 Add Custom Operators

```csharp
JsonLinq.Configure(options =>
  {
    options.OperatorMapper
      .Add("lt", Expression.LessThan)
      .Add("gt", Expression.GreaterThan);
  });
```

## 🔬 Demos and Experiments

This repository contains a project with **working examples**.
You are welcome to submit a PR with your own examples, bug reports, or new features.
**To describe a filter, use the following notation:**

### 🔤 Filter Notation

**Syntax**
```
expr = logic[expr(, expr)*]
```
- `expr` - a single rule or a combination of rules
- `logic` - an operator used to combine multiple rules, e.g. `&&`, `&`, `||`, `|`, or a custom one

**Examples**
1. `&&[x = null]`
1. `&&[a < 0, b > 0]`
1. `&&[x = null, ||[a < 0, b > 0]]`

### 💻 Demo Project

This project provides examples of working with **EF Core** and a **PostgreSQL database**.

🧪 A real database is used instead of in-memory storage, ensuring clean and realistic experiments.

➕ You can add your own examples with other databases or ORMs via a PR.

▶️ Before running the demos, select the first menu item, **Prepare Data**, to apply migrations and populate the tables with test data.

