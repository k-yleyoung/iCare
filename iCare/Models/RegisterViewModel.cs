namespace iCare.Models
{
    /// ViewModel for Register
    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }  // Doctor, Nurse, Admin etc.
    }
}