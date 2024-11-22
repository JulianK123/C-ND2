using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=user;database=nd2;trusted_connection=true;Encrypt=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Euler> Eulers { get; set; }
    }
}
