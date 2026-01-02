## 🚀 Quick Start<a name="quick-start"/>

1. Install the [NuGet package][package].
1. Pass JSON filter into `Where()` or other filtering methods.
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
