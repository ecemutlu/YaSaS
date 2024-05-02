using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using DataAccessLayer.Context;
using Microsoft.Extensions.Configuration;

namespace YaSaS.Areas.Admin.Data
{
    public class mydbContextFactory : IDesignTimeDbContextFactory<mydbContext>
    {
        private readonly IConfiguration? _configuration;
        public mydbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<mydbContext>();
            optionsBuilder.UseSqlServer(_configuration?.GetConnectionString("conn"));

            return new mydbContext(optionsBuilder.Options);
        }
    }
}
