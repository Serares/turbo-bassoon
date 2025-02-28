using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Console.HierarchyMapping;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Create a database context options builder
        DbContextOptionsBuilder<HierarchyDb> optionsBuilder = new();
        SqlConnectionStringBuilder builder = new() {
            DataSource = "tcp:localhost,1433",
            InitialCatalog = "HierarchyMapping",
            UserID = "sa",
            Password = "s3cret-Ninja",
            TrustServerCertificate = true,
            Encrypt = true,
            ConnectTimeout = 30,
            IntegratedSecurity = false, // because we are using a username and password
        };
        
        optionsBuilder.UseSqlServer(builder.ConnectionString);
        using (HierarchyDb db = new(optionsBuilder.Options))
        {
            bool deleted = await db.Database.EnsureDeletedAsync();
            WriteLine($"Database deleted: {deleted}");
            // Ensure database is created
            bool created = await db.Database.EnsureCreatedAsync();
            WriteLine($"Database created: {created}");
            WriteLine("SQL script used to create the database:");
            WriteLine(db.Database.GenerateCreateScript());

            Student newStudent = new()
            {
                Name = "Bob",
                Subject = "Computer Science"
            };
            db.Students?.Add(newStudent);

            Employee newEmployee = new()
            {
                Name = "Alice",
                HireDate = new DateTime(2023, 5, 15)
            };
            db.Employees?.Add(newEmployee);

            // Save changes to the database
            await db.SaveChangesAsync();
            WriteLine($"Added new student: {newStudent.Name} with ID {newStudent.Id}");
            WriteLine($"Added new employee: {newEmployee.Name} with ID {newEmployee.Id}");

            // Query and display all people
            WriteLine("All people:");
            foreach (Person person in db.People.OrderBy(p => p.Id))
            {
                WriteLine($"  {person.Id}: {person.Name}");
                
                if (person is Student student)
                {
                    WriteLine($"    Student studying {student.Subject}");
                }
                else if (person is Employee employee)
                {
                    WriteLine($"    Employee hired on {employee.HireDate:yyyy-MM-dd}");
                }
            }
            
            // Query just students
            WriteLine("\nStudents only:");
            foreach (Student student in db.Students.OrderBy(s => s.Id))
            {
                WriteLine($"  {student.Id}: {student.Name} studies {student.Subject}");
            }
            
            // Query just employees
            WriteLine("\nEmployees only:");
            foreach (Employee employee in db.Employees.OrderBy(e => e.Id))
            {
                WriteLine($"  {employee.Id}: {employee.Name} hired on {employee.HireDate:yyyy-MM-dd}");
            }
        }
    }
}
