using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace iCare.Controllers
{
    public class DocumentController : Controller
    {
        private readonly AppDbContext _context;

        public DocumentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Document/List/5
        public IActionResult List(int patientId)
        {
            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            var documents = _context.Documents
                .Where(d => d.PatientId == patientId)
                .ToList();

            ViewBag.PatientName = patient.Name;
            ViewBag.PatientId = patientId;
            return View(documents);
        }

        // GET: Document/Create/5
        public IActionResult Create(int patientId)
        {
            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            ViewBag.PatientName = patient.Name;
            ViewBag.PatientId = patientId;
            return View();
        }

        // POST: Document/Create
        [HttpPost]
        public async Task<IActionResult> Create(int patientId, string documentContent, IFormFile documentImage)
        {
            if (string.IsNullOrWhiteSpace(documentContent) && (documentImage == null || documentImage.Length == 0))
            {
                ModelState.AddModelError("", "Please provide either text content or an image file.");
                ViewBag.PatientId = patientId;
                return View();
            }

            var document = new Document
            {
                PatientId = patientId,
                CreatedAt = DateTime.Now
            };

            if (!string.IsNullOrWhiteSpace(documentContent))
            {
                // Handle text content upload
                document.DocName = $"TextRecord_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                document.DocType = "text/plain";
                document.Content = System.Text.Encoding.UTF8.GetBytes(documentContent);
            }
            else if (documentImage != null && documentImage.Length > 0)
            {
                // Handle file upload (image)
                using (var memoryStream = new MemoryStream())
                {
                    await documentImage.CopyToAsync(memoryStream);
                    document.DocName = documentImage.FileName;
                    document.DocType = documentImage.ContentType;
                    document.Content = memoryStream.ToArray();
                }
            }

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(List), new { patientId });
        }

        // GET: Document/Download/5
        public async Task<IActionResult> Download(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            return File(document.Content, document.DocType, document.DocName);
        }
    }
}
