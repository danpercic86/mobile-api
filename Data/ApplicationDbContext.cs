using System.Drawing;
using itec_mobile_api_final.Cars;
using itec_mobile_api_final.Entities;
using itec_mobile_api_final.Forum;
using itec_mobile_api_final.Stations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<StationEntity> StationEntities { get; set; }
        public DbSet<CarEntity> CarEntities { get; set; }
        public DbSet<CategoryForumEntity> CategoryForumEntities { get; set;}
        public DbSet<MessageForumEntity> MessageForumEntities { get; set; }
        public DbSet<TopicForumEntity> TopicForumEntities { get; set; }


        public IRepository<T> GetRepository<T>() where T: Entity
        {
            return new Repository<T>(this);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<StationEntity>()
                .Property(e => e.Location).HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<PointF>(v));
            
            base.OnModelCreating(builder);
        }
    }
}