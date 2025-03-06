namespace Northwind.EntityModels;

//Objects will be immutable after instantiation
public record class Customer(
    string FirstName,
    string LastName
);