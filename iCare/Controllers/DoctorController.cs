using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iCare.Controllers
{
    // restricts access to this controller to users with the "doctor" role
    [Authorize(Roles = "doctor")]
    public class DoctorController : Controller
    {
        // displays the doctor dashboard view
        // input: none, output: doctor dashboard page view
        public IActionResult DoctorDashboard()
        {
            return View();
        }

        // displays the view patient records page
        // input: none, output: patient records page view, where doctors can view patient information
        public IActionResult ViewPatientRecords()
        {
            return View();
        }
    }
}
