// DocumentController.cs

using iCare.Data;
using iCare.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IO;

namespace iCare.Controllers
{
    public class DocumentController : Controller
    {
        private readonly AppDbContext _context;

        public DocumentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Document/ManageDocuments/5
        public IActionResult ManageDocuments(int patientId)
        {
            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            var documents = _context.Documents
                .Where(d => d.PatientId == patientId)
                .ToList();

            ViewBag.Patient = patient;
            return View(documents);
        }

        // GET: Document/UploadDocument/5
        public IActionResult UploadDocument(int patientId)
        {
            var model = new UploadDocumentViewModel
            {
                PatientId = patientId
            };
            return View(model);
        }

        // POST: Document/UploadDocument
        [HttpPost]
        public async Task<IActionResult> UploadDocument(UploadDocumentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.File != null && model.File.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.File.CopyToAsync(memoryStream);

                        var document = new Document
                        {
                            PatientId = model.PatientId,
                            DocName = model.File.FileName,
                            DocType = model.File.ContentType,
                            CreatedAt = DateTime.Now,
                            Content = memoryStream.ToArray()
                        };

                        _context.Documents.Add(document);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("ManageDocuments", new { patientId = model.PatientId });
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "Please select a file to upload.");
                }
            }

            ViewBag.PatientId = model.PatientId;
            return View(model);
        }

        // GET: Document/DownloadDocument/5
        public async Task<IActionResult> DownloadDocument(int id)
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
