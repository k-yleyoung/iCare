using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using iCare.Data;
using iCare.Models;
using System.Threading.Tasks;
using System.Linq;

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
            // Find the user by username
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            // Check if the user was found
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username.");
                return View();
            }

            // Check if the password is incorrect
            if (user.Password != password)
            {
                ModelState.AddModelError("", "Invalid password.");
                return View();
            }

            // Create authentication claims for a successful login
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Add NameIdentifier claim
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // Redirect to the home page after successful login
            return RedirectToAction("Index", "Home");
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
                // Check if the username already exists
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View();
                }

                // Add user to the database
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
    }
}
