﻿using AnimeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AnimeShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;

namespace AnimeShop.Controllers
{
    public class CartController : Controller
{
    private readonly AnimeShopContext _context;

    public CartController(AnimeShopContext context)
    {
        _context = context;
    }

    public IActionResult AddToCart(int productId, int quantity)
    {
        var customerId = Request.Cookies["CustomerId"];
        if (string.IsNullOrEmpty(customerId))
        {
            return RedirectToAction("Login", "Auth");
        }
        var userId = int.Parse(customerId);

        var order = _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefault(o => o.CustomerId == userId && o.Status == "нове");

        if (order == null)
        {
            order = new Order
            {
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                CustomerId = userId,
                Status = "нове"
            };
            _context.Orders.Add(order);
            _context.SaveChanges(); // Save changes to generate OrderId
        }

        var product = _context.Products.Find(productId);

        if (product == null)
        {
            return RedirectToAction("Index", "Products");
        }

        var existingOrderItem = order.OrderItems
            .FirstOrDefault(oi => oi.ProductId == productId);

        if (existingOrderItem == null)
        {
            var orderItem = new OrderItem
            {
                OrderItemId = int.Parse($"{order.OrdersId}{productId}"), 
                OrdersId = order.OrdersId,
                ProductId = productId,
                Quantity = quantity,
                PricePerUnit = product.Price
            };
            _context.OrderItems.Add(orderItem);
        }
        else
        {
            existingOrderItem.Quantity += quantity;
            if (existingOrderItem.PricePerUnit != product.Price)
            {
                existingOrderItem.PricePerUnit = product.Price;
            }
        }

        _context.SaveChanges();

        return RedirectToAction("Index", "Cart");
    }

    public IActionResult Index()
    {
        var customerId = Request.Cookies["CustomerId"];
        if (string.IsNullOrEmpty(customerId))
        {
            return RedirectToAction("Login", "Auth");
        }
        var userId = int.Parse(customerId);

        var order = _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefault(o => o.CustomerId == userId && o.Status == "нове");

        if (order == null || !order.OrderItems.Any())
        {
            return RedirectToAction("Index", "Products");
        }

        order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.PricePerUnit);
        _context.SaveChanges();

        return View(order.OrderItems);
    }

    public IActionResult RemoveFromCart(int orderItemId)
    {
        var orderItem = _context.OrderItems.Find(orderItemId);
        var product = _context.Products.Find(orderItem.ProductId);
        if (orderItem != null)
        {
            product.Stock += orderItem.Quantity;
            _context.OrderItems.Remove(orderItem);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
}
