namespace iCare.Models
{
    public class PatientRecord
    {
        public int RecordId { get; set; }
        public int PatientId { get; set; }
        public DateTime RecordDate { get; set; }
        public string TreatmentDetails { get; set; }
        public int DoctorId { get; set; }

        public Patient Patient { get; set; }
        public User Doctor { get; set; }
    }
}