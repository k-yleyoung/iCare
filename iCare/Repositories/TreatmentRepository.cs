using iCare.Data;
using iCare.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iCare.Repositories
{
    public class TreatmentRepository
    {
        private readonly AppDbContext _context;

        public TreatmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PatientRecord>> GetRecordsByPatientIdAsync(int patientId)
        {
            return await _context.PatientRecords
                .Where(pr => pr.PatientId == patientId)
                .Include(pr => pr.Doctor)
                .ToListAsync();
        }

        public async Task AddRecordAsync(PatientRecord patientRecord)
        {
            await _context.PatientRecords.AddAsync(patientRecord);
            await _context.SaveChangesAsync();
        }
    }
}
