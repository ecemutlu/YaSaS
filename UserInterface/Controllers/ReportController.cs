using System.Drawing;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserInterface.Models;

public class ReportController : Controller
{
    private readonly mydbContext _context;
    private readonly UserManager<CustomUser> _userManager;

    public ReportController(mydbContext context, UserManager<CustomUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> ListReports()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var requests = await _context.RequestedReport.Where(r => r.UserId == user.Id)
                                     .ToListAsync();

        ViewData["ReportRequests"] = requests;

        return View();
    }
    public async Task<IActionResult> GetReportImage(int id)
    {
        var report = await _context.RequestedReport.FindAsync(id);
        if (report == null || report.ReportUrl == null)
        {
            return NotFound("Image data not found.");
        }
       
        using (var ms = new MemoryStream())
        {           
            report.ReportUrl = ms.ToArray();
        }

        return File(report.ReportUrl, "image/png"); // Adjust MIME type as needed
    }

    public IActionResult RequestReport()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RequestReport([Bind("DateRange,ReportType")] RequestedReport request)
    {
        if (!ModelState.IsValid)
        {
            return View(request); // Return the view with validation errors
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }      
        request.UserId = user.Id;
        request.Status = "In Progress";

        _context.RequestedReport.Add(request);
        await _context.SaveChangesAsync();

        // Redirect to a confirmation page or back to the list of reports
        return RedirectToAction("ListReports");
    }
}
