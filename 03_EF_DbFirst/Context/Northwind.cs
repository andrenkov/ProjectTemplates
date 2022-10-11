using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EF_DbFirst.Context
{
    internal class Northwind : DbContext
    {
        /// <summary>
        /// This the Constructor with DI
        /// </summary>
        /// <param name="options"></param>
        public Northwind(DbContextOptions options) : base(options)
        {
        }
    }
}
