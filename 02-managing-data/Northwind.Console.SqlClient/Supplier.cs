namespace Northwind.Models;

public class Supplier {
    public int SupplierId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Country { get; set; }
    
}
