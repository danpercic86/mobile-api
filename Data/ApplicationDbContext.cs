using System.Drawing;
using itec_mobile_api_final.Entities;
using itec_mobile_api_final.Sockets;
using itec_mobile_api_final.Stations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<StationEntity> StationEntities { get; set; }
        public DbSet<SocketsEntity> SocketsEntities { get; set; }
        
        public IRepository<T> GetRepository<T>() where T: Entity
        {
            return new Repository<T>(this);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<SocketsEntity>()
                .HasOne(s => s.Station)
                .WithMany(s => s.Sockets)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<StationEntity>()
                .Property(e => e.Location).HasConversion(
                    v => JsonConvert.SerializeObject(v/*, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }*/),
                    v => JsonConvert.DeserializeObject<PointF>(v/*, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }*/));
        }
    }
}