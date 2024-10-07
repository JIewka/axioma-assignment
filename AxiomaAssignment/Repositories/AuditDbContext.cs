using Microsoft.EntityFrameworkCore;

namespace AxiomaAssignment.Repositories
{
    public class AuditDbContext : DbContext
    {
        public DbSet<Audit> Audits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var dbPath = Path.Combine(currentDirectory, "../../../audits.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
