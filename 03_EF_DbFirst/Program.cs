using EF_DbFirst.Context;
using EF_DbFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using static System.Console;

Console.WriteLine("Starting EF DbFirst");

IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
#region Northwind
/*
DbContextOptions<Northwind> options = new DbContextOptionsBuilder<Northwind>()
    .UseSqlServer(config.GetConnectionString("NorthwindDatabase"))
    .Options;

Northwind DbContNorthwind = new(options);
*/
#endregion

#region NewSchool Db
DbContextOptions<NewSchoolDbContext> options = new DbContextOptionsBuilder<NewSchoolDbContext>()
    .UseSqlServer(config.GetConnectionString("NewSchoolDatabase"))
    .Options;

//Add new record test
try
{
    using (NewSchoolDbContext DbContScool = new(options))
    {
        if (DbContScool.Courses.Any(c => c.Title == "Delphi"))
        {
            Course crs = DbContScool.Courses.Where(c => c.Title == "Delphi").First();
            crs.Rating = rating.Medium;
            WriteLine($"Course updated: {"Delphi"}");
        }
        else
        {
            DbContScool.Courses.Add(new Course() { Title = "Delphi", Rating = rating.High });
            WriteLine($"Course inserted: {"Delphi"}");
        }
        DbContScool.SaveChanges();

        foreach (Course MyCource in DbContScool.Courses)
        {
            WriteLine($"Course : {MyCource.Title}");
        }
    }
}
catch (Exception ex)
{
    WriteLine($"Error : {ex.Message}");
}

#endregion