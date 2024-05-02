using EntityLayer.Concrete;

namespace YaSaS_UserInterface.Models
{
	public class UserDto
	{
		public string? Username { get; set; }
		public string? Password { get; set; }
		public string? ConfirmPassword { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public int BuildingId { get; set; }
	}
}
