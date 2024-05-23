using EntityLayer.Concrete;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UserInterface.Areas.Admin.Models
{
	public class BuildingDto:Building
	{
		public BuildingDto()
		{
			CityList = new List<SelectListItem>();
			TownList = new List<SelectListItem>();
		}
		// Bu şekilde, BuildingDto modeli artık city ve town bilgilerini tutacak property'lere sahip oldu.
		public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> TownList { get; set; }
    }
}
