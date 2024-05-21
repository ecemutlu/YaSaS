using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UserInterface.Controllers
{
	public class ReportController : Controller
	{
		private readonly mydbContext _context;

		public ReportController(mydbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<RequestedReport>>> ListReports()
		{
			var reports = await _context.RequestedReport.ToListAsync();
			return View(reports);
		}
	}
}
