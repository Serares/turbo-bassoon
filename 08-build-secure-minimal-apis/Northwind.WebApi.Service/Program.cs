using Northwind.EntityModels;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(defaultScheme: "Bearer").AddJwtBearer();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNorthwindContext();
builder.Services.AddCustomHttpLogging();
builder.Services.AddCustomCors();
builder.Services.AddCustomRateLimiting(builder.Configuration);

var app = builder.Build();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpLogging();
await app.UseCustomClientRateLimiting();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseCors(policyName: "Northwind.Mvc.Policy");
app.UseCors();

app.MapGets()
.MapPosts()
.MapPuts()
.MapDeletes();

app.Run();