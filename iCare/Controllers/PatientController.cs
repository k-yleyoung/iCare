using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;

namespace iCare.Controllers
{
    public class PatientController : Controller
    {
        private readonly AppDbContext _context;

        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
                return RedirectToAction(nameof(List));
            }
            return View(patient);
        }

        public IActionResult List()
        {
            var patients = _context.Patients.ToList();
            return View(patients);
        }
    }
}
