using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace UserInterface.Models
{
	public class CustomUser:IdentityUser
	{
		[ForeignKey("Building")]
        public int BuildingId { get; set; }
    }
}
