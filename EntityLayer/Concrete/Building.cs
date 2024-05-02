using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
		public string? City { get; set; }
		public string? Town { get; set; }
		public string? Address { get; set; }
		public float Longitude { get; set; }
		public float Latitude { get; set; }
		public float MaxX { get; set; }
		public float MaxY { get; set; }
		public float MaxZ { get; set; }
    }
}
