namespace iCare.Models
{
    public class Document
    {
        public int Id { get; set; }
        public required string FileName { get; set; }// name of the file being uploaded
        public required byte[] FileContent { get; set; } // binary content of the file
        public required string ContentType { get; set; } // content type
        public int PatientId { get; set; } // Change to public
    }
}
