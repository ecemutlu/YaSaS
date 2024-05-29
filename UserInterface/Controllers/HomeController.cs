using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserInterface.Models;
using Microsoft.AspNetCore.Identity; // Identity kütüphanesini ekleyin
using System.Threading.Tasks; // Asenkron iþlemler için gerekli
using Microsoft.EntityFrameworkCore;
using UserInterface.Data;
using System;

namespace UserInterface.Controllers
{
	[Authorize]
    public class HomeController : Controller
	{		
		private readonly ILogger<HomeController> _logger;
        // UserManager tipinde bir nesne ekleyin
        private readonly UserManager<CustomUser> _userManager;
        // DbContext enjekte ediliyor
        private readonly ApplicationDbContext _context;


        // UserManager'ý dependency injection ile ekleyin
        // Constructor'a DbContext eklendi
        public HomeController(ILogger<HomeController> logger, UserManager<CustomUser> userManager, ApplicationDbContext context) 
        {
			_logger = logger;
            _userManager = userManager;
            _context = context;

        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.UserEmail = user.Email;
                var building = await _context.Building.FindAsync(user.BuildingId); // Bina bilgisini çekme
                if (building != null)
                {
                    ViewBag.BuildingName = building.Name; // Bina adýný ViewBag'e ata
                    ViewBag.NumofFloor = building.NumofFloor; // Kat sayýsý bilgisini çekme
                }
                else
                {
                    ViewBag.BuildingName = "Bina bilgisi bulunamadý";
                    ViewBag.NumofFloor = "Kat bilgisi bulunamadý";
                }
                // En son rapor isteðini al
                var lastReportRequest = await _context.RequestedReport
                    .Where(r => r.UserId == user.Id)
                    .OrderByDescending(r => r.Id) // En son istek
                    .Select(r => r.Status) // Sadece status alanýný al
                    .FirstOrDefaultAsync();

                ViewBag.PendingRequestStatus = lastReportRequest ?? "Rapor isteði bulunamadý"; // NULL kontrolü
            }
            else
            {
                ViewBag.UserEmail = "Kullanýcý bilgisi çekilemedi";
                ViewBag.BuildingName = "Bina bilgisi bulunamadý";
                ViewBag.NumofFloor = "Kat bilgisi bulunamadý";
                ViewBag.PendingRequestStatus = "Rapor isteði bulunamadý";
            }
            return View();
        }

        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
