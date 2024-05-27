using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
	public class Sensor
	{
		[Key]
        public int Id { get; set; }

		[ForeignKey("Building")]
		public int BuildingId { get; set; }
		public string? PlaceDescription { get; set; }
    }
}
