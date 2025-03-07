using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        HttpClient client = new HttpClient();
        int concurrentRequests = 100;

        // Console.WriteLine("Testing async endpoint:");
        // await TestEndpoint(client, "https://localhost:5081/api/products/async", concurrentRequests);

        Console.WriteLine("\nTesting sync endpoint:");
        await TestEndpoint(client, "https://localhost:5081/api/products/sync", concurrentRequests);
    }

    static async Task TestEndpoint(HttpClient client, string url, int concurrentRequests)
    {
        Stopwatch sw = Stopwatch.StartNew();

        // Create multiple concurrent requests
        Task[] tasks = new Task[concurrentRequests];
        for (int i = 0; i < concurrentRequests; i++)
        {
            tasks[i] = MakeRequest(client, url);
        }

        // Wait for all requests to complete
        await Task.WhenAll(tasks);

        sw.Stop();
        Console.WriteLine($"Completed {concurrentRequests} requests in {sw.ElapsedMilliseconds}ms");
        Console.WriteLine($"Average: {sw.ElapsedMilliseconds / (double)concurrentRequests}ms per request");
    }

    static async Task MakeRequest(HttpClient client, string url)
    {
        await client.GetAsync(url);
    }
}