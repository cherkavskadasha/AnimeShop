using Microsoft.AspNetCore.Mvc;
using AnimeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeShop.Controllers
{
    public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _authService.ValidateUserAsync(model.Email, model.Password);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return View(model);
        }

        var authToken = _authService.GenerateJwtToken(user);
        Response.Cookies.Append("AuthToken", authToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return RedirectToAction("Dashboard", "Customer");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid || model.Password != model.ConfirmPassword)
        {
            ModelState.AddModelError(string.Empty, "Passwords do not match.");
            return View(model);
        }

        var user = await _authService.RegisterUserAsync(model);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Registration failed.");
            return View(model);
        }

        var authToken = _authService.GenerateJwtToken(user);
        Response.Cookies.Append("AuthToken", authToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return RedirectToAction("Dashboard", "Customer");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        return RedirectToAction("Login", "Auth");
    }
}
}
