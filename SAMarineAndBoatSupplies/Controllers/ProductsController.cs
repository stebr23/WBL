using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using SAMarineAndBoatSupplies.Data;
using SAMarineAndBoatSupplies.Models;
using SAMarineAndBoatSupplies.Models.ProdctViewModel;
using System.IO;

namespace SAMarineAndBoatSupplies.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


        // GET: Products
        public async Task<IActionResult> Index(string searchString)
        {
            //IQueryable<string> catQuery = from p in _context.Product select p.Name;

            var products = from p in _context.Product select p;
            var categories = from c in _context.Category select c;
            categories = categories.OrderBy(c => c.Name);

            if (searchString != null)
            {
                products = products.Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString));
            }

            var productCategoryVM = new ProductViewModel
            {
                Products = await products.ToListAsync(),
                Categories = await categories.ToListAsync()
            };

            return View(productCategoryVM);
        }

        public async Task<IActionResult> Filter(int? id)
        {
            var products = from p in _context.Product select p;
            if (id != null)
            {
                products = products.Where(p => p.CategoryId == id);
            }

            var categories = from c in _context.Category select c;
            categories = categories.OrderBy(c => c.Name);

            var pCVM = new ProductViewModel
            {
                Products = await products.ToListAsync(),
                Categories = await categories.ToListAsync()
            };

            return View("Index", pCVM);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                

                //Upload file
                if (HttpContext.Request.Form.Files != null)
                {
                    var files = HttpContext.Request.Form.Files;

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                            var uniqueFilename = Convert.ToString(Guid.NewGuid());
                            var FileExtension = Path.GetExtension(filename);
                            var newFilename = uniqueFilename + FileExtension;

                            product.ImagePath = newFilename;

                            filename = Path.Combine(_hostingEnvironment.WebRootPath, "images") + $@"\{newFilename}";


                            using (FileStream fs = System.IO.File.Create(filename))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                }


                // Add product to db
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index");
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var pVM = new ProductViewModel
            {
                Product = product,
                Categories = await _context.Category.ToListAsync()
            };

            return View(pVM);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (HttpContext.Request.Form.Files != null)
                {
                    var files = HttpContext.Request.Form.Files;

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                            var uniqueFilename = Convert.ToString(Guid.NewGuid());
                            var FileExtension = Path.GetExtension(filename);
                            var newFilename = uniqueFilename + FileExtension;

                            product.ImagePath = newFilename;

                            filename = Path.Combine(_hostingEnvironment.WebRootPath, "images") + $@"\{newFilename}";


                            using (FileStream fs = System.IO.File.Create(filename))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                }

                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);

            var sCI = from s in _context.ShoppingCartItems select s;
            sCI = sCI.Where(s => s.Product.Id == id);

            var sList = sCI.ToList();

            if (sList.Count > 0)
            {
                foreach (var s in sList)
                {
                    _context.ShoppingCartItems.Remove(s);
                }
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
