using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iCare.Controllers
{
    [Authorize(Roles = "nurse")]
    public class NurseController : Controller
    {
        public IActionResult NurseDashboard()
        {
            // Add logic specific to the nurse dashboard
            return View();
        }

        public IActionResult ManagePatientCare()
        {
            // Logic to allow nurses to manage patient care logs or view necessary information
            return View();
        }

        // Add other nurse-specific actions here
    }
}
