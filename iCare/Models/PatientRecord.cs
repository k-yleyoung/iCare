namespace iCare.Models
{
    public class PatientRecord
    {
        public int Id { get; set; }
        public required int RecordId { get; set; }
        public required int PatientId { get; set; }
        public DateTime RecordDate { get; set; }
        public required string TreatmentDetails { get; set; }
        public required int DoctorId { get; set; }

        public required Patient Patient { get; set; }
        public required User Doctor { get; set; }
    }
}
