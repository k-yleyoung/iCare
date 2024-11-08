using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        // GET: Show the upload form
        public IActionResult UploadDocument(int patientId)
        {
            ViewBag.PatientId = patientId; // Pass the patientId to the view
            return View();
        }

        // POST: Handle the file upload
        [HttpPost]
        public async Task<IActionResult> UploadDocument(IFormFile Document, int patientId)
        {
            if (Document != null && Document.Length > 0)
            {
                // Define the path where you want to save the file
                var fileName = Path.GetFileName(Document.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                // Save the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Document.CopyToAsync(stream);
                }

                // Insert document information into the database if needed
                // For example, using a `PatientDocuments` table:
                // var document = new PatientDocument { PatientId = patientId, FileName = fileName, FilePath = "/uploads/" + fileName, DocumentType = "Uploaded" };
                // _context.PatientDocuments.Add(document);
                // _context.SaveChanges();

                return RedirectToAction(nameof(List)); // Redirect back to the list view
            }

            return View();
        }
    }
}
