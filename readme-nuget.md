# JsonToLinq

**JsonToLinq** - lightweight C# library that converts JSON-based query definitions into LINQ expressions. Ideal for building dynamic filters, predicates, and queries.

## ⚡ TL;DR

Use JSON to build LINQ expressions!

## 🤩 Advantages

1. **Friendliness**

   JSON is a simple and widely known format, easy to read not only for developers.

1. **Broad applicability**

   JSON-based filters can be used in tests, described in specifications, test plans, and other technical documentation, lowering the entry barrier for readers.

1. **Universality**

   To retrieve data from a .NET application using EF, it is enough to request it in JSON format. This makes the approach accessible to any client that knows the field names and their types.

1. **Client-server independence**

   To build filters, the client only needs to know the names and types of data fields, which are usually already present in DTOs.

1. **Flexibility**

   Custom operators can be added out of the box, and the core functionality can serve as a foundation for other projects - for example, building an alternative to [HotChocolate][hot-chocolate] with standard JSON and different design choices.

1. **Simplicity**

   Install the NuGet package and pass JSON filters into `Where()` and other LINQ methods. This enables not a minimal subset, but the full filtering functionality.

   **No need to:**
   
   - Register anything in `Program.cs`
   - Create schemas, separate DTOs, filters, resolvers, etc.
   - Generate a schema for the client
   
   Everything required for filtering is already contained in the DTOs.

[hot-chocolate]: https://chillicream.com/docs/hotchocolate

## 🚀 Quick Start

1. Install the [NuGet package][package].
1. Pass JSON text into `Where()` or other filtering methods.
1. To use with `IQueryable`, first create a predicate via `JsonLinq.ParseFilterExpression()`.

```csharp
using Neomaster.JsonToLinq;

var users = source.Where(
  """
  {
    "Logic": "&&",
    "Rules": [
      { "Field": "balance", "Operator": "=", "Value": 0 },
      { "Field": "status", "Operator": "in", "Value": [ 1, 3 ] },
      { "Field": "country", "Operator": "as lower contains", "Value": "islands" },
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

Equivalent LINQ query:

```csharp
var users = source.Where(u =>
  (u.Balance == 0
  && new[] { 1, 3 }.Contains(u.Status)
  && u.Country.ToLower().Contains("islands"))
  &&
  (u.LastVisitAt == null
  || u.LastVisitAt <= JsonSerializer.Deserialize<DateTime?>("\"2026-01-01T00:00:00Z\"")));
```

[package]: https://www.nuget.org/packages/JsonToLinq

## 🛠️ Operators

1. Built-in operators and their handling logic are encapsulated in the `ExpressionOperatorMapper` class.
1. The mapping between operators and their processing methods is available via the `ExpressionOperatorMapper.Pairs` property.
1. Operator keys are case-sensitive.

### 📌 Built-in Operators

| Operator               | Description                            |
|------------------------|----------------------------------------|
| `&`                    | Bitwise AND                            |
| `|`                   | Bitwise OR                             |
| `&&` / `and`           | Logical AND                            |
| `||` / `or`          | Logical OR                             |
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
1. Using not is not always grammatically correct.
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

## 🧪 Testing

Unit tests cover:

1. Everything involved in parsing JSON filters
1. Operators with custom expressions (`in`, `contains`, etc.)
1. Configuration methods
1. `IEnumerable` extension methods

Full unit test coverage will be relevant after the library has been used in real projects.

## 🔬 Demos and Experiments

This repository includes the [JsonToLinq.Demo][demo-project] project with **working examples**.
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

[demo-project]: https://github.com/neomasterhub/Neomaster.JsonToLinq/tree/master/Neomaster.JsonToLinq.Demo

![Demo project](https://github.com/neomasterhub/Neomaster.JsonToLinq/blob/master/Docs/img/demo-project.png?raw=true "Demo project")

## 🚧 Limitations

1. **No IDE syntax highlighting for JSON filter arguments in LINQ methods**

   The library targets `netstandard2.1`, which does not support `StringSyntaxAttribute`.
   Care is needed when writing filters manually. In practice, this is not critical, as filters typically come from client applications.

1. **No IntelliSense for JSON filters**
   
   Without suggestions, it is harder to ensure filter correctness.

1. **Aggregate fields are not supported**

   For now, it is recommended to use **flat DTOs** or **database views**.

   ```json
   {
     "Rules": [
       { "Field": "✅ department_id", "Operator": "=", "Value": 123 },
       { "Field": "❌ department.id", "Operator": "=", "Value": 123 }
     ]
   }
   ```

   These features may be implemented in future versions...

## 🔮 Potential

Filtering is just the first stage in the development of JSON-LINQ infrastructure.
Possible future directions include:

1. Data selection - JSON for `Select()`
1. Data grouping - JSON for `GroupBy()`
1. GraphQL engine using standard JSON (as opposed to HotChocolate)
1. Server-side equivalents of RxJS/NgRx for reactive data processing
1. Interactive query-building studios - visual builders with *canvas*, *drag-and-drop*, *flowcharts*, *node-based UI*, *etc.*
1. Semantic search and NLP
1. A new standard for data exchange between services

