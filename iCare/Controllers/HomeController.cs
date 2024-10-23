using Microsoft.AspNetCore.Mvc;

namespace iCare.Controllers
{
    public class HomeController : Controller
    {
        // This action method handles GET requests to the root URL.
        public IActionResult Index()
        {
            // Return the view associated with this action method.
            return View();
        }

        // Optionally, you can add more actions if needed, for example:

        // This action method handles GET requests to /Home/About
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        // This action method handles GET requests to /Home/Contact
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        // This action method handles GET requests to /Home/Error
        public IActionResult Error()
        {
            return View();
        }
    }
}
