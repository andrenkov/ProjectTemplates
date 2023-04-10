using NorthWindEfExpression.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Connecting to the database!");

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
DbContextOptions options = new DbContextOptionsBuilder<NorthwindContext>()
    .UseSqlServer(config.GetConnectionString("NorthwindConn"))
    .Options;

using (var db = new NorthwindContext(options))
{
    Console.WriteLine("Database Connected");
    Console.WriteLine();
    Console.WriteLine("Listing Category Sales");
    db.Categories.ToList().ForEach(x => Console.WriteLine(x.CategoryName));
    Console.WriteLine();
    Console.WriteLine("Listing Products Above Average Prices");
    db.Products.ToList().ForEach(x => Console.WriteLine(x.ProductName));
    Console.WriteLine();
    Console.WriteLine("Listing Territories");
    db.Territories.ToList().ForEach(x => Console.WriteLine(x.TerritoryDescription));
}