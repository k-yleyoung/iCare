using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using iCare.Data;
using iCare.Models;

namespace iCare.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // Display Login View
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Handle Login Form Submission
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                // Redirect based on user role
                return user.Role switch
                {
                    "doctor" => RedirectToAction("DoctorDashboard", "Doctor"),
                    "nurse" => RedirectToAction("NurseDashboard", "Nurse"),
                    "admin" => RedirectToAction("AdminDashboard", "Admin"),
                    _ => RedirectToAction("Index", "Home") // Fallback in case of an unknown role
                };
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        // Display Registration View
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Handle Registration Form Submission
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // Handle Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Optional: Database connection test
        public IActionResult TestDatabaseConnection()
        {
            try
            {
                bool isConnected = _context.Database.CanConnect();
                return Content(isConnected ? "Database connection successful." : "Failed to connect to the database.");
            }
            catch (Exception ex)
            {
                return Content($"Database connection error: {ex.Message}");
            }
        }
    }
}
