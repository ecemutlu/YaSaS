using System.Text.Encodings.Web;
using System.Text;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using UserInterface.Models;
using Microsoft.AspNetCore.Authorization;

namespace UserInterface.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        //private readonly IUserEmailStore<IdentityUser> _emailStore;
        //private readonly ILogger<AuthController> _logger;
        //private readonly IEmailSender<IdentityUser> _emailSender;

        //UserManager<UserAccount> userManager,
        //IUserStore<UserAccount> userStore,
        //SignInManager<UserAccount> signInManager,
        //ILogger<RegisterModel> logger,
        //IEmailSender emailSender

        public AuthController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            //_emailStore = emailStore;
            //_emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserDto p)
        {

            User appUser = new User()
            {
                Email = p.Email,
                UserName = p.Username,
                //Building = p.Building
            };

            //if (p.Password == p.ConfirmPassword)
            //{
            //    var result = await _userManager.CreateAsync(appUser, p.Password);
            //    if (result.Succeeded)
            //    {
            //        return RedirectToAction("AddBuildingInfo", "Home");
            //    }
            //    else
            //    {
            //        foreach (var item in result.Errors)
            //        {
            //            ModelState.AddModelError("", item.Description);
            //        }
            //    }
            //}
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Login(UserDto p)
        //{
        //	if (ModelState.IsValid)
        //	{
        //		var result = await _signInManager.PasswordSignInAsync(p.Username, p.Password, false, true);
        //		if (result.Succeeded)
        //		{
        //			return RedirectToAction("Index", "Home");
        //		}
        //		else
        //		{
        //			return RedirectToAction("Login", "Auth");
        //		}
        //	}
        //	return View();
        //}        
        
        public IActionResult Logout()
        {
            // Call the logout method from your Razor Page
            return RedirectToPage("/Identity/Account/Logout");
        }
    }
}
