using Microsoft.AspNetCore.Mvc;
using AnimeShop.Models;

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
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.ValidateUserByEmailAndPassword(model.Email, model.Password);

                if (user != null)
                {
                    Response.Cookies.Append("CustomerId", user.CustomerId.ToString(), new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
                    return RedirectToAction("Dashboard", "Customer");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password");
                }
            }
            return View(model);
        }


        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("CustomerId");
            Response.Cookies.Delete("WishlistItems");

            return RedirectToAction("Login", "Auth");
        }
    }
}
