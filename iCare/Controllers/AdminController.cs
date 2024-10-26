using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iCare.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        public IActionResult AdminDashboard()
        {
            
            return View();
        }

        public IActionResult ManageUsers()
        {
            
            return View();
        }

        
    }
}
