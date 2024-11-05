using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace iCare.Controllers
{
    public class TreatmentRecordController : Controller
    {
        private readonly AppDbContext _context;

        public TreatmentRecordController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TreatmentRecord/ManageTreatment/5
        public IActionResult ManageTreatment(int patientId)
        {
            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            var treatmentRecords = _context.PatientRecords
                .Where(pr => pr.PatientId == patientId)
                .Include(pr => pr.Doctor)
                .ToList();

            ViewBag.Patient = patient;
            return View(treatmentRecords);
        }

        // GET: TreatmentRecord/Create/5
        public IActionResult Create(int patientId)
        {
            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            var patientRecord = new PatientRecord
            {
                PatientId = patientId,
                RecordDate = DateTime.Now
            };

            ViewBag.Patient = patient;
            ViewBag.Doctors = new SelectList(_context.Users.Where(u => u.Role.Equals("doctor", StringComparison.OrdinalIgnoreCase)), "Id", "Username");
            return View(patientRecord);
        }

        // POST: TreatmentRecord/Create
        [HttpPost]
        public IActionResult Create(PatientRecord patientRecord)
        {
            if (ModelState.IsValid)
            {
                _context.PatientRecords.Add(patientRecord);
                _context.SaveChanges();
                return RedirectToAction(nameof(ManageTreatment), new { patientId = patientRecord.PatientId });
            }
            // Repopulate ViewBag in case of validation errors
            ViewBag.Patient = _context.Patients.Find(patientRecord.PatientId);
            ViewBag.Doctors = new SelectList(_context.Users.Where(u => u.Role.Equals("doctor", StringComparison.OrdinalIgnoreCase)), "Id", "Username", patientRecord.DoctorId);
            return View(patientRecord);
        }

        // GET: TreatmentRecord/Edit/5
        public IActionResult Edit(int id)
        {
            var patientRecord = _context.PatientRecords.FirstOrDefault(pr => pr.RecordId == id);
            if (patientRecord == null)
            {
                return NotFound();
            }
            ViewBag.Doctors = new SelectList(_context.Users.Where(u => u.Role.Equals("doctor", StringComparison.OrdinalIgnoreCase)), "Id", "Username", patientRecord.DoctorId);
            return View(patientRecord);
        }

        // POST: TreatmentRecord/Edit/5
        [HttpPost]
        public IActionResult Edit(PatientRecord patientRecord)
        {
            if (ModelState.IsValid)
            {
                _context.PatientRecords.Update(patientRecord);
                _context.SaveChanges();
                return RedirectToAction(nameof(ManageTreatment), new { patientId = patientRecord.PatientId });
            }
            ViewBag.Doctors = new SelectList(_context.Users.Where(u => u.Role.Equals("doctor", StringComparison.OrdinalIgnoreCase)), "Id", "Username", patientRecord.DoctorId);
            return View(patientRecord);
        }

        // GET: TreatmentRecord/Delete/5
        public IActionResult Delete(int id)
        {
            var patientRecord = _context.PatientRecords.FirstOrDefault(pr => pr.RecordId == id);
            if (patientRecord == null)
            {
                return NotFound();
            }
            return View(patientRecord);
        }

        // POST: TreatmentRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var patientRecord = _context.PatientRecords.FirstOrDefault(pr => pr.RecordId == id);
            if (patientRecord != null)
            {
                _context.PatientRecords.Remove(patientRecord);
                _context.SaveChanges();
                return RedirectToAction(nameof(ManageTreatment), new { patientId = patientRecord.PatientId });
            }
            return NotFound();
        }
    }
}
