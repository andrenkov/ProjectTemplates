using EF_CodeFirst.Data;
using EF_CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using static System.Console;

Console.WriteLine("Starting Code First EF project");


IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

DbContextOptions options = new DbContextOptionsBuilder<School>()
    .UseSqlServer(config.GetConnectionString("SchoolDatabase"))
    .Options;

//create Db with all tables as per Models
//School myShoolDbContext  = new (options);

//or do as below

//delete Db if exists and create with all tables as per Models
//
try
{
    using (School DbCont = new(options))
    {
        //Only if Db from Scratch. Otherwise, EnsureCreatedAsync() will work.
        /*
        WriteLine("Deleting Db");
        bool deleted = await DbCont.Database.EnsureDeletedAsync();
        WriteLine($"Db deleted : {deleted}");
        */
        //Create Db if not created
        WriteLine("Creating Db");
        bool created = await DbCont.Database.EnsureCreatedAsync();
        WriteLine($"Db created : {created}");

        //WriteLine($"Sql script used : {DbCont.Database.GenerateCreateScript()}");

        //Apply pending migrations
        DbCont.Database.Migrate();

        //just to test
        foreach (Student s in DbCont.Students.Include(s => s.Courses))
        {
            WriteLine("{0} {1} attends the following {2} courses:", s.FirstName, s.LastName, s.Courses.Count);
            foreach (Course c in s.Courses)
            {
                WriteLine($"   {c.Title}");
            }
        }
    }
}
catch (Exception ex)
{
    WriteLine($"Error : {ex.Message}");
}

//To force OnConfiguring() execution
//myShoolDbContext.ConfigureAwait(false);

//Apply pending migrations
//myShoolDbContext.Database.Migrate();

//Add new record test
try
{
    using (School DbCont = new(options))
    {
        WriteLine("Adding Irina");
        DbCont.Students.Add(new Student() { FirstName = "Irina", LastName = "Lomakina" });
        DbCont.SaveChanges();
    }
}
catch (Exception ex)
{
    WriteLine($"Error : {ex.Message}");
}
