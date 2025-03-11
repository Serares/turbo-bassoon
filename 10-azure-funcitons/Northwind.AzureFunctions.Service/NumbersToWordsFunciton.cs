using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Northwind.AzureFunctions.Service
{
    public class NumbersToWordsFunciton
    {
        private readonly ILogger<NumbersToWordsFunciton> _logger;

        public NumbersToWordsFunciton(ILogger<NumbersToWordsFunciton> logger)
        {
            _logger = logger;
        }

        [Function("NumbersToWordsFunciton")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
            HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string? amount = req.Query["amount"];
            HttpResponseData response;
            if (long.TryParse(amount, out long number))
            {
                response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await response.WriteStringAsync(number.ToWords());
            }
            else
            {
                response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await response.WriteStringAsync($"failed to parse: {amount}");
            }
            return response;
        }
    }
}
