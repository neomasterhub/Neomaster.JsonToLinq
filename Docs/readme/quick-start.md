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
