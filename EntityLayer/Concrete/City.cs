using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EntityLayer.Concrete
{
	public class City
	{
		[Key]
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		//[JsonProperty("population")]
		//public int Population { get; set; }

		//[JsonProperty("area")]
		//public int Area { get; set; }

		//[JsonProperty("altitude")]
		//public int Altitude { get; set; }

		//[JsonProperty("areaCode")]
		//public List<int> AreaCode { get; set; }

		//[JsonProperty("isMetropolitan")]
		//public bool IsMetropolitan { get; set; }

		//[JsonProperty("nuts")]
		//public Nuts Nuts { get; set; }

		//[JsonProperty("coordinates")]
		//public Coordinates Coordinates { get; set; }

		//[JsonProperty("maps")]
		//public Maps Maps { get; set; }

		//[JsonProperty("region")]
		//public Region Region { get; set; }

		//[JsonProperty("districts")]
		//public List<Town> Districts { get; set; }
	}

	//public class Nuts
	//{
	//	[JsonProperty("nuts1")]
	//	public Nuts1 Nuts1 { get; set; }

	//	[JsonProperty("nuts2")]
	//	public Nuts2 Nuts2 { get; set; }

	//	[JsonProperty("nuts3")]
	//	public string Nuts3 { get; set; }
	//}

	//public class Nuts1
	//{
	//	[JsonProperty("code")]
	//	public string Code { get; set; }

	//	[JsonProperty("name")]
	//	public Dictionary<string, string> Name { get; set; }
	//}

	//public class Nuts2
	//{
	//	[JsonProperty("code")]
	//	public string Code { get; set; }

	//	[JsonProperty("name")]
	//	public string Name { get; set; }  // Bu alanı Dictionary yerine string yapıyoruz
	//}

	//public class Coordinates
	//{
	//	[JsonProperty("latitude")]
	//	public double Latitude { get; set; }

	//	[JsonProperty("longitude")]
	//	public double Longitude { get; set; }
	//}

	//public class Maps
	//{
	//	[JsonProperty("googleMaps")]
	//	public string GoogleMaps { get; set; }

	//	[JsonProperty("openStreetMap")]
	//	public string OpenStreetMap { get; set; }
	//}

	//public class Region
	//{
	//	[JsonProperty("en")]
	//	public string En { get; set; }

	//	[JsonProperty("tr")]
	//	public string Tr { get; set; }
	//}
}
