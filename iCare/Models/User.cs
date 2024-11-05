using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iCare.Models
{
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }  // Maps to 'id' in the database

        [Required]
        [StringLength(50)]
        [Column("username")]
        public string Username { get; set; }  // Maps to 'username' in the database

        [Required]
        [StringLength(100)]
        [Column("password")]
        public string Password { get; set; }  // Maps to 'password' in the database

        [Required]
        [Column("role")]
        public string Role { get; set; }  // Maps to 'role' in the database

        // Navigation properties
        public ICollection<UserPatient> UserPatients { get; set; }
        public ICollection<PatientRecord> PatientRecords { get; set; }
    }
}
