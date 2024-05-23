using System.Security.Policy;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UserInterface.Models;
using UserInterface.Services;

namespace UserInterface.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BuildingController : Controller
    {
		private readonly mydbContext _context;
		private readonly CityTownService _cityTownService;

		// Constructor: CityTownService bağımlılığını ekliyoruz
		public BuildingController(mydbContext context, CityTownService cityTownService)
		{
			_context = context;
			_cityTownService = cityTownService;
		}
		
		//[HttpGet]
		//public IActionResult AddBuilding()
		//{
		//	return View();
		//}
		
		// GET: /Admin/Building/AddBuilding
		// Bina ekleme sayfasını gösteren action metodu
		[HttpGet]
		public async Task<IActionResult> AddBuilding()
		{
			// BuildingDto modelini oluşturuyoruz
			var model = new BuildingDto();

			try
			{
				// Şehirlerin listesini CityTownService servisi aracılığıyla alıyoruz
				model.CityList = await _cityTownService.GetCities();

				// İlçelerin listesini CityTownService servisi aracılığıyla alıyoruz
				// İlk olarak boş bir liste gönderiyoruz, ilçe bilgilerini şehir seçildiğinde AJAX ile alacağız
				model.TownList = new List<SelectListItem>();
			}
			catch (Exception)
			{
				// Servis çağrısı sırasında bir hata oluşursa, hata yakalanacak ve gerekli işlemler yapılacak
				// Örneğin, loglama yapılabilir veya kullanıcıya hata mesajı gösterilebilir
				ModelState.AddModelError(string.Empty, "Şehir ve ilçe bilgileri alınırken bir hata oluştu.");
				// Hata durumunda kullanıcıya view'i göstermek için model'i boş olarak geri döndürüyoruz
				return View(model);
			}

			// Modeli view'e gönderiyoruz
			return View(model);
		}

		// POST: /Admin/Building/AddBuilding
		// Bina ekleme işlemini yapan action metodu
		[HttpPost]
		public async Task<IActionResult> AddBuilding([Bind("Name,NumberOfFloor,CityId,TownId,Address")] BuildingDto building)
		{
			if (ModelState.IsValid)
			{
				// Bina modelini veritabanına ekliyoruz
				_context.Building.Add(new Building
				{
					Name = building.Name,
					NumofFloor = building.NumofFloor,
					CityId = building.CityId,
					TownId = building.TownId,
					Address = building.Address
				});
				await _context.SaveChangesAsync();

				return RedirectToAction("Index", "Home"); // Örnek bir yönlendirme
			}

			// Şehir ve ilçe bilgilerini tekrar yüklemek için
			building.CityList = await _cityTownService.GetCities();
			building.TownList = new List<SelectListItem>();

			// ModelState.IsValid false ise, tekrar AddBuilding view'ını göster
			return View(building);
		}

		// GET: /Admin/Building/GetTowns
		// Şehir seçildiğinde ilçeleri döndüren action metodu
		[HttpGet]
		public async Task<IActionResult> GetTowns(int cityId)
		{
			// Seçilen şehir ID'sine göre ilçe listesini alıyoruz
			var towns = await _cityTownService.GetTowns(cityId);
			return Json(towns); // JSON formatında ilçe listesini döndürüyoruz
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
