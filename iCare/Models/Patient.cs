using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iCare.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }  // Matches 'id' in Patients table

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }  // Matches 'name' in Patients table

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }  // Matches 'DateOfBirth' in Patients table

        [StringLength(255)]
        public string? Address { get; set; }  // Matches 'address' in Patients table

        [StringLength(5)]
        public string? BloodGroup { get; set; }  // Matches 'BloodGroup' in Patients table

        [StringLength(100)]
        public string? TreatmentArea { get; set; }  // Matches 'TreatmentArea' in Patients table

        // Navigation properties
        public ICollection<UserPatient> UserPatients { get; set; } = new List<UserPatient>();
        public ICollection<PatientRecord> PatientRecords { get; set; } = new List<PatientRecord>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();

        // Not mapped properties for form binding
        [NotMapped]
        public int? AssignedDoctorId { get; set; }

        [NotMapped]
        public int? AssignedNurseId { get; set; }

        [NotMapped]
        public string AssignedDoctorName { get; set; } = string.Empty;

        [NotMapped]
        public string AssignedNurseName { get; set; } = string.Empty;
    }
}
