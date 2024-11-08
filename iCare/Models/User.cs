namespace iCare.Models
{
    public class User
    {
        // unique identifier for the user
        public int Id { get; set; }

        // username of the user for login purposes
        public string Username { get; set; }

        // password of the user for authentication
        public string Password { get; set; }

        // role of the user (e.g., "doctor", "nurse"), used for authorization
        public string Role { get; set; }

        // collection of user-patient relationships, allowing association with multiple patients
        public ICollection<UserPatient>? UserPatients { get; set; }
    }
}
