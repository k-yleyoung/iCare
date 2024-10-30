﻿// Controllers/iCareBoardController.cs
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

        // GET: iCareBoard/Edit/5
        public IActionResult Edit(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: iCareBoard/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Update(patient);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // Assign patient
        [HttpPost]
        public IActionResult AssignPatients(int[] selectedPatientIds)
        {
            // Check if the user has the role of Doctor or Nurse
            if (User.IsInRole("Doctor") || User.IsInRole("Nurse"))
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
            }
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
