using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserInterface.Models;
using Microsoft.AspNetCore.Identity; // Identity k�t�phanesini ekleyin
using System.Threading.Tasks; // Asenkron i�lemler i�in gerekli
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


        // UserManager'� dependency injection ile ekleyin
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
                var building = await _context.Building.FindAsync(user.BuildingId); // Bina bilgisini �ekme
                if (building != null)
                {
                    ViewBag.BuildingName = building.Name; // Bina ad�n� ViewBag'e ata
                    ViewBag.NumofFloor = building.NumofFloor; // Kat say�s� bilgisini �ekme
                }
                else
                {
                    ViewBag.BuildingName = "Bina bilgisi bulunamad�";
                    ViewBag.NumofFloor = "Kat bilgisi bulunamad�";
                }
                // En son rapor iste�ini al
                var lastReportRequest = await _context.RequestedReport
                    .Where(r => r.UserId == user.Id)
                    .OrderByDescending(r => r.Id) // En son istek
                    .Select(r => r.Status) // Sadece status alan�n� al
                    .FirstOrDefaultAsync();

                ViewBag.PendingRequestStatus = lastReportRequest ?? "Rapor iste�i bulunamad�"; // NULL kontrol�
            }
            else
            {
                ViewBag.UserEmail = "Kullan�c� bilgisi �ekilemedi";
                ViewBag.BuildingName = "Bina bilgisi bulunamad�";
                ViewBag.NumofFloor = "Kat bilgisi bulunamad�";
                ViewBag.PendingRequestStatus = "Rapor iste�i bulunamad�";
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
