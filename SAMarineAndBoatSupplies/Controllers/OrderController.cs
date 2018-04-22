using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SAMarineAndBoatSupplies.Data;
using SAMarineAndBoatSupplies.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SAMarineAndBoatSupplies.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCart _shoppingCart;
        private readonly HtmlEncoder _htmlEncoder;

        public OrderController(ApplicationDbContext context, ShoppingCart shoppingCart, HtmlEncoder htmlEncoder)
        {
            _context = context;
            _shoppingCart = shoppingCart;
            _htmlEncoder = htmlEncoder;
        }

        // GET: /<controller>/
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(Order order)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, please add some products");
            }

            if (ModelState.IsValid)
            {
                order.FirstName = _htmlEncoder.Encode(order.FirstName);
                order.LastName = _htmlEncoder.Encode(order.LastName);
                order.AddressLine1 = _htmlEncoder.Encode(order.AddressLine1);
                order.AddressLine2 = order.AddressLine2 == null ? "" : _htmlEncoder.Encode(order.AddressLine2);
                order.City = _htmlEncoder.Encode(order.City);
                order.County = order.County == null ? "" : _htmlEncoder.Encode(order.County);
                order.Email = _htmlEncoder.Encode(order.Email);
                order.PhoneNumber = _htmlEncoder.Encode(order.PhoneNumber);
                order.PostCode = _htmlEncoder.Encode(order.PostCode);

                CreateOrder(order: order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }

            return View(order);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thank you for confirming your order. We will be in touch shortly to finalise payment over the phone.";

            // Display more details about the order?


            return View();
        }

        private void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = _shoppingCart.GetShoppingCartTotal();
            
            _context.Orders.Add(order);

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = shoppingCartItem.Quantity,
                    ProductId = shoppingCartItem.Product.Id,
                    OrderId = order.OrderId,
                    Price = shoppingCartItem.Product.Price
                };

                _context.OrderDetail.Add(orderDetail);
            }

            _context.SaveChanges();
        }
    }
}
