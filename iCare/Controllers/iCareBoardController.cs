// Controllers/iCareBoardController.cs
using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;
using System.Linq;
using System.Security.Claims;

namespace iCare.Controllers
{
    public class iCareBoardController : Controller
    {
        private readonly AppDbContext _context;

        public iCareBoardController(AppDbContext context)
        {
            _context = context;
        }

        // GET: iCareBoard
        public IActionResult Index()
        {
            var patients = _context.Patients.ToList();
            return View(patients);
        }

        // Assign patient
        [HttpPost]
        public IActionResult AssignPatients(int[] selectedPatientIds)
        {
            var workerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            foreach (var patientId in selectedPatientIds)
            {
                var assignmentExists = _context.UserPatients.Any(wp => wp.WorkerId == workerId && wp.PatientId == patientId);
                if (!assignmentExists)
                {
                    _context.UserPatients.Add(new UserPatient
                    {
                        WorkerId = workerId,
                        PatientId = patientId
                    });
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: myCareBoard
        public IActionResult myCareBoard()
        {
            var workerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var assignedPatients = _context.UserPatients
                .Where(up => up.WorkerId == workerId)
                .Select(up => up.Patient)
                .ToList();

            return View(assignedPatients);
        }
    }
}
