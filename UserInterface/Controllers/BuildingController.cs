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
using Microsoft.Extensions.Logging;



namespace UserInterface.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BuildingController : Controller
    {
		private readonly mydbContext _context;
		private readonly CityTownService _cityTownService;
        private readonly ILogger<BuildingController> _logger;

        // Constructor: CityTownService bağımlılığını ekliyoruz
        public BuildingController(mydbContext context, CityTownService cityTownService, ILogger<BuildingController> logger)
		{
			_context = context;
			_cityTownService = cityTownService;
            _logger = logger;
        }
		
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
        public async Task<IActionResult> AddBuilding([Bind("Name,NumofFloor,CityId,TownId,Address")] BuildingDto building)
        {
            _logger.LogInformation($"Received building data: {JsonConvert.SerializeObject(building)}");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Model state is invalid.");
                return View(building);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // City önceden var mı kontrol et, yoksa API'den çek ve ekle
                    var city = await _context.City.FindAsync(building.CityId);
                    if (city == null)
                    {
                        city = await _cityTownService.GetCityById(building.CityId);
                        if (city == null)
                        {
                            ModelState.AddModelError("", "City cannot be found.");
                            return View(building);
                        }
                        _context.City.Add(city);
                        await _context.SaveChangesAsync();
                    }

                    // Town önceden var mı kontrol et, yoksa API'den çek ve ekle
                    var town = await _context.Town.FindAsync(building.TownId);
                    if (town == null)
                    {
                        town = await _cityTownService.GetTownById(building.TownId);
                        if (town == null || town.CityId != building.CityId)
                        {
                            ModelState.AddModelError("", "Town cannot be found or does not match the city.");
                            return View(building);
                        }
                        _context.Town.Add(town);
                        await _context.SaveChangesAsync();
                    }

                    // Building nesnesini oluştur ve ekle
                    var newBuilding = new Building
                    {
                        Name = building.Name,
                        NumofFloor = building.NumofFloor,
                        CityId = building.CityId,
                        TownId = building.TownId,
                        Address = building.Address
                    };
                    _context.Building.Add(newBuilding);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred: {ex.Message}");
                    transaction.Rollback();
                    ModelState.AddModelError("", "An error occurred while saving the building.");
                    return View(building);
                }
            }
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
    }
}
