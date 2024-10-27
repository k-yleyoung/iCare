// Controllers/iCareBoardController.cs
using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;
using System.Linq;

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
            // Logic to assign selected patients to the logged-in user
            // This is a placeholder for your assignment logic
            return RedirectToAction("Index");
        }
    }
}
