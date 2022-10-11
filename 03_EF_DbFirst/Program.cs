using EF_DbFirst.Context;
using EF_DbFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        using (IDbContextTransaction trans = DbContScool.Database.BeginTransaction())
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
            trans.Commit();
        }
        int affected = DbContScool.SaveChanges();
        WriteLine($"{affected} row(s) affected ");

        foreach (Course MyCource in DbContScool.Courses)
        {
            WriteLine($"Course : {MyCource.Title}");
        }

        WriteLine("##################################");
        //https://www.entityframeworktutorial.net/querying-entity-graph-in-entity-framework.aspx
        //LinQ example
        //IQueryable<Student> 
        //var students = (from st in DbContScool.Students
        //                join cr in DbContScool.Courses on st.StudentId equals cr.StudentsStudents.First().StudentId
        //                //where cr.StudentsStudents.Equals 
        //                select st.FirstName + " " + st.LastName
        //                ).Take(1000);

        /*
        var students = from st in DbContScool.Students
                           where st.CoursesCourses.Any()
                       select st.FirstName + " " + st.LastName;
        
        foreach (var student in students)
        {
            WriteLine(student);
            //WriteLine(student.FirstName +" "+ student.LastName); for IQueryable
        }

        */

        //IQueryable<Student> newstudents = DbContScool.Students.Where(cr => cr.CoursesCourses.Any());
        IQueryable<Student> newstudents = DbContScool.Students.Where(cr => cr.CoursesCourses.FirstOrDefault().Title == "Delphi");
        foreach (var student in newstudents)
        {
            WriteLine(student.FirstName +" "+ student.LastName);
        }

    }
}
catch (Exception ex)
{
    WriteLine($"Error : {ex.Message}");
}

#endregion