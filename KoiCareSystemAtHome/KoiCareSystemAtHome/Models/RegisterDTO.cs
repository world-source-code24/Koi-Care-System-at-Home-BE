namespace KoiCareSystemAtHome.Models
{
    public class RegisterDTO
    {
        public string Email { get; set; } = null!; 
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Confirm_Password { get; set; } = null!;

    }

}

