using iCare.Data;
using iCare.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iCare.Repositories
{
    public class DocumentRepository
    {
        private readonly AppDbContext _context;

        public DocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Document>> GetDocumentsByPatientIdAsync(int patientId)
        {
            return await _context.Documents
                .Where(d => d.PatientId == patientId)
                .ToListAsync();
        }

        public async Task AddDocumentAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
        }
    }
}
