using itec_mobile_api_final.Entities;
using Microsoft.EntityFrameworkCore;

namespace itec_mobile_api_final.Data
{
    public class ExampleContext : DbContext
    {
        public ExampleContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<ExampleEntity> Examples { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}