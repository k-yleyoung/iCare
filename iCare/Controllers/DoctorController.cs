using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using System.Linq;

namespace iCare.Controllers
{
    [Authorize(Roles = "doctor")]
    public class DoctorController : Controller
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult DoctorDashboard()
        {
            // Fetch patients assigned to the logged-in doctor
            var doctorId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var assignedPatients = _context.UserPatients
                .Where(up => up.WorkerId == doctorId)
                .Select(up => up.Patient)
                .ToList();

            return View(assignedPatients);
        }

        public IActionResult ViewPatientRecords(int patientId)
        {
            // Logic to allow doctors to view patient records
            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            // Check if the doctor is assigned to this patient
            var doctorId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var isAssigned = _context.UserPatients.Any(up => up.PatientId == patientId && up.WorkerId == doctorId);

            if (!isAssigned)
            {
                return Unauthorized();
            }

            var patientRecords = _context.PatientRecords
                .Where(pr => pr.PatientId == patientId)
                .ToList();

            ViewBag.Patient = patient;
            return View(patientRecords);
        }
    }
}
