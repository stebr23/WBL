using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAMarineAndBoatSupplies.Data;
using SAMarineAndBoatSupplies.Models;
using SAMarineAndBoatSupplies.Models.AdminViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SAMarineAndBoatSupplies.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public async Task<IActionResult> Dashboard()
        {
            var orders = from o in _context.Orders select o;
            var categories = from c in _context.Category select c;

            var dashViewModel = new DashboardViewModel {
                Orders = await orders.ToListAsync(),
                Categories = await categories.ToListAsync()
            };

            return View(dashViewModel);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var orderDetail = from od in _context.OrderDetail select od;
            orderDetail = orderDetail.Where(o => o.OrderId == id);

            var order = await _context.Orders.SingleOrDefaultAsync(o => o.OrderId == id);

            var result = from detail in _context.OrderDetail
                         join product in _context.Product on detail.ProductId equals product.Id
                         where detail.OrderId == id
                         select product;
           
            var oVM = new OrderViewModel
            {
                OrderDetails = await orderDetail.ToListAsync(),
                Products = await result.ToListAsync(),
                Order = order
            };

            return View(oVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToActionResult AddCategory(string categoryName)
        {
            _context.Category.Add(new Category { Name = categoryName });
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToActionResult RemoveCategory(int categoryId)
        {
            var category = (from c in _context.Category select c);
            category = category.Where(c => c.CategoryId == categoryId);
            if (category != null)
            {
                _context.Category.RemoveRange(category);
                _context.SaveChanges();
            }
            
            return RedirectToAction("Dashboard");
        }

        public RedirectToActionResult RemoveOrder(int id)
        {
            var orderDetail = from o in _context.OrderDetail select o;
            orderDetail = orderDetail.Where(o => o.OrderId == id);

            var order = from o in _context.Orders select o;
            order = order.Where(o => o.OrderId == id);
            

            if (orderDetail != null && order != null)
            {
                _context.OrderDetail.RemoveRange(orderDetail);
                _context.Orders.RemoveRange(order);
                _context.SaveChanges();
            }


            return RedirectToAction("Dashboard");
        }
    }
}
