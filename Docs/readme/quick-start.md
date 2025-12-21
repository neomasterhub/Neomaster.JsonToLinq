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
