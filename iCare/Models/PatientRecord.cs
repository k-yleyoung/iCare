using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iCare.Models
{
    public class PatientRecord
    {
        [Key]
        [Column("record_id")]
        public int RecordId { get; set; }  // Maps to 'record_id' in the database

        [Column("patient_id")]
        public int PatientId { get; set; }  // Maps to 'patient_id' in the database

        [DataType(DataType.Date)]
        [Column("record_date")]
        public DateTime RecordDate { get; set; }  // Maps to 'record_date' in the database

        [Column("treatment_details")]
        public string TreatmentDetails { get; set; }  // Maps to 'treatment_details' in the database

        [Column("doctor_id")]
        public int? DoctorId { get; set; }  // Maps to 'doctor_id' in the database

        // Navigation properties
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        [ForeignKey("DoctorId")]
        public User Doctor { get; set; }
    }
}
