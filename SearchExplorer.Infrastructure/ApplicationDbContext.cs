using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace SearchExplorer.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }

}
