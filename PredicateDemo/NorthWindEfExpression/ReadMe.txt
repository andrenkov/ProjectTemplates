https://learn.microsoft.com/en-us/visualstudio/data-tools/create-a-simple-data-application-with-wpf-and-entity-framework-6?view=vs-2022
https://www.c-sharpcorner.com/article/entity-framework-core-6-with-database-first/

1. New .Core project
2. Add NuGet packages:
	EntityFramework
	Microsoft.EntityFrameworkCore.Design
	Microsoft.EntityFrameworkCore.SqlServer
	Microsoft.EntityFrameworkCore.Tools
3. Scaffold the Existing Database. This step is needed to create the entities classes and the DbContext class in your Console Application. 
	Execute the following script on your Package Manager Console. Or use "EF Core Power Tools" for reverse engineering.
	Scaffold-DbContext "Data Source=.;Initial Catalog=Northwind;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
	* NorthwindContext.cs is also added into the Models. Move it to Data if needed.
4. If you need to change the database based on code changes then you can use the Migration Commands as if we were using the Code First approach.
5. Connect to the database:
	- Add Microsoft.Extensions.Configuration
	- Add Microsoft.Extensions.Configuration.Json
	- In program, add IConfiguration config ... and DbContextOptions options = new ...
	- Pass option to DbContext.Create using (var db = new NorthwindContext(options))
	- The above will pass the "options" into the Base constructor to init teh connection.
	- Update OnConfiguring() with  "if (!optionsBuilder.IsConfigured)" to re-init if not IsConfigured.
