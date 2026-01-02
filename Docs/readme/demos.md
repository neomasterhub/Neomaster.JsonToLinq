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
