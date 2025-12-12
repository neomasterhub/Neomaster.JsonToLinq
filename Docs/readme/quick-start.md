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
