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
