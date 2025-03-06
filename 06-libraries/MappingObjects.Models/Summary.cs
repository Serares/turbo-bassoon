using System.Security.Cryptography.X509Certificates;

namespace Northwind.ViewModels;

public record class Summary
{
    // These properties can be initialized once but then never changed
    public string? FullName { get; init; }
    public decimal Total { get; init; }
    // public Summary();
};