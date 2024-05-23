using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserInterface.Models;

namespace UserInterface.Data
{
	public class ApplicationDbContext : IdentityDbContext<CustomUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{			
		}
		DbSet<Building> Building { get; set; }
	}
}
