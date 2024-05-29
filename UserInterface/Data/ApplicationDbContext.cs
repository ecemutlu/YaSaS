using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserInterface.Models;

namespace UserInterface.Data
{
    // Identity ile CustomUser tipini kullanan ApplicationDbContext sınıfı
    public class ApplicationDbContext : IdentityDbContext<CustomUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{			
		}
        // Binaların saklandığı DbSet, diğer sınıflar tarafından erişilebilir olmalı
        public DbSet<Building> Building { get; set; }
        public DbSet<RequestedReport> RequestedReport { get; set; }
    }
}
