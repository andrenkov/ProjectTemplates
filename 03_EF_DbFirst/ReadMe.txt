https://www.entityframeworktutorial.net/entityframework6/introduction.aspx
https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx
https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx
https://www.c-sharpcorner.com/article/using-entity-framework-core/

Entity Framework Core supports Database-First approach via the Scaffold-DbContext command of Package Manager Console. 
This command scaffolds a DbContext and entity type classes for a specified database.

1. Install
	Microsoft.EntityFrameworkCore
	Microsoft.EntityFrameworkCore.Sqlserver
	Microsoft.EntityFrameworkCore.Tools

	+ (for passing DbCotext to the constructor from appsettings.json)
	Microsoft.Extensions.Configuration
	Microsoft.Extensions.Configuration.Json

	+ optional:
	#Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
2. Add into appsettings.json:
	"ConnectionStrings": {
		"AcademyDatabase": "Server=DevCenter;Database=NewSchoolDb;Uid=sa;Pwd=B0ba1964;" providerName="System.Data.SqlClient"
	}
	Set appsettings.json "always copy" or "Copy if newer".
3. Add new class for DbContext. For example, Northwind.cs. Better to ../Context folder
4. Add OnConfig:
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
######################################################
Models:
######################################################
We need to do reverse engineering using the Scaffold-DbContext command:
Scaffold-DbContext [-Connection] [-Provider] [-OutputDir] [-Context] [-Schemas>] [-Tables>] 
                    [-DataAnnotations] [-Force] [-Project] [-StartupProject] [<CommonParameters>]
PM> Scaffold-DbContext "Server=DevCenter;Database=NewSchoolDb;Uid=sa;Pwd=B0ba1964;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

Good practice is to move the Db Context class to the "Context" folder or other. Leave models in Models folder.
######################################################
Migration
######################################################
Add new field into a Model.

For DbFirst, it could be an error: "There is already an object named". 
Add into DbContext constructor(s) Database.EnsureCreated(); didn't help.

1. Add InitialMigration and delete the context from up() and down() methods.
2. Run update-database
3. Change Models as required.
4. Add NewMigration like "add-migration RatingAdded -context NewSchoolDBContext"
5. Run Update-Database -context NewSchoolDBContext 20221011143728_AddRatingMigration


