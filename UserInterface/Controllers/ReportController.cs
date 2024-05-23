using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserInterface.Models;

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
		public async Task<IActionResult> RequestReport([FromBody] RequestedReport request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Unauthorized();
			}

			request.UserId = user.Id;
			request.DateRange = "";
			request.Status = "Request Sent";

			_context.RequestedReport.Add(request);
			await _context.SaveChangesAsync();

			// Simulate report generation
			// In a real-world scenario, you would offload this to a background job
			await Task.Delay(5000); // Simulate report generation delay
			request.Status = "Completed";
			request.ReportUrl = $"/path/to/generated/report/{request.Id}.pdf"; // Mock URL for the PDF
			await _context.SaveChangesAsync();

			return Ok(request);
		}
	}
}

