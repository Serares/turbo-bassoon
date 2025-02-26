using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;
using System.IO;

ConfigureConsole("ro-RO");
SqlConnectionStringBuilder builder = new()
{
    InitialCatalog = "Northwind",
    MultipleActiveResultSets = true,
    Encrypt = true,
    TrustServerCertificate = true,
    ConnectTimeout = 10
};

WriteLine("Connect to:");
WriteLine("  1 - SQL Server on local machine");
WriteLine("  2 - Azure SQL Database");
WriteLine("  3 – Azure SQL Edge");
WriteLine();
Write("Press a key: ");

ConsoleKey key = ReadKey().Key;
WriteLine(); WriteLine();

switch (key)
{
    case ConsoleKey.D1 or ConsoleKey.NumPad1:
        builder.DataSource = ".";
        break;
    case ConsoleKey.D2 or ConsoleKey.NumPad2:
        builder.DataSource =
          // Use your Azure SQL Database server name.
          "tcp:apps-services-book.database.windows.net,1433";
        break;
    case ConsoleKey.D3 or ConsoleKey.NumPad3:
        builder.DataSource = Constants.AZURE_EDGE_CONNECTION_STRING;
        break;
    default:
        WriteLine("No data source selected.");
        return;
}

WriteLine("Authenticate using:");
WriteLine("  1 – Windows Integrated Security");
WriteLine("  2 – SQL Login, for example, sa");
WriteLine();
Write("Press a key: ");

key = ReadKey().Key;
WriteLine(); WriteLine();

if (key is ConsoleKey.D1 or ConsoleKey.NumPad1)
{
    builder.IntegratedSecurity = true;
}
else if (key is ConsoleKey.D2 or ConsoleKey.NumPad2)
{
    string? userId = null;
    string? password = null;
    if (File.Exists(Constants.CREDENTIALS_FILE))
    {
        try
        {
            string json = File.ReadAllText(Constants.CREDENTIALS_FILE);
            UserCredentials? cachedCredentials = JsonSerializer.Deserialize<UserCredentials>(json);

            if (cachedCredentials is not null)
            {
                userId = cachedCredentials.UserId;
                password = cachedCredentials.Password;
            }
        }
        catch (Exception ex)
        {
            WriteLineInColor("Can't find the cached file", ConsoleColor.Red);
        }
    }

    if (string.IsNullOrWhiteSpace(userId))
    {
        Write("Enter your SQL Server user ID: ");
        userId = ReadLine();
        if (string.IsNullOrWhiteSpace(userId))
        {
            WriteLine("User ID cannot be empty or null.");
            return;
        }
    }

    builder.UserID = userId;

    if (string.IsNullOrWhiteSpace(password))
    {
        Write("Enter your SQL Server password: ");
        password = ReadLine();
        if (string.IsNullOrWhiteSpace(password))
        {
            WriteLine("Password cannot be empty or null.");
            return;
        }
    }

    builder.Password = password;
    builder.PersistSecurityInfo = false;

    if (builder.DataSource == Constants.AZURE_EDGE_CONNECTION_STRING)
    {
        UserCredentials credentials = new()
        {
            UserId = userId,
            Password = password
        };
        string json = JsonSerializer.Serialize(credentials);
        File.WriteAllText(Constants.CREDENTIALS_FILE, json);
    }
}
else
{
    WriteLine("No authentication selected.");
    return;
}


SqlConnection connection = new(builder.ConnectionString);

WriteLine(connection.ConnectionString);
WriteLine();

connection.StateChange += Connection_StateChange;
connection.InfoMessage += Connection_InfoMessage;

try
{
    WriteLine("Opening connection. Please wait up to {0} seconds...",
      builder.ConnectTimeout);
    WriteLine();
    await connection.OpenAsync();

    WriteLine($"SQL Server version: {connection.ServerVersion}");

    connection.StatisticsEnabled = true;
}
catch (SqlException ex)
{
    WriteLineInColor($"SQL exception: {ex.Message}",
      ConsoleColor.Red);
    return;
}

Write("Enter a unit price: ");

string? priceText = ReadLine();
if (!decimal.TryParse(priceText, out decimal price))
{
    WriteLine("You must enter a valid price");
    return;
}

SqlCommand cmd = connection.CreateCommand();

WriteLine("Execute command using:");
WriteLine("1 - Text");
WriteLine("2 - Stored Procedure");
WriteLine();

Write("Press a key: ");

key = ReadKey().Key;
WriteLine(); WriteLine();

SqlParameter p1, p2 = new(), p3 = new();

if (key is ConsoleKey.D1 or ConsoleKey.NumPad1)
{
    cmd.CommandType = CommandType.Text;
    cmd.CommandText = "SELECT ProductId, ProductName, UnitPrice FROM Products WHERE UnitPrice >= @minimumPrice";
    cmd.Parameters.AddWithValue("minimumPrice", price);
}
else if (key is ConsoleKey.D2 or ConsoleKey.NumPad2)
{
    cmd.CommandType = CommandType.StoredProcedure;
    cmd.CommandText = "GetExpensiveProducts";
    p1 = new()
    {
        ParameterName = "price",
        SqlDbType = SqlDbType.Money,
        SqlValue = price
    };
    p2 = new()
    {
        Direction = ParameterDirection.Output,
        SqlDbType = SqlDbType.Int,
        ParameterName = "count"
    };

    p3 = new()
    {
        Direction = ParameterDirection.ReturnValue,
        ParameterName = "rv",
        SqlDbType = SqlDbType.Int
    };
    cmd.Parameters.AddRange(new[] { p1, p2, p3 });
}


SqlDataReader r = await cmd.ExecuteReaderAsync();
string horizontalLine = new string('-', 60);
WriteLine(horizontalLine);
WriteLine("| {0,5} | {1,-35} | {2,10} |",
arg0: "Id", arg1: "Name", arg2: "Price");
WriteLine(horizontalLine);
string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), Constants.JSON_FILE);

await using (FileStream jsonStream = File.Create(jsonPath))
{
    Utf8JsonWriter jsonWriter = new(jsonStream);
    jsonWriter.WriteStartArray();
    while (await r.ReadAsync())
    {
        WriteLine("| {0,5} | {1,-35} | {2,10:C} |",
        await r.GetFieldValueAsync<int>("ProductId"),
        await r.GetFieldValueAsync<string>("ProductName"),
        await r.GetFieldValueAsync<decimal>("UnitPrice"));
        jsonWriter.WriteStartObject();
        jsonWriter.WriteNumber("ProductId", await r.GetFieldValueAsync<int>("ProductId"));
        jsonWriter.WriteString("ProductName", await r.GetFieldValueAsync<string>("ProductName"));
        jsonWriter.WriteNumber("UnitPrice", await r.GetFieldValueAsync<decimal>("UnitPrice"));
        jsonWriter.WriteEndObject();
    }
    jsonWriter.WriteEndArray();
    jsonWriter.Flush();
    jsonStream.Close();
}
WriteLine(horizontalLine);
await r.CloseAsync();


if (key is ConsoleKey.D2 or ConsoleKey.NumPad2)
{
    WriteLine($"Output count: {p2.Value}");
    WriteLine($"Return value: {p3.Value}");
}

OutputStatistics(connection);
await connection.CloseAsync();