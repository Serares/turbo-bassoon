using System.Text.Json.Serialization;
using Northwind.Models;
namespace Northwind.Serialization;

[JsonSerializable(typeof(Product))]
[JsonSerializable(typeof(List<Product>))]
internal partial class NorthwindJsonSerializerContext : JsonSerializerContext { }