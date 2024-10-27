using Microsoft.EntityFrameworkCore;
using iCare.Models;

namespace iCare.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor with options parameter
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets for your models
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public DbSet<Document> Documents { get; set; }
    }
}
