using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iCare.Controllers
{
    // restricts access to this controller to users with the "nurse" role
    [Authorize(Roles = "nurse")]
    public class NurseController : Controller
    {
        // displays the nurse dashboard
        // output: nurse dashboard page view
        public IActionResult NurseDashboard()
        {
            // add logic specific to the nurse dashboard
            return View();
        }

        // displays the manage patient care page
        //output: view for managing patient care logs or viewing necessary information
        public IActionResult ManagePatientCare()
        {
            // logic to allow nurses to manage patient care logs or view necessary information
            return View();
        }

        // additional nurse-specific actions can be added here
    }
}
