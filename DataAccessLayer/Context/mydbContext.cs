using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.Context
{
    public class mydbContext : IdentityDbContext<User, Role, int>
    {

        public mydbContext(DbContextOptions<mydbContext> options)
        : base(options)
        {
        }
        public DbSet<Building> Building { get; set; }
        public DbSet<Sensor> Sensor { get; set; }
        public DbSet<Data> Data { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Town> Town { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Data>().HasKey(s => new { s.SensorId, s.TimeStamp });
            base.OnModelCreating(modelBuilder);
        }

    }
}
