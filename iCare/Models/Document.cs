using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iCare.Models
{
    public class Document
    {
        [Key]
        public int DocId { get; set; }  // Maps to doc_id in Documents table

        [Required]
        public int PatientId { get; set; }  // Maps to patient_id in Documents table

        [Required]
        [StringLength(255)]
        public string DocName { get; set; } = string.Empty;  // Name of the document

        [Required]
        [StringLength(50)]
        public string DocType { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Creation timestamp

        [Required]
        public byte[] Content { get; set; }  // Stores either text as bytes or image data

        // Navigation property
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
