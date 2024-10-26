using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iCare.Controllers
{
    [Authorize(Roles = "doctor")]
    public class DoctorController : Controller
    {
        public IActionResult DoctorDashboard()
        {
            
            return View();
        }

        public IActionResult ViewPatientRecords()
        {
            
            return View();
        }

        // Add other doctor-specific actions here
    }
}
