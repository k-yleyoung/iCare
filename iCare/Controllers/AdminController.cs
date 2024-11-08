using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iCare.Controllers
{
    // restricts access to this controller to users with the "admin" role
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        // displays the admin dashboard view
        // output: admin dashboard page view
        public IActionResult AdminDashboard()
        {
            return View();
        }

        // displays the manage users view
        // output: manage users page view, where admin can manage users
        public IActionResult ManageUsers()
        {
            return View();
        }
    }
}
