using Microsoft.Extensions.Caching.Memory;
using Northwind.EntityModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMemoryCache>(new MemoryCache(
    new MemoryCacheOptions
    {
        TrackStatistics = true,
        SizeLimit = 50
    }
));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddResponseCaching();
// Add services to the container.
builder.Services.AddNorthwindContext();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
