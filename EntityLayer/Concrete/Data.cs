using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
	public class Data
	{
		[Key]
		public int SensorId { get; set; }
		public Sensor? Sensor { get; set; }
		[Key]
        public DateTime TimeStamp { get; set; }
        public float CoordinateX { get; set; }
        public float CoordinateY { get; set; }
        public float CoordinateZ { get; set; }

    }
}
