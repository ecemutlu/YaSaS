using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EntityLayer.Concrete
{
	public class Town
	{
		[Key]
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
		[ForeignKey("City")]
        public int CityId { get; set; }

        //[JsonProperty("population")]
        //public int Population { get; set; }

        //[JsonProperty("area")]
        //public int Area { get; set; }

    }
}
