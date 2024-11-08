using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace iCare.Controllers
{
    public class DocumentController : Controller
    {
        private readonly AppDbContext _context;

        // constructor takes the app database context as a parameter
        // allows access to the documents table for uploading and listing documents
        public DocumentController(AppDbContext context)
        {
            _context = context;
        }

        // displays the upload document view
        // input: patientId of the patient for whom the document is being uploaded
        // output: upload page view with the patient id set in ViewBag
        public IActionResult Upload(int patientId)
        {
            ViewBag.PatientId = patientId;
            return View();
        }

        // handles the file upload form submission
        // input: patientId for association, file (the document to upload)
        // output: if upload is successful, redirects to the document list page for the patient
        [HttpPost]
        public IActionResult Upload(int patientId, IFormFile file)
        {
            // check if a file was uploaded and it has content
            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    // copy file contents to a memory stream
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray(); // convert file to byte array

                    // create a new document object with the file details
                    var document = new Document
                    {
                        PatientId = patientId, // associate document with the patient
                        FileName = file.FileName,
                        FileContent = fileBytes,
                        ContentType = file.ContentType
                    };

                    // add the document to the database and save changes
                    _context.Documents.Add(document);
                    _context.SaveChanges();
                }
            }

            // redirect to the list page for documents associated with the patient
            return RedirectToAction("List", new { patientId = patientId });
        }

        // displays the list of documents for a specific patient
        // input: patientId to filter the documents by patient
        // output: list page view with documents for the specified patient
        public IActionResult List(int patientId)
        {
            // retrieve documents associated with the given patient id
            var documents = _context.Documents.Where(d => d.PatientId == patientId).ToList();
            ViewBag.PatientId = patientId;
            return View(documents);
        }
    }
}
