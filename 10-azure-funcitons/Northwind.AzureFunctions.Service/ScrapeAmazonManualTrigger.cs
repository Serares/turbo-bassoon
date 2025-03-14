using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace Northwind.AzureFunctions.Service
{
    public class ScrapeAmazonManualTrigger
    {
        private const string relativePath = "12-NET-Cross-Platform-Development-Fundamentals/dp/1837635870/";
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;

        public ScrapeAmazonManualTrigger(
            IHttpClientFactory clientFactory,
            ILoggerFactory loggerFactory)
        {
            _clientFactory = clientFactory;
            _logger = loggerFactory.CreateLogger<ScrapeAmazonManualTrigger>();
        }

        [Function("ScrapeAmazonManual")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("Manual trigger for Amazon scraping executed.");

            HttpClient client = _clientFactory.CreateClient("AmazonClient");
            HttpResponseMessage response = await client.GetAsync(relativePath);

            _logger.LogInformation(
                $"Request: GET {client.BaseAddress}{relativePath}");

            var functionResponse = req.CreateResponse(HttpStatusCode.OK);

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
                    string result = $"Best Sellers Rank #{bestSellersRank:N0}.";
                    _logger.LogInformation(result);
                    await functionResponse.WriteStringAsync(result);
                }
                else
                {
                    string error = $"Failed to extract BSR number from: {bsrSection}.";
                    _logger.LogError(error);
                    await functionResponse.WriteStringAsync(error);
                }
            }
            else
            {
                string error = "Bad HTTP request.";
                _logger.LogError(error);
                await functionResponse.WriteStringAsync(error);
            }

            return functionResponse;
        }
    }
} 