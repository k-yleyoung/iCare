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

        // constructor takes the app database context as a parameter
        // allows access to the users table for checking login and registration
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // displays the login view (input: none, output: login page view)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // handles the login form submission
        // input: username and password from login form
        // output: redirects to the home page if login is successful, else reloads login page with errors
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // find the user by username from the database
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            // if user is not found, return error message
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username.");
                return View();
            }

            // if password does not match, return error message
            if (user.Password != password)
            {
                ModelState.AddModelError("", "Invalid password.");
                return View();
            }

            // create claims for the logged-in user, including username and role
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // unique user identifier
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // create an identity based on the claims for cookie authentication
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // sign in the user using cookie authentication and set up a principal for them
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // redirect to the home page after successful login
            return RedirectToAction("Index", "Home");
        }

        // displays the registration view (input: none, output: registration page view)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // handles the registration form submission
        // input: user object containing username, password, etc.
        // output: redirects to login page if registration is successful, else reloads registration page with errors
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // check if the username is already in use
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View();
                }

                // add new user to the database and save changes
                _context.Users.Add(user);
                _context.SaveChanges();

                // redirect to the login page after successful registration
                return RedirectToAction("Login");
            }

            // if the form is not valid, reload registration view with the entered data and errors
            return View(user);
        }

        // handles logout process (input: none, output: redirects to login page)
        public async Task<IActionResult> Logout()
        {
            // sign out the user, clearing their cookie-based authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // redirect back to the login page after logout
            return RedirectToAction("Login");
        }
    }
}
