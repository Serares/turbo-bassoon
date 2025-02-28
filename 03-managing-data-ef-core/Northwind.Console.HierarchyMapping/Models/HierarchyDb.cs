using Microsoft.EntityFrameworkCore;

namespace Northwind.Models;

public class HierarchyDb : DbContext
{
    public DbSet<Person>? People { get; set; }
    public DbSet<Student>? Students { get; set; }
    public DbSet<Employee>? Employees { get; set; }
    public HierarchyDb(DbContextOptions<HierarchyDb> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Person>().UseTphMappingStrategy();
        // modelBuilder.Entity<Person>().UseTptMappingStrategy();
        modelBuilder.Entity<Person>()
        .UseTpcMappingStrategy()
        .Property(person => person.Id)
        .HasDefaultValueSql("NEXT VALUE FOR [PersonIds]");

        modelBuilder.HasSequence<int>("PersonIds", builder =>
        {
            builder.StartsAt(5);
        });

        Student p1 = new() { Id = 1, Name = "John", Subject = "Math" };
        Student p2 = new() { Id = 2, Name = "Jane", Subject = "Science" };
        Employee p3 = new() { Id = 3, Name = "Jim", HireDate = new DateTime(2020, 1, 1) };
        Employee p4 = new() { Id = 4, Name = "Jill", HireDate = new(year: 2024, month: 1, day: 26) };

        // modelBuilder.Entity<Person>().HasData(p1, p2, p3, p4);
        modelBuilder.Entity<Student>().HasData(p1, p2);
        modelBuilder.Entity<Employee>().HasData(p3, p4);
    }
}
