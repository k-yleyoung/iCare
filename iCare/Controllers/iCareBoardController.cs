using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;
using System.IO;
using System.Linq;
using System.Security.Claims;
using iText.Kernel.Pdf; // iText 7
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace iCare.Controllers
{
    public class iCareBoardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<iCareBoardController> _logger;

        public iCareBoardController(AppDbContext context, IWebHostEnvironment env, ILogger<iCareBoardController> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        // GET: iCareBoard/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: iCareBoard/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creating new patient: {PatientName}", patient.Name);
                _context.Patients.Add(patient);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Patient created successfully.";
                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("Invalid model state for patient creation.");
            return View(patient);
        }

        // GET: iCareBoard
        public IActionResult Index()
        {
            _logger.LogInformation("Fetching list of patients.");
            var patients = _context.Patients.ToList();
            return View(patients);
        }

        // GET: iCareBoard/Edit/5
        public IActionResult Edit(int id)
        {
            _logger.LogInformation("Fetching patient details for ID {PatientId}.", id);
            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found.", id);
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
                _logger.LogInformation("Updating patient ID {PatientId}.", patient.Id);
                _context.Update(patient);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("Model state invalid for patient ID {PatientId}.", patient.Id);
            return View(patient);
        }

        // GET: iCareBoard/Records/5
        public IActionResult Records(int patientId)
        {
            _logger.LogInformation("Retrieving records for patient ID {PatientId}.", patientId);
            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found.", patientId);
                return NotFound();
            }

            var pdfFolder = Path.Combine(_env.ContentRootPath, "PDFs", patientId.ToString());
            var pdfFiles = Directory.Exists(pdfFolder) ?
                Directory.GetFiles(pdfFolder).Select(Path.GetFileName).ToList() : new List<string>();

            ViewBag.PatientId = patientId;
            ViewBag.PatientName = patient.Name;
            return View(pdfFiles);
        }

        // POST: iCareBoard/CreatePdf
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePdf(int patientId, string documentContent)
        {
            if (string.IsNullOrWhiteSpace(documentContent))
            {
                _logger.LogWarning("Document content is empty for patient ID {PatientId}.", patientId);
                ModelState.AddModelError(string.Empty, "Document content cannot be empty.");
                return RedirectToAction(nameof(Records), new { patientId });
            }

            _logger.LogInformation("Creating PDF for patient ID {PatientId}.", patientId);
            var patientFolder = Path.Combine(_env.ContentRootPath, "PDFs", patientId.ToString());
            if (!Directory.Exists(patientFolder))
            {
                _logger.LogInformation("Creating directory for patient PDFs at {FolderPath}.", patientFolder);
                Directory.CreateDirectory(patientFolder);
            }

            var pdfPath = Path.Combine(patientFolder, $"{Guid.NewGuid()}.pdf");

            try
            {
                using (var writer = new PdfWriter(pdfPath))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new iText.Layout.Document(pdf);
                        document.Add(new Paragraph(documentContent));
                    }
                }
                _logger.LogInformation("PDF created successfully at {PdfPath}.", pdfPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating PDF for patient ID {PatientId}.", patientId);
                return StatusCode(500, "Internal server error while creating PDF.");
            }

            return RedirectToAction(nameof(Records), new { patientId });
        }

        // POST: iCareBoard/AssignPatients
        [HttpPost]
        public IActionResult AssignPatients(int[] selectedPatientIds)
        {
            if (User.IsInRole("Doctor") || User.IsInRole("Nurse"))
            {
                var workerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _logger.LogInformation("Assigning patients to user ID {UserId}.", workerId);

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
                        _logger.LogInformation("Assigned patient ID {PatientId} to worker ID {WorkerId}.", patientId, workerId);
                    }
                }
                _context.SaveChanges();
            }
            else
            {
                _logger.LogWarning("Unauthorized attempt to assign patients by user ID {UserId}.", User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            return RedirectToAction("Index");
        }

        // GET: myCareBoard
        public IActionResult myCareBoard()
        {
            var workerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _logger.LogInformation("Loading assigned patients for worker ID {WorkerId}.", workerId);

            var assignedPatients = _context.UserPatients
                .Where(up => up.WorkerId == workerId)
                .Select(up => up.Patient)
                .ToList();

            return View(assignedPatients);
        }

        // GET: iCareBoard/ViewPatient/5
        public IActionResult ViewPatient(int id)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: iCareBoard/ManageDocuments/5
        public IActionResult ManageDocuments(int patientId)
        {
            var documents = _context.Documents.Where(d => d.PatientId == patientId).ToList();
            ViewBag.PatientId = patientId;
            return View(documents);
        }

        // GET: iCareBoard/ManageTreatment/5
        public IActionResult ManageTreatment(int patientId)
        {
            var treatmentRecords = _context.PatientRecords.Where(r => r.PatientId == patientId).ToList();
            ViewBag.PatientId = patientId;
            return View(treatmentRecords);
        }
    }
}
