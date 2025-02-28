using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.EntityModels;

// 💡 good practice to use partial classes 
// so that when using dotnet-ef tool changes won't be overwritten
public partial class Employee : IHasLastRefreshed
{
    [NotMapped] // excluded from database mapping
    public DateTimeOffset LastRefreshed { get; set; }
}
