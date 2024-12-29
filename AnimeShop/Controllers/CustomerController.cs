using Microsoft.AspNetCore.Mvc;
using AnimeShop.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace AnimeShop.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AnimeShopContext _context;

        public CustomerController(AnimeShopContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            if (!Request.Cookies.ContainsKey("CustomerId"))
            {
                return RedirectToAction("Login", "Auth");
            }

            int customerId = int.Parse(Request.Cookies["CustomerId"]);
            var customer = _context.Customers
         .Include(c => c.Address)
         .Include(c => c.Payment)
         .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        [HttpGet]
        public IActionResult Edit()
        {
            if (!Request.Cookies.ContainsKey("CustomerId"))
            {
                return RedirectToAction("Login", "Auth");
            }

            int customerId = int.Parse(Request.Cookies["CustomerId"]);
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer updatedCustomer)
        {
            if (!Request.Cookies.ContainsKey("CustomerId"))
            {
                return RedirectToAction("Login", "Auth");
            }

            int customerId = int.Parse(Request.Cookies["CustomerId"]);
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return NotFound();
            }

            customer.Email = updatedCustomer.Email;
            customer.Phone = updatedCustomer.Phone;
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }
}
