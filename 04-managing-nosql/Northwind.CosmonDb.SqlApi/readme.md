```
We create new models because the scaffolded 
models from Northwind.Common.EntityModels.SqlServer are designed for normalized data structure
So we need models to better represent a noSQL db
```

```
Members of the model classes don't follow .NET casing conventions 
because serialization can't be dynamically manipulated and JSON has to use camel case
```