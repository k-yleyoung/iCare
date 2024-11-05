using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace iCare.Controllers
{
    public class PatientController : Controller
    {
        private readonly AppDbContext _context;

        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Patient/List
        public IActionResult List()
        {
            var patients = _context.Patients.ToList();
            return View(patients);
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            ViewBag.Doctors = new SelectList(_context.Users.Where(u => u.Role.Equals("doctor", StringComparison.OrdinalIgnoreCase)), "Id", "Username");
            ViewBag.Nurses = new SelectList(_context.Users.Where(u => u.Role.Equals("nurse", StringComparison.OrdinalIgnoreCase)), "Id", "Username");
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public IActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();

                // Assign the selected doctor and nurse to the patient
                if (patient.AssignedDoctorId.HasValue)
                {
                    _context.UserPatients.Add(new UserPatient
                    {
                        WorkerId = patient.AssignedDoctorId.Value,
                        PatientId = patient.Id
                    });
                }
                if (patient.AssignedNurseId.HasValue)
                {
                    _context.UserPatients.Add(new UserPatient
                    {
                        WorkerId = patient.AssignedNurseId.Value,
                        PatientId = patient.Id
                    });
                }
                _context.SaveChanges();

                return RedirectToAction(nameof(List));
            }
            // Repopulate dropdowns if validation fails
            ViewBag.Doctors = new SelectList(_context.Users.Where(u => u.Role.Equals("doctor", StringComparison.OrdinalIgnoreCase)), "Id", "Username");
            ViewBag.Nurses = new SelectList(_context.Users.Where(u => u.Role.Equals("nurse", StringComparison.OrdinalIgnoreCase)), "Id", "Username");
            return View(patient);
        }

        // GET: Patient/Edit/5
        public IActionResult Edit(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            // Include User in UserPatients
            var doctorAssignment = _context.UserPatients
                .Include(up => up.User)
                .FirstOrDefault(up => up.PatientId == id && up.User.Role.Equals("doctor", StringComparison.OrdinalIgnoreCase));

            var nurseAssignment = _context.UserPatients
                .Include(up => up.User)
                .FirstOrDefault(up => up.PatientId == id && up.User.Role.Equals("nurse", StringComparison.OrdinalIgnoreCase));

            patient.AssignedDoctorId = doctorAssignment?.WorkerId;
            patient.AssignedNurseId = nurseAssignment?.WorkerId;

            ViewBag.Doctors = new SelectList(_context.Users.Where(u => u.Role.Equals("doctor", StringComparison.OrdinalIgnoreCase)), "Id", "Username", patient.AssignedDoctorId);
            ViewBag.Nurses = new SelectList(_context.Users.Where(u => u.Role.Equals("nurse", StringComparison.OrdinalIgnoreCase)), "Id", "Username", patient.AssignedNurseId);

            return View(patient);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        public IActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Patients.Update(patient);
                _context.SaveChanges();

                // Update doctor assignment
                var doctorAssignment = _context.UserPatients
                    .Include(up => up.User)
                    .FirstOrDefault(up => up.PatientId == patient.Id && up.User.Role.Equals("doctor", StringComparison.OrdinalIgnoreCase));
                if (doctorAssignment != null)
                {
                    _context.UserPatients.Remove(doctorAssignment);
                }
                if (patient.AssignedDoctorId.HasValue)
                {
                    _context.UserPatients.Add(new UserPatient
                    {
                        WorkerId = patient.AssignedDoctorId.Value,
                        PatientId = patient.Id
                    });
                }

                // Update nurse assignment
                var nurseAssignment = _context.UserPatients
                    .Include(up => up.User)
                    .FirstOrDefault(up => up.PatientId == patient.Id && up.User.Role.Equals("nurse", StringComparison.OrdinalIgnoreCase));
                if (nurseAssignment != null)
                {
                    _context.UserPatients.Remove(nurseAssignment);
                }
                if (patient.AssignedNurseId.HasValue)
                {
                    _context.UserPatients.Add(new UserPatient
                    {
                        WorkerId = patient.AssignedNurseId.Value,
                        PatientId = patient.Id
                    });
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(List));
            }
            // Repopulate dropdowns if validation fails
            ViewBag.Doctors = new SelectList(_context.Users.Where(u => u.Role.Equals("doctor", StringComparison.OrdinalIgnoreCase)), "Id", "Username", patient.AssignedDoctorId);
            ViewBag.Nurses = new SelectList(_context.Users.Where(u => u.Role.Equals("nurse", StringComparison.OrdinalIgnoreCase)), "Id", "Username", patient.AssignedNurseId);
            return View(patient);
        }

        // GET: Patient/ViewPatient/5
        public IActionResult ViewPatient(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            // Get assigned doctor and nurse names
            var doctorAssignment = _context.UserPatients
                .Include(up => up.User)
                .Where(up => up.PatientId == id && up.User.Role.Equals("doctor", StringComparison.OrdinalIgnoreCase))
                .Select(up => up.User.Username)
                .FirstOrDefault();

            var nurseAssignment = _context.UserPatients
                .Include(up => up.User)
                .Where(up => up.PatientId == id && up.User.Role.Equals("nurse", StringComparison.OrdinalIgnoreCase))
                .Select(up => up.User.Username)
                .FirstOrDefault();

            patient.AssignedDoctorName = doctorAssignment ?? "Not Assigned";
            patient.AssignedNurseName = nurseAssignment ?? "Not Assigned";

            return View(patient);
        }
    }
}
