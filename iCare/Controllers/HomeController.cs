using Microsoft.AspNetCore.Authorization; // Make sure this is imported
using Microsoft.AspNetCore.Mvc;

namespace iCare.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            
            return View();
        }

        

        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        
        public IActionResult Error()
        {
            return View();
        }
    }
}