using Microsoft.Data.SqlClient;
using Northwind.Models;
using System.Data;
namespace WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapGets(this WebApplication app)
    {
        // app.MapGet(pattern, handler);

        app.MapGet("/", () => "Hello from a native AOT minimal API web service.");
        app.MapGet("/products", GetProducts);
        app.MapGet("/products/{minimumUnitPrice:decimal?}", GetProducts);
        return app;
    }

    public static List<Product> GetProducts(decimal? minimumUnitPrice = null)
    {
        // ❗we cant use EF Core with AOT
        SqlConnectionStringBuilder builder = new();

        builder.InitialCatalog = "Northwind";
        builder.MultipleActiveResultSets = true;
        builder.Encrypt = true;
        builder.TrustServerCertificate = true;
        builder.ConnectTimeout = 10;
        builder.DataSource = "tcp:127.0.0.1,1433";

        // Because we want to fail fast. Default is 15 seconds.
        builder.ConnectTimeout = 3;

        // If using Windows Integrated authentication.
        // builder.IntegratedSecurity = true;

        // If using SQL Server authentication.
        builder.UserID = "sa";
        builder.Password = "s3cret-Ninja";

        SqlConnection connection = new(builder.ConnectionString);

        connection.Open();

        SqlCommand cmd = connection.CreateCommand();

        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "SELECT ProductId, ProductName, UnitPrice FROM Products";

        if (minimumUnitPrice.HasValue)
        {
            cmd.CommandText += " WHERE UnitPrice >= @minimumUnitPrice";
            cmd.Parameters.AddWithValue("minimumUnitPrice", minimumUnitPrice);
        }


        SqlDataReader r = cmd.ExecuteReader();

        List<Product> products = new();

        while (r.Read())
        {
            Product p = new()
            {
                ProductId = r.GetInt32("ProductId"),
                ProductName = r.GetString("ProductName"),
                UnitPrice = r.GetDecimal("UnitPrice")
            };
            products.Add(p);
        }

        r.Close();

        return products;
    }
}