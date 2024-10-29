namespace iCare.Models

{
    public class Patient
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Address { get; set; }
        public required string BloodGroup { get; set; }
        public required string TreatmentArea { get; set; }
        public ICollection<UserPatient>? WorkerPatients { get; set; }
    }
}