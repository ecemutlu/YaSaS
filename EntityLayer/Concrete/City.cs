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
	public class City
	{
		[Key]
        [JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

        // Towns koleksiyonu ekleniyor
		// JSON'dan "districts" olarak alınacak
        [JsonProperty("districts")]
        public virtual ICollection<Town> Towns { get; set; }
    }
}
