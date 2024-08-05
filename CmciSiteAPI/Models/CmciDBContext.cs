using Microsoft.EntityFrameworkCore;

namespace CmciSiteAPI.Models
{
    public class CmciDBContext : DbContext
    {
        public CmciDBContext(DbContextOptions<CmciDBContext> options) : base(options)
        {

        }

        public DbSet<Admindb> administrateurs { get; set; }
        public DbSet<Booksdb> Books { get; set; }
        public DbSet<Eventdb> News { get; set; }

    }
}
