using System.Text.Json.Serialization;
using Northwind.Serialization;
using WebApi.Extensions;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, NorthwindJsonSerializerContext.Default);
});

var app = builder.Build();
app.MapGets();

app.Run();