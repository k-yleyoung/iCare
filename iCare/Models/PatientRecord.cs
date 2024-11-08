namespace iCare.Models
{
    public class PatientRecord
    {
        // unique identifier for the patient record
        public int Id { get; set; }

        // unique identifier for the specific record (required)
        public required int RecordId { get; set; }

        // id of the associated patient (required)
        public required int PatientId { get; set; }

        // date when the record was created or updated
        public DateTime RecordDate { get; set; }

        // details about the treatment provided to the patient (required)
        public required string TreatmentDetails { get; set; }

        // id of the doctor who provided the treatment (required)
        public required int DoctorId { get; set; }

        // navigation property linking to the patient associated with this record (required)
        public required Patient Patient { get; set; }

        // navigation property linking to the doctor (user) associated with this record (required)
        public required User Doctor { get; set; }
    }
}
