using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace iCare.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                // Define the user's claims (e.g., username and role)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                // Create a claims identity
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Redirect based on user role
                return user.Role switch
                {
                    "doctor" => RedirectToAction("DoctorDashboard", "doctor"),
                    "nurse" => RedirectToAction("NurseDashboard", "nurse"),
                    "admin" => RedirectToAction("AdminDashboard", "admin"),
                    _ => RedirectToAction("Index", "Home") // Default redirect if role is unknown
                };
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }


        // Add any other user-related actions, such as editing roles
    }
}