namespace iCare.Models
{
    public class Document
    {
        public int DocId { get; set; }
        public int PatientId { get; set; }
        public string DocName { get; set; }
        public string DocType { get; set; }
        public DateTime CreatedAt { get; set; }

        public Patient Patient { get; set; }
    }
}

