using System.IO;
using itec_mobile_api_final.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace itec_mobile_api_final.Helpers
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
                //var connectionString = configuration.GetConnectionString("DefaultConnection");
            var connectionString = "server=localhost;port=3306;database=mobile_api_1;uid=root;password=parola01;";
            builder.UseMySql(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
    }
}