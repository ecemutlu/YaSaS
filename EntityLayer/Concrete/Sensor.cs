using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
	public class Sensor
	{
		[Key]
        public int Id { get; set; }
		public int BuildingId { get; set; }
		public Building? Building { get; set; }
		public string? PlaceDescription { get; set; }
    }
}
