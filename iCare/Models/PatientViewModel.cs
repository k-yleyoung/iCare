namespace iCare.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Address { get; set; }
        public required string BloodGroup { get; set; }
        public required string TreatmentArea { get; set; }
    }
}