using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
	public class CityResponse
	{
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("data")]
		public List<City> Data { get; set; }
	}
}
