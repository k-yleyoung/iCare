using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;

namespace iCare.Controllers
{
    public class PatientController : Controller
    {
        private readonly AppDbContext _context;

        // constructor takes the app database context as a parameter
        // allows access to the patients table for creating and listing patients
        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        // displays the create patient view
        // output: page view for creating a new patient
        public IActionResult Create()
        {
            return View();
        }

        // handles form submission to create a new patient
        // input: patient object with patient details, output: redirects to patient list if successful, else reloads view with errors
        [HttpPost]
        public IActionResult Create(Patient patient)
        {
            // check if model state is valid before saving patient to database
            if (ModelState.IsValid)
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
                return RedirectToAction(nameof(List));
            }
            // if model state is invalid, return view with validation errors
            return View(patient);
        }

        // displays a list of all patients
        // output: list page view with all patients
        public IActionResult List()
        {
            var patients = _context.Patients.ToList(); // fetch all patients from database
            return View(patients);
        }
    }
}
