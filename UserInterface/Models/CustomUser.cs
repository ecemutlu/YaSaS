using System.ComponentModel.DataAnnotations.Schema;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;

namespace UserInterface.Models
{
	public class CustomUser:IdentityUser
	{
		[ForeignKey("Building")]
        public int BuildingId { get; set; }


        // Building nesnesine doğrudan erişim sağlayan navigation property
        public virtual Building Building { get; set; }
    }
}
