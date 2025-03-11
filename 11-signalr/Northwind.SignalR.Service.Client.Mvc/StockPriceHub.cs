using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices; // to use [EnumeratorCancellation]
using Northwind.SignalR.Streams;

namespace Northwind.SignalR.Service.Hubs;

public class StockPriceHub : Hub
{
    public async IAsyncEnumerable<StockPrice> GetStockPriceUpdates(
        string stock,
        [EnumeratorCancellation] CancellationToken cancellationToken
        )
    {
        double currentPrice = 267.10; // initial price
        for (int i = 0; i < 10; i++)
        {
            // check cancellation token regularly
            // to stop the server producing items if the client disconnects.
            cancellationToken.ThrowIfCancellationRequested();

            currentPrice += Random.Shared.NextDouble() * 10.0 - 5.0;

            StockPrice stockPrice = new(stock, currentPrice);


            Console.WriteLine("[{0}] {1} at {2:C}",
  DateTime.UtcNow, stockPrice.Stock, stockPrice.Price);

            yield return stockPrice;

            await Task.Delay(4000, cancellationToken); // miliseconds
        }

    }

    public async Task UploadStocks(IAsyncEnumerable<string> stocks)
    {
        await foreach (string stock in stocks)
        {
            Console.WriteLine($"Uploading {stock}...");
        }
    }
}