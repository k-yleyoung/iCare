using Microsoft.EntityFrameworkCore;
using iCare.Models;

namespace iCare.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<UserPatient> UserPatients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPatient>()
                .HasKey(up => new { up.WorkerId, up.PatientId }); // Composite key
        }
    }
}
