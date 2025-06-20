using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApp.sevices
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> products { get; set; }
    }
}
