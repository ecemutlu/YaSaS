using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserInterface.Models;
using Microsoft.AspNetCore.Identity; // Identity kütüphanesini ekleyin
using Microsoft.EntityFrameworkCore;
using UserInterface.Data;

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
                var building = await _context.Building.FindAsync(user.BuildingId); 
                if (building != null)
                {
                    ViewBag.BuildingName = building.Name; 
                    ViewBag.NumofFloor = building.NumofFloor; 
                }
                else
                {
                    ViewBag.BuildingName = "Building information couldn't found";
                    ViewBag.NumofFloor = "Floor information couldn't found";
                }
                // En son rapor isteðini al
                var lastReportRequest = await _context.RequestedReport
                    .Where(r => r.UserId == user.Id)
                    .OrderByDescending(r => r.Id) 
                    .Select(r => r.Status) 
                    .FirstOrDefaultAsync();

                ViewBag.PendingRequestStatus = lastReportRequest ?? "No report request found";
            }
            else
            {
                ViewBag.UserEmail = "Couldn't fetch user information";
                ViewBag.BuildingName = "Building information couldn't found";
                ViewBag.NumofFloor = "Floor information couldn't found";
                ViewBag.PendingRequestStatus = "Report request doesn't found";
            }
            return View();
        }

        public IActionResult Privacy()
		{
			return View();
		}
        public IActionResult DateTest()
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
