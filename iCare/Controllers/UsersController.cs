using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;
using System.Linq;

namespace iCare.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        // constructor initializes the database context for interacting with user data
        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // displays a list of all users
        // input: none, output: view with a list of all users
        public IActionResult Index()
        {
            var users = _context.Users.ToList(); // fetch all users from database
            return View(users);
        }

        // displays details of a specific user by id
        // input: user id, output: details page view for the user if found, else not found result
        public IActionResult Details(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // displays the create user form
        // input: none, output: form view for creating a new user
        public IActionResult Create()
        {
            return View();
        }

        // handles form submission to create a new user
        // input: user object with user details, output: redirects to index if successful, else reloads view with errors
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // displays the edit form for a specific user by id
        // input: user id, output: edit page view for the user if found, else not found result
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // handles form submission to update a user's details
        // input: user id and updated user data, output: redirects to index if successful, else reloads view with errors
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, User updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest(); // check for matching ids
            }

            if (ModelState.IsValid)
            {
                _context.Users.Update(updatedUser);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(updatedUser);
        }

        // displays the delete confirmation page for a specific user
        // input: user id, output: delete confirmation view if user found, else not found result
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // handles form submission to delete a user
        // input: user id, output: redirects to index after successful deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
