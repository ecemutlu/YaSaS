using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
	public class Building
	{
		[Key]
		public int Id { get; set; }
		public string? Name { get; set; }
		public int NumofFloor { get; set; }
		[ForeignKey("City")]
		public int CityId { get; set; }
		[ForeignKey("Town")]
		public int TownId { get; set; }
		public string? Address { get; set; }
		public float Longitude { get; set; }
		public float Latitude { get; set; }
		public float MaxX { get; set; }
		public float MaxY { get; set; }
		public float MaxZ { get; set; }
	}
}
