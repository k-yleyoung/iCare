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

        // constructor initializes the database context, environment, and logger for logging actions
        public iCareBoardController(AppDbContext context, IWebHostEnvironment env, ILogger<iCareBoardController> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        // displays the create patient view
        // input: none, output: patient creation page view
        public IActionResult Create()
        {
            return View();
        }

        // handles form submission to create a new patient
        // input: patient data, output: redirects to index if creation is successful, else returns view with errors
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("creating new patient: {PatientName}", patient.Name);
                _context.Patients.Add(patient);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "patient created successfully.";
                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("invalid model state for patient creation.");
            return View(patient);
        }

        // displays a list of patients
        // input: none, output: list page view with all patients
        public IActionResult Index()
        {
            _logger.LogInformation("fetching list of patients.");
            var patients = _context.Patients.ToList();
            return View(patients);
        }

        // displays the edit page for a specific patient
        // input: patient id, output: edit page view with patient details if found, else not found result
        public IActionResult Edit(int id)
        {
            _logger.LogInformation("fetching patient details for id {PatientId}.", id);
            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                _logger.LogWarning("patient with id {PatientId} not found.", id);
                return NotFound();
            }
            return View(patient);
        }

        // handles the form submission to edit a patient's details
        // input: updated patient data, output: redirects to index if successful, else returns view with errors
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("updating patient id {PatientId}.", patient.Id);
                _context.Update(patient);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("model state invalid for patient id {PatientId}.", patient.Id);
            return View(patient);
        }

        // displays a list of pdf records for a specific patient
        // input: patient id, output: records page view with list of pdfs
        public IActionResult Records(int patientId)
        {
            _logger.LogInformation("retrieving records for patient id {PatientId}.", patientId);
            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                _logger.LogWarning("patient with id {PatientId} not found.", patientId);
                return NotFound();
            }

            // get the folder path where pdfs for this patient are stored
            var pdfFolder = Path.Combine(_env.ContentRootPath, "PDFs", patientId.ToString());
            var pdfFiles = Directory.Exists(pdfFolder) ?
                Directory.GetFiles(pdfFolder).Select(Path.GetFileName).ToList() : new List<string>();

            ViewBag.PatientId = patientId;
            ViewBag.PatientName = patient.Name;
            return View(pdfFiles);
        }

        // handles creation of a new pdf for a specific patient
        // input: patient id and document content, output: redirects to records page or error status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePdf(int patientId, string documentContent)
        {
            if (string.IsNullOrWhiteSpace(documentContent))
            {
                _logger.LogWarning("document content is empty for patient id {PatientId}.", patientId);
                ModelState.AddModelError(string.Empty, "document content cannot be empty.");
                return RedirectToAction(nameof(Records), new { patientId });
            }

            _logger.LogInformation("creating pdf for patient id {PatientId}.", patientId);
            var patientFolder = Path.Combine(_env.ContentRootPath, "PDFs", patientId.ToString());
            if (!Directory.Exists(patientFolder))
            {
                _logger.LogInformation("creating directory for patient pdfs at {FolderPath}.", patientFolder);
                Directory.CreateDirectory(patientFolder);
            }

            var pdfPath = Path.Combine(patientFolder, $"{Guid.NewGuid()}.pdf");

            try
            {
                // create and save pdf with given content
                using (var writer = new PdfWriter(pdfPath))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new iText.Layout.Document(pdf);
                        document.Add(new Paragraph(documentContent));
                    }
                }
                _logger.LogInformation("pdf created successfully at {PdfPath}.", pdfPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occurred while creating pdf for patient id {PatientId}.", patientId);
                return StatusCode(500, "internal server error while creating pdf.");
            }

            return RedirectToAction(nameof(Records), new { patientId });
        }

        // assigns selected patients to the current logged-in doctor or nurse
        // input: array of selected patient ids, output: redirects to index after assigning
        [HttpPost]
        public IActionResult AssignPatients(int[] selectedPatientIds)
        {
            if (User.IsInRole("Doctor") || User.IsInRole("Nurse"))
            {
                var workerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _logger.LogInformation("assigning patients to user id {UserId}.", workerId);

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
                        _logger.LogInformation("assigned patient id {PatientId} to worker id {WorkerId}.", patientId, workerId);
                    }
                }
                _context.SaveChanges();
            }
            else
            {
                _logger.LogWarning("unauthorized attempt to assign patients by user id {UserId}.", User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            return RedirectToAction("Index");
        }

        // displays the my care board for the logged-in worker (doctor or nurse) with assigned patients
        // input: none, output: my care board view with assigned patients
        public IActionResult myCareBoard()
        {
            var workerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _logger.LogInformation("loading assigned patients for worker id {WorkerId}.", workerId);

            var assignedPatients = _context.UserPatients
                .Where(up => up.WorkerId == workerId)
                .Select(up => up.Patient)
                .ToList();

            return View(assignedPatients);
        }

        // displays patient details from either iCareBoard or myCareBoard view
        // input: patient id, output: view with patient details if found, else not found result
        [HttpGet]
        [Route("iCareBoard/ViewPatient/{id}")]
        [Route("myCareBoard/ViewPatient/{id}")]
        public IActionResult ViewPatient(int id)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // displays a list of documents associated with a specific patient
        // input: patient id, output: manage documents view with list of documents
        public IActionResult ManageDocuments(int patientId)
        {
            var documents = _context.Documents.Where(d => d.PatientId == patientId).ToList();
            ViewBag.PatientId = patientId;
            return View(documents);
        }

        // displays treatment records for a specific patient
        // input: patient id, output: manage treatment view with list of treatment records
        public IActionResult ManageTreatment(int patientId)
        {
            var treatmentRecords = _context.PatientRecords.Where(r => r.PatientId == patientId).ToList();
            ViewBag.PatientId = patientId;
            return View(treatmentRecords);
        }
    }
}
