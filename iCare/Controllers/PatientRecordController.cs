﻿using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;

namespace iCare.Controllers
{
    public class PatientRecordController : Controller
    {
        private readonly AppDbContext _context;

        public PatientRecordController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PatientRecord patientRecord)
        {
            if (ModelState.IsValid)
            {
                _context.PatientRecords.Add(patientRecord);
                _context.SaveChanges();
                return RedirectToAction(nameof(List));
            }
            return View(patientRecord);
        }

        public IActionResult List()
        {
            var patientRecords = _context.PatientRecords.ToList();
            return View(patientRecords);
        }
    }
}