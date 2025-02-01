using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimeShop.Models;

namespace AnimeShop.Controllers
{
   public class AddressesController : Controller
{
    private readonly AnimeShopContext _context;

    public AddressesController(AnimeShopContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Addresses.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        var address = await GetAddressByIdAsync(id);
        if (address == null)
        {
            return NotFound();
        }

        return View(address);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(nameof(Address.AddressId), nameof(Address.Street), nameof(Address.ZipCode))] Address address)
    {
        if (!ModelState.IsValid)
        {
            return View(address);
        }

        _context.Add(address);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        var address = await GetAddressByIdAsync(id);
        if (address == null)
        {
            return NotFound();
        }

        return View(address);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(nameof(Address.AddressId), nameof(Address.Street), nameof(Address.ZipCode))] Address address)
    {
        if (id != address.AddressId)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(address);
        }

        try
        {
            _context.Update(address);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AddressExists(address.AddressId))
            {
                return NotFound();
            }
            throw;
        }

        return RedirectToAction(nameof(Index)); // Removed hardcoded redirect
    }

    public async Task<IActionResult> Delete(int? id)
    {
        var address = await GetAddressByIdAsync(id);
        if (address == null)
        {
            return NotFound();
        }

        return View(address);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address == null)
        {
            return NotFound();
        }

        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AddressExists(int id) => _context.Addresses.Any(e => e.AddressId == id);

    private async Task<Address?> GetAddressByIdAsync(int? id)
    {
        if (id == null)
        {
            return null;
        }

        return await _context.Addresses.FirstOrDefaultAsync(m => m.AddressId == id);
    }
}
}
