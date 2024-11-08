using Microsoft.EntityFrameworkCore;
using iCare.Models;

namespace iCare.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<UserPatient> UserPatients { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite key for UserPatient
            modelBuilder.Entity<UserPatient>()
                .HasKey(up => new { up.WorkerId, up.PatientId });
            // Configure relationships
            modelBuilder.Entity<UserPatient>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPatients)
                .HasForeignKey(up => up.WorkerId);
            modelBuilder.Entity<UserPatient>()
                .HasOne(up => up.Patient)
                .WithMany(p => p.UserPatients)
                .HasForeignKey(up => up.PatientId);
            modelBuilder.Entity<PatientRecord>()
                .HasOne(pr => pr.Patient)
                .WithMany(p => p.PatientRecords)
                .HasForeignKey(pr => pr.PatientId);
            modelBuilder.Entity<PatientRecord>()
                .HasOne(pr => pr.Doctor)
                .WithMany(u => u.PatientRecords)
                .HasForeignKey(pr => pr.DoctorId);
            modelBuilder.Entity<Document>()
                .HasOne(d => d.Patient)
                .WithMany(p => p.Documents)
                .HasForeignKey(d => d.PatientId);
        }
    }
}
