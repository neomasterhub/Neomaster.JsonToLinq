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
