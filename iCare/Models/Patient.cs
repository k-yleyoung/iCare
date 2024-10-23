namespace iCare.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public string TreatmentArea { get; set; }
    }
}
