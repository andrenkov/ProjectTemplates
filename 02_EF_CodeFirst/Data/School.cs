using EF_CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace EF_CodeFirst.Data
{
    public class School : DbContext
    {
        public DbSet<Student>? Students { get; set; }
        public DbSet<Course>? Courses { get; set; }

        /// <summary>
        /// This a default constructor to Migrations to work 
        /// </summary>
        public School() {}
        /// <summary>
        /// This the Constructor with DI
        /// </summary>
        /// <param name="options"></param>
        public School(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// This to override the Db connection OnConfiguring() if needed to create the context manually
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                string connectionString = configuration.GetConnectionString("SchoolDatabase");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        /// <summary>
        /// This method is called when the model for a derived context has been initialized, 
        /// but before the model has been locked down and used to initialize the context.
        /// Typically, this method is called only once when the first instance of a derived context is created.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //add validation rule (if required)
            modelBuilder.Entity<Student>()
                .Property(s => s.LastName).HasMaxLength(30).IsRequired();

            #region seed data
            IList<Student> students = new List<Student>();
            students.Add(new() { StudentId = 1, FirstName = "Vlad", LastName = "A" });
            students.Add(new() { StudentId = 2, FirstName = "Mike", LastName = "B" });
            students.Add(new() { StudentId = 3, FirstName = "Sasha", LastName = "C" });
            modelBuilder.Entity<Student>().HasData(students);

            IList<Course> courses = new List<Course>();
            courses.Add(new() { CourseId = 1, Title = "CSharp" });
            courses.Add(new() { CourseId = 2, Title = "Python" });
            modelBuilder.Entity<Course>().HasData(courses);

            //For crating StudentCourse join table
            //The primary key for the join table is a composite key comprising both of the foreign key values.
            //In addition, both sides of the many-to-many relationship are configured using the HasOne, WithMany and HasForeignKey Fluent API methods
                        modelBuilder.Entity<Course>()
                .HasMany(c => c.Students)
                .WithMany(s => s.Courses)
                .UsingEntity(e => e.HasData(
                    //all students signed up for C# course 
                    new { CoursesCourseId = 1, StudentsStudentId = 1 },
                    new { CoursesCourseId = 1, StudentsStudentId = 2 },
                    new { CoursesCourseId = 1, StudentsStudentId = 3 },
                    //only one student signed up for Python
                    new { CoursesCourseId = 2, StudentsStudentId = 2 }
                    ));

            //The ForeignKey example:
            //https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
            //https://www.entityframeworktutorial.net/code-first/configure-one-to-many-relationship-in-code-first.aspx
            //modelBuilder.Entity<Course>()
            //    .HasOne(c => c.Student)
            //    .WithMany(b => b.Students)
            //    .HasForeignKey(bc => bc.StudentId);
            #endregion
        }
    }
}
