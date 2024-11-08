using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;
using System.Linq;

namespace iCare.Controllers
{
    // This controller handles administrative actions and requires the user to be in the "admin" role
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to inject the AppDbContext dependency
        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Action to display the list of users
        public IActionResult ManageUsers()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // GET: Admin/CreateUser
        // Action to display the form for creating a new user
        public IActionResult CreateUser()
        {
            return View();
        }

        // POST: Admin/CreateUser
        // Action to handle the form submission for creating a new user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(ManageUsers));
            }
            return View(user);
        }

        // GET: Admin/EditUser/5
        // Action to display the form for editing an existing user
        public IActionResult EditUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/EditUser/5
        // Action to handle the form submission for editing an existing user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(ManageUsers));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving changes: " + ex.Message);
                }
            }
            return View(user);
        }

        // GET: Admin/DeleteUser/5
        // Action to display the confirmation page for deleting a user
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/DeleteUser/5
        // Action to handle the form submission for deleting a user
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUserConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(ManageUsers));
        }

        // Action to display the admin dashboard
        public IActionResult Index()
        {
            return View("AdminDashboard");
        }

        // Action to display the system settings page
        public IActionResult SystemSettings()
        {
            // Add logic for system settings
            return View();
        }

        // Action to display the reports page
        public IActionResult ViewReports()
        {
            // Add logic for viewing reports
            return View();
        }
    }
}
