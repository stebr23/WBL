using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SAMarineAndBoatSupplies.Data;
using SAMarineAndBoatSupplies.Models;

namespace SAMarineAndBoatSupplies.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View(_context.Welcome.FirstOrDefault(w => w.Id == 1));
        }

        public IActionResult About()
        {

            return View(_context.AboutContact.FirstOrDefault(a => a.Id == 1));
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
