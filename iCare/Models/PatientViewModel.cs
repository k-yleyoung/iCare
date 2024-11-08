namespace iCare.Models
{
    public class PatientViewModel
    {
        // unique identifier for the patient
        public int Id { get; set; }

        // name of the patient (required)
        public required string Name { get; set; }

        // date of birth of the patient
        public DateTime DateOfBirth { get; set; }

        // address of the patient (required)
        public required string Address { get; set; }

        // blood group of the patient (required)
        public required string BloodGroup { get; set; }

        // primary area or department where the patient receives treatment (required)
        public required string TreatmentArea { get; set; }
    }
}
