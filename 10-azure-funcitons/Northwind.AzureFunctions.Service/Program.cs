using Microsoft.Extensions.DependencyInjection; // to use addhttpclient
using System.Net.Http.Headers; // to use mediatypewithqualityheadervalue
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Add HttpClient with Amazon base address and Chrome headers
builder.Services.AddHttpClient("AmazonClient", options =>
{
    options.BaseAddress = new Uri("https://www.amazon.com/");
    
    // Pretend to be Chrome with US English.
    options.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("text/html"));
    options.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
    options.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
    options.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("image/avif"));
    options.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("image/webp"));
    options.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("image/apng"));
    options.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("*/*", 0.8));
    options.DefaultRequestHeaders.AcceptLanguage.Add(
        new StringWithQualityHeaderValue("en-US"));
    options.DefaultRequestHeaders.AcceptLanguage.Add(
        new StringWithQualityHeaderValue("en", 0.8));
    options.DefaultRequestHeaders.UserAgent.Add(
        new ProductInfoHeaderValue("Chrome", "114.0.5735.91"));
});

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
