using Microsoft.Azure.Functions.Worker; // to use [Function]
using Microsoft.Extensions.Logging; // to use ILogger
using System.IO.Compression; // to use GZipStream, CompressionMode.
using System.Net.Http; // to use HttpClient
namespace Northwind.AzureFunctions.Service;

public class ScrapeAmazonFunction
{
    private const string relativePath = "12-NET-Cross-Platform-Development-Fundamentals/dp/1837635870/";
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger _logger;
    public ScrapeAmazonFunction(IHttpClientFactory clientFactory,
    ILoggerFactory loggerFactory
    )
    {
        _clientFactory = clientFactory;
        _logger = loggerFactory.CreateLogger<ScrapeAmazonFunction>();
    }

    [Function(nameof(ScrapeAmazonFunction))]
    public async Task Run(// Run every minute for testing
        [TimerTrigger("0 * * * * *")] TimerInfo timer
    )
    {
        _logger.LogInformation($"C# Timer trigger function executed at {DateTime.UtcNow}.");
        _logger.LogInformation(
  $"C# Timer trigger function next occurrence at: {timer.ScheduleStatus?.Next}.");

        HttpClient client = _clientFactory.CreateClient("AmazonClient");
        _logger.LogInformation(
          $"Request: GET {client.BaseAddress}{relativePath}");
        HttpResponseMessage response = await client.GetAsync(relativePath);


        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Successful HTTP request.");

            // Read the content from a GZIP stream into a string.
            Stream stream = await response.Content.ReadAsStreamAsync();
            GZipStream gzipStream = new(stream, CompressionMode.Decompress);
            StreamReader reader = new(gzipStream);
            string page = reader.ReadToEnd();

            // Extract the Best Sellers Rank.
            int posBsr = page.IndexOf("Best Sellers Rank");
            string bsrSection = page.Substring(posBsr, 45);

            // bsrSection will be something like:
            //   "Best Sellers Rank: </span> #22,258 in Books ("

            // Get the position of the # and the following space.
            int posHash = bsrSection.IndexOf("#") + 1;
            int posSpaceAfterHash = bsrSection.IndexOf(" ", posHash);

            // Get the BSR number as text.
            string bsr = bsrSection.Substring(
              posHash, posSpaceAfterHash - posHash);

            bsr = bsr.Replace(",", null); // remove commas

            // Parse the text into a number.
            if (int.TryParse(bsr, out int bestSellersRank))
            {
                _logger.LogInformation(
                  $"Best Sellers Rank #{bestSellersRank:N0}.");
            }
            else
            {
                _logger.LogError(
                  $"Failed to extract BSR number from: {bsrSection}.");
            }
        }
        else
        {
            _logger.LogError("Bad HTTP request.");
        }

    }
}
