using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace YaSaS_UserInterface.Data
{
	public class ApplicationDbContext:IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		   : base(options)
		{
		}
	}
}
