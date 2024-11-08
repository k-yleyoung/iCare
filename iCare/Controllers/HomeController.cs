using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iCare.Controllers
{
    public class HomeController : Controller
    {
        // displays the home page
        // input: none, output: home page view or redirect to login if user is not authenticated
        [HttpGet]
        public IActionResult Index()
        {
            // check if the user is not authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // if not authenticated, redirect to the login page in the account controller
                return RedirectToAction("Login", "Account");
            }

            // if authenticated, show the home page view
            return View();
        }
    }
}
