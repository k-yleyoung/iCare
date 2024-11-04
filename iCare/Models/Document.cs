namespace iCare.Models
{
    public class Document
    {
        public int Id { get; set; }
        public required string FileName { get; set; }
        public required byte[] FileContent { get; set; }
        public required string ContentType { get; set; }
        public int PatientId { get; internal set; }
    }
}