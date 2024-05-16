using Microsoft.EntityFrameworkCore;
using Sistema8QUALY.Models;

namespace Sistema8QUALY.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Contacts> Contacts { get; set; }
    }
}
