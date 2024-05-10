using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserInterface.Areas.Admin.Models;

namespace UserInterface.Areas.Admin.Controllers
{
	[Area("Admin")]

	public class BuildingController : Controller
    {
		private readonly mydbContext _context;

		public BuildingController(mydbContext context)
		{
			_context = context;
		}

        [HttpGet]
        public IActionResult AddBuilding()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> AddBuilding([Bind("Name,NumberOfFloor,CityId,TownId")]BuildingDto building)
		{
			_context.Building.Add(building);
			await _context.SaveChangesAsync();
			return View(building);
		}
	}
}
