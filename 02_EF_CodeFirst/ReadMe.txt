https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx
https://www.entityframeworktutorial.net/code-first/simple-code-first-example.aspx
https://www.c-sharpcorner.com/article/using-entity-framework-core/

1. Install
	Microsoft.Data.Sqlite.Core or Microsoft.Data.Sqlserver.Core
	#Microsoft.EntityFrameworkCore
	Microsoft.EntityFrameworkCore.Design
	#Microsoft.EntityFrameworkCore.Sqlite.Design or Microsoft.EntityFrameworkCore.Sqlserver.Design
	Microsoft.EntityFrameworkCore.SqlServer
	#Microsoft.EntityFrameworkCore.Tools

	+ (for passing DbCotext to the constructor)
	Microsoft.Extensions.Configuration
	Microsoft.Extensions.Configuration.Json

	+ optional:
	Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
2. Add into appsettings.json:
  "ConnectionStrings": {
    "AcademyDatabase": "Server=DevCneter;Database=AcademyDb; Uid=sa; Pwd=B0ba1964;" providerName="System.Data.SqlClient"
  }
2.1 Set appsettings.json "always copy".
3. Add classes Student and Course into Models. 
4. Add School : DbContext into the Data folder. 
5. Note that Student class is POCO (plain old CLR objects and have no attributes) 
6. In Course class add attributes like [Required]. For this, add using System.ComponentModel.DataAnnotations;  
7. Add public ICollection<Student>? Students { get; set; } and public ICollection<Student>? Students { get; set; } into Course and Student classes 
   to define foreign keys and search optimization. New table CourseStudent will be added automatically.!!!!!!!!
8. Modify the MySchool class and add using Microsoft.EntityFrameworkCore;
	- Make it a DbContect as: public class School : DbContext {}
	- Add two DbSet:
	    public DbSet<Student>? Students { get; set; }
        public DbSet<Course>? Courses { get; set; }
	- Add a constructor with DI:
		public School(DbContextOptions<School> options) : base(options)

		The Injection added in the Program file as:

			.AddJsonFile("appsettings.json")
			.Build();

	- To overwrite the OnConfig (if needed) add the below method:
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	- To seed data or add validation rules, add : 
		protected override void OnModelCreating(ModelBuilder modelBuilder)

9. Migrations. Install Microsoft.EntityFrameworkCore.Tools first
   DbContext must have a default constructor!!!!!!!!!!!!!!
   Create initial migration and delete the database. The migration will include tables and seeded data.


	Add migration (for example, after adding new field toa table)
	PM> 
		add-migration YourMigrationName
	or CLI (might need to exec "dotnet tool update --global dotnet-ef")
		dotnet ef migrations add YourMigrationName

	Updating the Database:
	PM> 
		Update-Database
	or CLI
		dotnet ef database update

If this is the first migration, then it will also create a table called __EFMigrationsHistory

	Removing a Migration:
	PM> 
		remove-migration
	or CLI
		dotnet ef migrations remove

	Reverting a Migration:
	PM> 
		Update-database YourMigrationName
	or CLI
		dotnet ef database update YourMigrationName

