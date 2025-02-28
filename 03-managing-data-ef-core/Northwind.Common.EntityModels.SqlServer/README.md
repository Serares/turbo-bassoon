Used to scaffold

```bash
dotnet ef dbcontext scaffold "Server=127.0.0.1,1433;Database=Northwind;User Id=sa;Password=s3cret-Ninja;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --namespace Northwind.EntityModels --data-annotations
```