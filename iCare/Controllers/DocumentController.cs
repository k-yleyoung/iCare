using Microsoft.AspNetCore.Mvc;
using iCare.Data;
using iCare.Models;

namespace iCare.Controllers
{
    public class DocumentController : Controller
    {
        private readonly AppDbContext _context;

        public DocumentController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    var document = new Document
                    {
                        FileName = file.FileName,
                        FileContent = fileBytes,
                        ContentType = file.ContentType
                    };

                    _context.Documents.Add(document);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction(nameof(List));
        }

        public IActionResult List()
        {
            var documents = _context.Documents.ToList();
            return View(documents);
        }
    }
}