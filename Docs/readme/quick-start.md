## 🚀 Quick Start

🗄️ With **EF**, create predicates using `JsonLinq.ParseFilterExpression(json)`.

```csharp
using Neomaster.JsonToLinq;

var users = source.Where(
  """
  {
    "Logic": "&&",
    "Rules": [
      { "Field": "balance", "Operator": "=", "Value": 0 },
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
