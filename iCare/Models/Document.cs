using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iCare.Models
{
    public class Document
    {
        [Key]
        [Column("doc_id")]
        public int DocId { get; set; }  // Maps to 'doc_id' in the database

        [Column("patient_id")]
        public int PatientId { get; set; }  // Maps to 'patient_id' in the database

        [Column("doc_name")]
        public string DocName { get; set; }  // Maps to 'doc_name' in the database

        [Column("doc_type")]
        public string DocType { get; set; }  // Maps to 'doc_type' in the database

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }  // Maps to 'created_at' in the database

        [Column("content")]
        public byte[] Content { get; set; }  // Maps to 'content' in the database

        // Navigation property
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
