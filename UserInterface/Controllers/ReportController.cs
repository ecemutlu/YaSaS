using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserInterface.Models;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterface.Controllers
{
	public class ReportController : Controller
	{
		private readonly mydbContext _context;
		private readonly UserManager<CustomUser> _userManager;

		public ReportController(mydbContext context, UserManager<CustomUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public IActionResult ListReports()
		{
			return View();
		}

		public IActionResult RequestReport()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetReportRequests()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Unauthorized();
			}

			var requests = await _context.RequestedReport
										 .Where(r => r.UserId == user.Id)
										 .ToListAsync();

			return Ok(requests);
		}

		[HttpPost]
		public async Task<IActionResult> RequestReport([Bind("DateRange,ReportType")] RequestedReport request)
		{
			if (!ModelState.IsValid)
			{			
				return View(request); // Return the view with validation errors // Return the view with validation errors
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Unauthorized();
			}

			request.UserId = user.Id;
			request.Status = "Request Sent";

			_context.RequestedReport.Add(request);
			await _context.SaveChangesAsync();


			// Redirect to a confirmation page or back to the list of reports
			return RedirectToAction("ListReports");
		}
	}
}
