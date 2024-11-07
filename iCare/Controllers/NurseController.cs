using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using System.Linq;

namespace iCare.Controllers
{
    [Authorize(Roles = "nurse")]
    public class NurseController : Controller
    {
        private readonly AppDbContext _context;

        public NurseController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult NurseDashboard()
        {
            // Fetch patients assigned to the logged-in nurse
            var nurseId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var assignedPatients = _context.UserPatients
                .Where(up => up.WorkerId == nurseId)
                .Select(up => up.Patient)
                .ToList();

            return View(assignedPatients);
        }

        public IActionResult ManagePatientCare(int patientId)
        {
            // Logic to allow nurses to manage patient care logs or view necessary information
            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            // Check if the nurse is assigned to this patient
            var nurseId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var isAssigned = _context.UserPatients.Any(up => up.PatientId == patientId && up.WorkerId == nurseId);

            if (!isAssigned)
            {
                return Unauthorized();
            }

            return View(patient);
        }
    }
}
