using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAMarineAndBoatSupplies.Models.AdminViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SAMarineAndBoatSupplies.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AdminViewModel adminViewModel)
        {
            if (!ModelState.IsValid)
                return View(adminViewModel);

            var user = await _userManager.FindByNameAsync(adminViewModel.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, adminViewModel.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Username or password not found");
            return View(adminViewModel);
        }

        /*
        public IActionResult Register()
        {
            return View(new AdminViewModel());
        }

        
        [HttpPost]
        public async Task<IActionResult> Register(AdminViewModel adminViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = adminViewModel.UserName };
                var result = await _userManager.CreateAsync(user, adminViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(adminViewModel);
        }
        */

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
