using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YaSaS_UserInterface.Models;

namespace YaSaS_UserInterface.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
				
            };

            if (p.Password == p.ConfirmPassword)
            {
                var result = await _userManager.CreateAsync(appUser, p.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("AddBuildingInfo", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserDto p)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(p.Username, p.Password, false, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            return View();
        }
    }
}
