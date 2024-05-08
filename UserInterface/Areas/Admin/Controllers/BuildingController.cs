using Microsoft.AspNetCore.Mvc;

namespace UserInterface.Areas.Admin.Controllers
{
    public class BuildingController : Controller
    {
        public IActionResult EditBuilding()
        {
            return View();
        }
    }
}
