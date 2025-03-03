using Microsoft.Azure.Cosmos;
using System.Net;

partial class Program
{
    private static string endpointUri = "https://localhost:8081";
    private static string primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

    static async Task CreateCosmosResources()
    {
        SectionTitle("Creating Cosmos resources");

        try
        {
            using (CosmosClient client = new(
                accountEndpoint: endpointUri,
                authKeyOrResourceToken: primaryKey
            ))
            {
                DatabaseResponse dbResponse = await client.CreateDatabaseIfNotExistsAsync(
                    "Northwind", throughput: 400
                );
                string status = dbResponse.StatusCode switch
                {
                    HttpStatusCode.OK => "exists",
                    HttpStatusCode.Created => "created",
                    _ => "unknown"
                };
                WriteLine("Database Id: {0}, Status: {1}.",
                    arg0: dbResponse.Database.Id,
                    arg1: status);

                IndexingPolicy indexingPolicy = new()
                {
                    IndexingMode = IndexingMode.Consistent,
                    Automatic = true,
                    IncludedPaths = { new IncludedPath { Path = "/*" } },
                };

                ContainerProperties containerProperties = new("Products", partitionKeyPath: "/productId")
                {
                    IndexingPolicy = indexingPolicy
                };

                ContainerResponse containerResponse = await dbResponse.Database.CreateContainerIfNotExistsAsync(
                    containerProperties, throughput: 1000
                );
                status = dbResponse.StatusCode switch
                {
                    HttpStatusCode.OK => "exists",
                    HttpStatusCode.Created => "created",
                    _ => "unknown"
                };

                WriteLine("Container Id: {0}, Status: {1}.",
                    arg0: containerResponse.Container.Id,
                    arg1: status);


                Container container = containerResponse.Container;
                ContainerProperties properties = await container.ReadContainerAsync();

                WriteLine($" PartitionKeyPath: {properties.PartitionKeyPath}");
                WriteLine($" LastModified: {properties.LastModified}");
                WriteLine(" IndexingPolicy.IndexingMode: {0}",
                arg0: properties.IndexingPolicy.IndexingMode);
                WriteLine(" IndexingPolicy.IncludedPaths: {0}",
                arg0: string.Join(",", properties.IndexingPolicy
                .IncludedPaths.Select(path => path.Path)));
                WriteLine($" IndexingPolicy: {properties.IndexingPolicy}");

            }
        }
        catch (HttpRequestException ex)
        {
            WriteLine($"Error: {ex.Message}");
            WriteLine($"Error: {ex.StackTrace}");
        }
        catch (Exception ex)
        {
            WriteLine("Error: {0} says {1}", arg0: ex.GetType(), arg1: ex.Message);
        }
    }
}