using AutoMapper;
using MappingObjects.Mappers;
using Northwind.EntityModels;
using Northwind.ViewModels;
using System.Text;

OutputEncoding = Encoding.UTF8; // to suport unicode characters Euro currency symbol

Cart cart = new(
    Customer: new(
        FirstName: "John",
        LastName: "Doe"
    ),
    Items: new() {
        new(ProductName: "Apples", UnitPrice: 0.49M, Quantity: 10),
        new(ProductName: "Bananas", UnitPrice: 0.99M, Quantity: 4)
    }
);

WriteLine("*** Original data before mapping.");
WriteLine($"{cart.Customer}");
foreach (LineItem item in cart.Items)
{
    WriteLine($"{item}");
}

MapperConfiguration config = CartToSummaryMapper.GetMapperConfiguration();

IMapper mapper = config.CreateMapper();

Summary summary = mapper.Map<Cart, Summary>(cart);

WriteLine("");
WriteLine("*** After mapping");
WriteLine($"Summary: {summary.FullName} spend {summary.Total}");

