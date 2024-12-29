using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimeShop.Models;
using Microsoft.CodeAnalysis.CodeStyle;

namespace AnimeShop.Controllers
{
    public class WishlistsController : Controller
    {
        private readonly AnimeShopContext _context;

        public WishlistsController(AnimeShopContext context)
        {
            _context = context;
        }

        // GET: Wishlists
        public async Task<IActionResult> Index()
        {
            if (!Request.Cookies.ContainsKey("CustomerId"))
            {
                return RedirectToAction("Login", "Auth");
            }

            int customerId = int.Parse(Request.Cookies["CustomerId"]);
            var wishlistItems = await _context.Wishlists
                .Include(w => w.Product)
                .Where(w => w.CustomerId == customerId)
                .ToListAsync();

            return View(wishlistItems);
        }

        // GET: Wishlists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleWishlist(int productId)
        {
            if (!Request.Cookies.ContainsKey("CustomerId"))
            {
                return RedirectToAction("Login", "Auth");
            }

            int customerId = int.Parse(Request.Cookies["CustomerId"]);
            var wishlistItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.CustomerId == customerId && w.ProductId == productId);

            if (wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newWishlistItem = new Wishlist
                {
                    WishlistId = int.Parse(customerId.ToString() + productId.ToString()),
                    CustomerId = customerId,
                    ProductId = productId
                };
                _context.Wishlists.Add(newWishlistItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleWishlistFromWishlist(int productId)
        {
            if (!Request.Cookies.ContainsKey("CustomerId"))
            {
                return Unauthorized();
            }

            int customerId = int.Parse(Request.Cookies["CustomerId"]);
            var wishlistItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.CustomerId == customerId && w.ProductId == productId);

            if (wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // GET: Wishlists/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: Wishlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WishlistId,CustomerId,ProductId")] Wishlist wishlist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wishlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", wishlist.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", wishlist.ProductId);
            return View(wishlist);
        }

        // GET: Wishlists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", wishlist.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", wishlist.ProductId);
            return View(wishlist);
        }

        // POST: Wishlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WishlistId,CustomerId,ProductId")] Wishlist wishlist)
        {
            if (id != wishlist.WishlistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishlist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishlistExists(wishlist.WishlistId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", wishlist.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", wishlist.ProductId);
            return View(wishlist);
        }

        // GET: Wishlists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlists
                .Include(w => w.Customer)
                .Include(w => w.Product)
                .FirstOrDefaultAsync(m => m.WishlistId == id);
            if (wishlist == null)
            {
                return NotFound();
            }

            return View(wishlist);
        }

        // POST: Wishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist != null)
            {
                _context.Wishlists.Remove(wishlist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishlistExists(int id)
        {
            return _context.Wishlists.Any(e => e.WishlistId == id);
        }
    }
}
