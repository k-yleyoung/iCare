using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iCare.Models
{
    public class UserPatient
    {
        [Key, Column(Order = 0)]
        public int WorkerId { get; set; }  // Matches 'WorkerId' in UserPatients table

        [Key, Column(Order = 1)]
        public int PatientId { get; set; }  // Matches 'PatientId' in UserPatients table

        public DateTime AssignedAt { get; set; }  // Matches 'AssignedAt' in UserPatients table

        // Navigation properties
        [ForeignKey("WorkerId")]
        public User User { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
