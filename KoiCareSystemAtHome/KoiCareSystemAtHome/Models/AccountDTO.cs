namespace KoiCareSystemAtHome.Models
{
    public class AccountDTO
    {
        public int AccId { get; set; }

        public string? Name { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Image { get; set; }

        public string Phone { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Role { get; set; } = null!;

        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public bool Status { get; set; }
    }
}
