using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Northwind.EntityModels;

public partial class NorthwindContext : DbContext
{
    private static readonly SetLastRefreshedInterceptor setLastRefreshedInterceptor = new();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            SqlConnectionStringBuilder builder = new()
            {
                DataSource = "localhost",
                InitialCatalog = "Northwind",
                TrustServerCertificate = true,
                UserID = "sa",
                Password = "s3cret-Ninja"
            };

            optionsBuilder.UseSqlServer(builder.ConnectionString);
        }
        optionsBuilder.AddInterceptors(setLastRefreshedInterceptor);
    }
}