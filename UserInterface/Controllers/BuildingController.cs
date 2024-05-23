using System.Security.Policy;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace UserInterface.Controllers
{    
    [Authorize(Roles = "Admin")]
    public class BuildingController : Controller
    {
        private readonly mydbContext _context;
        private readonly HttpClient _httpClient;

        public BuildingController(mydbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult AddBuilding()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> AddBuilding([Bind("Name,NumberOfFloor,CityId,TownId")] BuildingDto building)
        //{
        //	HttpResponseMessage response = await _httpClient.GetAsync("https://turkiyeapi.dev/api/v1/provinces");

        //	//// Check if the request was successful
        //	//if (response.IsSuccessStatusCode)
        //	//{
        //	//	// Read the response content as string
        //	//	string apiResponse = await response.Content.ReadAsStringAsync();
        //	//	// Parse the response JSON or process the response data as needed
        //	//	// Example: Deserialize JSON response to a model
        //	//	var data = JsonConvert.DeserializeObject<City>(apiResponse);
        //	//	ViewData["City"]=data;

        //		_context.Building.Add(building);
        //		await _context.SaveChangesAsync();
        //		return View(building);
        //	}
    }
}
